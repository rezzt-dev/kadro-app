using craftingTask.model;
using craftingTask.model.objects;
using craftingTask.persistence;
using craftingTask.persistence.managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Task = craftingTask.model.objects.Task;

namespace craftingTask.view.frame_pages
{
  /// <summary>
  /// Lógica de interacción para RemindersPage.xaml
  /// </summary>
  public partial class RemindersPage : Page, INotifyPropertyChanged
  {
    private Task selectedTask;
    public Task SelectedTask
    {
      get => selectedTask;
      set
      {
        selectedTask = value;
        OnPropertyChanged(nameof(SelectedTask));
      }
    }

    public ICommand ShowTaskInfoCommand { get; }

    public List<Task> UpcomingTasks { get; set; }

    public RemindersPage()
    {
      InitializeComponent();
      DataContext = this;
      ShowTaskInfoCommand = new RelayCommand(param =>
      {
        if (param is Task task)
        {
          SelectedTask = task;
          OnPropertyChanged(nameof(SelectedTask));
        }
      });
      ChargeUpcomingTasks();
    }

    private void btnCloseReminderFrame(object sender, RoutedEventArgs e)
    {
      Frame? parentFrame = HelpMethods.FindParentFrame(this);
      if (parentFrame != null)
      {
        parentFrame.Content = null;
      }
    }

    private void ChargeUpcomingTasks ()
    {
      TaskManager taskManager = new TaskManager();
      UpcomingTasks = taskManager.GetUpcomingTasks();
      OnPropertyChanged(nameof(UpcomingTasks));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
  }
}
