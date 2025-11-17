using craftingTask.model.objects;
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

namespace craftingTask.view.windows.windows_dialogs
{
  /// <summary>
  /// Lógica de interacción para DeleteBoardsDialog.xaml
  /// </summary>
  public partial class DeleteBoardsDialog : Window
  {
    public ObservableCollection<Board> BoardList { get; set; }
    public List<Board> SelectedBoards => boardsListBox.SelectedItems.Cast<Board>().ToList();

    public DeleteBoardsDialog(ObservableCollection<Board> boardList)
    {
      InitializeComponent();
      BoardList = boardList;
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
    private void btnCancelDelete_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
    private void btnDeleteBoards_Click(object sender, RoutedEventArgs e)
    {
      if (SelectedBoards.Count == 0)
      {
        MessageBox.Show("Debes seleccionar al menos un tablero para eliminar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var result = MessageBox.Show($"¿Seguro que quieres eliminar {SelectedBoards.Count} tableros?", "Confirmar borrado", MessageBoxButton.YesNo, MessageBoxImage.Question);
      if (result == MessageBoxResult.Yes)
      {
        foreach (var board in SelectedBoards)
        {
          board.Delete();
          BoardList.Remove(board);
        }
        this.Close();
      }
    }
  }
}
