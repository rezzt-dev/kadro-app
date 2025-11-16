using craftingTask.model.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace craftingTask.persistence.managers
{
  public class PanelManager
  {
    private List<Panel> panelList;
    private DBBroker broker;

    public PanelManager()
    {
      panelList = new List<Panel>();
    }

    public long GetPanelLastId ()
    {
      broker = new DBBroker();
      var panelCount = broker.ExecuteScalar("SELECT COUNT(*) FROM Panel");
      long lastPanelId = Convert.ToInt64(panelCount) + 1;
      return lastPanelId;
    }

    public int GetPanelLastOrder ()
    {
      broker = new DBBroker();
      var panelOrder = broker.ExecuteScalar("SELECT MAX([Order]) FROM Panel");
      int lastOrder = panelOrder != DBNull.Value ? Convert.ToInt32(panelOrder) : 0;
      return lastOrder + 1;
    }

    public void AddPanel (Panel inputPanel)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        {"@PanelId", inputPanel.PanelId },
        {"@BoardId", inputPanel.BoardId},
        {"@Name", inputPanel.Name},
        {"@Order", inputPanel.Order },
        {"@Color", inputPanel.Color },
        {"@CreationDate", inputPanel.CreationDate }
      };
      broker.ExecuteNonQuery("INSERT INTO Panel (PanelId, BoardId, Name, [Order], Color, CreationDate) VALUES (@PanelId, @BoardId, @Name, @Order, @Color, @CreationDate)",
        insertParams);
    }

    public void UpdatePanel (Panel inputPanel)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        {"@PanelId", inputPanel.PanelId },
        {"@Name", inputPanel.Name},
        {"@Order", inputPanel.Order },
      };
      broker.ExecuteNonQuery("UPDATE Panel SET Name = @Name, [Order] = @Order WHERE PanelId = @PanelId", insertParams);
    }

    public void RemovePanel (Panel inputPanel)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        {"@PanelId", inputPanel.PanelId }
      };
      broker.ExecuteNonQuery("DELETE FROM Panel WHERE PanelId = @PanelId", insertParams);
    }

    public List<Panel> GetAllPanelsFromBoard (long inputBoardId)
    {
      broker = new DBBroker();
      var parameters = new Dictionary<string, object>
      {
        { "@BoardId", inputBoardId }
      };
      var returnedPanelList = broker.ExecuteQuery<Panel>("SELECT * FROM Panel WHERE BoardId = @BoardId", parameters);
      return returnedPanelList;
    }
  }
}
