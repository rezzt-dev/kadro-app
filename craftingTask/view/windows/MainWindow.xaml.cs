using craftingTask.model.objects;
using craftingTask.persistence.managers;
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
using SQLitePCL;
using craftingTask.view.frame_pages;

namespace craftingTask
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public List<Board> BoardList { get; set; }
    private BoardManager mainBoardManager;

    public MainWindow()
    {
      InitializeComponent();

      mainBoardManager = new BoardManager();
      BoardList = mainBoardManager.GetAllBoards();

      DataContext = this;
    }

    private void btnMinimizeWindow (object sender, RoutedEventArgs e)
    {
      this.WindowState = WindowState.Minimized;
    }

    private void btnMaximizeWindow(object sender, RoutedEventArgs e)
    {
      if (this.WindowState == WindowState.Maximized)
      {
        this.WindowState = WindowState.Normal;
      } else
      {
        this.WindowState = WindowState.Maximized;
      }
    }

    private void btnCloseWindow(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var selectedBoard = ((ListViewItem)sender).Content as Board;
      if (selectedBoard != null)
      {
        Board mainSelectedBoard = selectedBoard;
        BoardPage boardPage = new BoardPage(mainSelectedBoard);
        this.boardsFrame.Navigate(boardPage);
      }
    }
  }
}