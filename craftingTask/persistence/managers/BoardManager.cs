using craftingTask.model.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftingTask.persistence.managers
{
  public class BoardManager
  {
    private List<Board> boardList;
    private DBBroker broker;

    public BoardManager() 
    {
      boardList = new List<Board>();
    }

    public long GetBoardLastId()
    {
      broker = new DBBroker();
      var boardCount = broker.ExecuteScalar("SELECT COUNT(*) FROM 'Board'");
      long lastBoardId = Convert.ToInt64(boardCount) + 1;
      return lastBoardId;
    }

    public void AddBoard (Board inputBoard)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        { "@BoardId", inputBoard.BoardId },
        { "@Name", inputBoard.Name },
        { "@Color", inputBoard.Color },
        { "@CreationDate", inputBoard.CreationDate },
        { "@ModificationDate", inputBoard.ModificationDate },
      };
      broker.ExecuteNonQuery("INSERT INTO 'Board' (BoardId, Name, Color, CreationDate, ModificationDate) VALUES (@BoardId, @Name, @Color, @CreationDate, @ModificationDate)", 
        insertParams);

      Panel PanelPendientes = new Panel(inputBoard.BoardId, "Pendiente", "#FF6B6B");
      Panel PanelPogreso = new Panel(inputBoard.BoardId, "En Progreso", "#FFD93D");
      Panel PanelRealzadas = new Panel(inputBoard.BoardId, "Realizada", "#6BCB77");
      Panel PanelArchivadas = new Panel(inputBoard.BoardId, "Archivada", "#4D96FF");

      PanelPendientes.Add();
      PanelPogreso.Add();
      PanelRealzadas.Add();
      PanelArchivadas.Add();
    }

    public void UpdateBoard (Board inputBoard)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        { "@BoardId", inputBoard.BoardId },
        { "@Name", inputBoard.Name },
        { "@Color", inputBoard.Color },
        { "@ModificationDate", inputBoard.ModificationDate },
      };
      broker.ExecuteNonQuery("UPDATE 'Board' SET Name = @Name, Color = @Color, ModificationDate = @ModificationDate WHERE BoardId = @BoardId", insertParams);
    }

    public void DeleteBoard (Board inputBoard)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        {"@BoardId", inputBoard.BoardId }
      };
      broker.ExecuteNonQuery("DELETE FROM 'Board' WHERE BoardId = @BoardId", insertParams);
    }

    public List<Board> GetAllBoards ()
    {
      broker = new DBBroker();
      var returnedPanelList = broker.ExecuteQuery<Board>("SELECT * FROM 'Board'");
      return returnedPanelList;
    }
  }
}
