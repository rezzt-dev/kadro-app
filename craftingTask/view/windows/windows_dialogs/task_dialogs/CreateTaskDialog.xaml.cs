using craftingTask.model.objects;
using craftingTask.persistence.managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Panel = craftingTask.model.objects.Panel;
using Task = craftingTask.model.objects.Task;

namespace craftingTask.view.windows.windows_dialogs.task_dialogs
{
  /// <summary>
  /// Lógica de interacción para CreateTaskDialog.xaml
  /// </summary>
  public partial class CreateTaskDialog : Window, INotifyPropertyChanged
  {
    private long selectedPanelId;
    private string selectedPanelColor;
    private long selectedStatusId;

    public event Action TaskCreated;

    private int selectedPriority;
    public int SelectedPriority
    {
      get => selectedPriority;
      set { selectedPriority = value; OnPropertyChanged();  }
    }
    public List<KeyValuePair<int, string>> PriorityList { get; set; }

    private ObservableCollection<Status> statusList { get; set; } = new ObservableCollection<Status>();

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public CreateTaskDialog(Panel inputSelectedPanel)
    {
      InitializeComponent();

      this.selectedPanelId = inputSelectedPanel.PanelId;
      this.selectedPanelColor = inputSelectedPanel.Color;

      StatusManager statusManager = new StatusManager();
      statusList = new ObservableCollection<Status>(statusManager.GetAllStatus());

      PriorityList = new List<KeyValuePair<int, string>>()
      {
        new KeyValuePair<int, string>(1, "Baja"),
        new KeyValuePair<int, string>(2, "Media"),
        new KeyValuePair<int, string>(3, "Alta"),
        new KeyValuePair<int, string>(4, "Inmediata")
       };
      SelectedPriority = PriorityList.First().Key;
      DataContext = this;
    }

    private void datePickerTaskDeadlineSelector_Loaded(object sender, RoutedEventArgs e)
    {
      var datePicker = sender as DatePicker;
      if (datePicker != null)
      {
        var textBox = FindVisualChild<DatePickerTextBox>(datePicker);
        if (textBox != null)
        {
          textBox.IsReadOnly = true;
        }
      }
    }

    private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
    {
      for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
      {
        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
        if (child != null && child is childItem)
          return (childItem)child;
        else
        {
          childItem childOfChild = FindVisualChild<childItem>(child);
          if (childOfChild != null)
            return childOfChild;
        }
      }
      return null;
    }

    private void btnMinimizeWindowClick(object sender, RoutedEventArgs e)
    {
      this.WindowState = WindowState.Minimized;
    }
    private void btnCloseWindowClick(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
    private void btnCancelCreation_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void btnCreateTask_Click(object sender, RoutedEventArgs e)
    {
      string taskTitle = txtTaskTitleSelector.Text.Trim();
      string taskDescription = txtTaskDescriptionSelector.Text.Trim();
      string taskTag = txtTaskTagSelector.Text.Trim();

      DateTime? endDate = datePickerTaskDeadlineSelector.SelectedDate;
      DateTime creationDate = DateTime.UtcNow;
      int selectedPriority = SelectedPriority;
      long statusId = statusList.Where(s => s.Name.ToLower() == "pendiente").First().StatusId;

      if (string.IsNullOrEmpty(taskTitle) || string.IsNullOrEmpty(taskDescription) || string.IsNullOrEmpty(taskTag) ||
        !endDate.HasValue || selectedPriority == 0)
      {
        MessageBox.Show("Por favor, complete todos los campos antes de crear la tarea.", "Campos incompletos",
          MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if ((endDate.Value < creationDate) || (endDate.Value == creationDate))
      {
        MessageBox.Show("La fecha de finalización no puede ser anterior o igual a la fecha de creación.", "Fecha inválida",
          MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (endDate != null)
      {
        Task taskToCreate = new Task(selectedPanelId, taskTitle, taskDescription, taskTag, selectedPriority, statusId, selectedPanelColor, endDate);
        taskToCreate.Add();

        TaskCreated?.Invoke();
        this.Close();
      }
    }
  }
}
