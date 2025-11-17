using craftingTask.model.objects;
using craftingTask.persistence.managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
  /// Lógica de interacción para ShowTaskArchive.xaml
  /// </summary>
  public partial class ShowTaskArchive : Window
  {
    public ObservableCollection<Task> ArchiveTaskList { get; set; }
    public List<Task> SelectedTasks => taskListBox.SelectedItems.Cast<Task>().ToList();
    private Panel donePanel;
    public event Action TaskUnarchive;

    public ShowTaskArchive(Panel inputTargetPanel, Panel inputDonePanel)
    {
      TaskManager taskManager = new TaskManager();
      this.donePanel = inputDonePanel;
      this.ArchiveTaskList = new ObservableCollection<model.objects.Task>(taskManager.GetAllTasksFromPanel(inputTargetPanel.PanelId));
      InitializeComponent();
      DataContext = this;
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

    private void btnDeleteTasks_Click(object sender, RoutedEventArgs e)
    {
      if (SelectedTasks.Count == 0)
      {
        MessageBox.Show("Debes seleccionar al menos una tarea para eliminar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }
      
      var result = MessageBox.Show($"¿Seguro que quieres eliminar {SelectedTasks.Count} tareas?", "Confirmar borrado", MessageBoxButton.YesNo, MessageBoxImage.Question);
      if (result == MessageBoxResult.Yes)
      {
        foreach (var task in SelectedTasks)
        {
          task.Remove();
          ArchiveTaskList.Remove(task);
        }
        this.Close();
      }
    }

    private void btnUnarchiveTasks_Click(object sender, RoutedEventArgs e)
    {
      if (SelectedTasks.Count == 0)
      {
        MessageBox.Show("Debes seleccionar al menos una tarea para desarchivar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var result = MessageBox.Show($"¿Seguro que quieres desarchivar {SelectedTasks.Count} tareas?", "Confirmar desarchivado", MessageBoxButton.YesNo, MessageBoxImage.Question);
      if (result == MessageBoxResult.Yes)
      {
        foreach (var task in SelectedTasks)
        {
          task.MoveToPanel(this.donePanel);
          ArchiveTaskList.Remove(task);
        }

        TaskUnarchive.Invoke();
        this.Close();
      }
    }
  }
}
