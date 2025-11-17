using craftingTask.model.objects;
using craftingTask.persistence.managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Task = craftingTask.model.objects.Task;

namespace craftingTask.view.windows.windows_dialogs.task_dialogs
{
  /// <summary>
  /// Lógica de interacción para ArchiveTaskDialog.xaml
  /// </summary>
  public partial class ArchiveTaskDialog : Window
  {
    public ObservableCollection<Task> DoneTaskList { get; set; }
    public List<Task> SelectedTasks => taskListBox.SelectedItems.Cast<Task>().ToList();
    private Panel selectedPanel;
    private Panel archivePanel;
    public event Action TasksArchive;

    public ArchiveTaskDialog(Panel inputDonePanel, Panel inputArchivePanel)
    {
      this.selectedPanel = inputDonePanel;
      this.archivePanel = inputArchivePanel;
      TaskManager taskManager = new TaskManager();
      InitializeComponent();
      this.DoneTaskList = new ObservableCollection<model.objects.Task>(taskManager.GetAllTasksFromPanel(inputDonePanel.PanelId));
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
    private void btnCancelArchive_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void btnArchiveTasks_Click(object sender, RoutedEventArgs e)
    {
      var tasksToArchive = SelectedTasks;

      if (tasksToArchive.Count == 0)
      {
        MessageBox.Show("No hay tareas seleccionadas para archivar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var result = MessageBox.Show($"¿Seguro que quieres archivar {tasksToArchive.Count} tareas?", "Confirmar archivado", MessageBoxButton.YesNo, MessageBoxImage.Question);
      if (result == MessageBoxResult.Yes)
      {
        foreach (var task in tasksToArchive)
        {
          task.MoveToPanel(this.archivePanel);
          DoneTaskList.Remove(task);
        }

        TasksArchive?.Invoke();
        this.Close();
      }
    }
  }
}
