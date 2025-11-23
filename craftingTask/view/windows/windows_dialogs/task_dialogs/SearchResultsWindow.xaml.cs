using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using craftingTask.model.objects;

namespace craftingTask.view.windows
{
  public partial class SearchResultsWindow : Window
  {
    public SearchResultsWindow(List<model.objects.Task> tasks)
    {
      InitializeComponent();
      lstResults.ItemsSource = tasks;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void btnMinimizeWindowClick(object sender, RoutedEventArgs e)
    {
      this.WindowState = WindowState.Minimized;
    }
    private void btnCloseWindowClick(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void lstResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (lstResults.SelectedItem is model.objects.Task selectedTask)
      {
        MessageBox.Show($"Tarea seleccionada: {selectedTask.Title}\nID: {selectedTask.TaskId}\nPanel ID: {selectedTask.PanelId}",
                        "Información de Tarea", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }
  }
}
