using craftingTask.model.objects;
using craftingTask.persistence;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static craftingTask.persistence.HelpMethods;
using Panel = craftingTask.model.objects.Panel;

namespace craftingTask.view.frame_pages
{
  /// <summary>
  /// Lógica de interacción para BoardPage.xaml
  /// </summary>
  public partial class BoardPage : Page
  {
    public Board selectedBoard;
    private List<Panel> BoardPanels { get; set; } = new List<Panel>();
    public ObservableCollection<Panel> PanelList { get; set; }

    public BoardPage(Board inputSelectedBoard)
    {
      InitializeComponent();
      this.selectedBoard = inputSelectedBoard;

      this.PanelList = new ObservableCollection<Panel>();
      LoadPanels(selectedBoard.BoardId);
      DataContext = this;
    }

    private void btnCloseBoardFrame (object sender, RoutedEventArgs e)
    {
      Frame? parentFrame = HelpMethods.FindParentFrame(this);
      if (parentFrame != null)
      {
        parentFrame.Content = null;
      }
    }

    private void LoadPanels (long inputBoardId)
    {
      PanelList.Clear();

      PanelManager panelManager = new PanelManager();
      List<Panel> loadedPanels = panelManager.GetAllPanelsFromBoard(inputBoardId);

      foreach (Panel auxPanel in loadedPanels)
      {
        if (auxPanel.Name.ToLower() != "archivadas")
        {
          Panel sndAuxPanel = new Panel(auxPanel.PanelId, auxPanel.BoardId, auxPanel.Name, auxPanel.Order, auxPanel.Color, auxPanel.CreationDate);
          PanelList.Add(sndAuxPanel);
        }
      }
    }
  }
}
