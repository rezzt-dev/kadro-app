using craftingTask.model.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
      try
      {
        using (var broker = new DBBroker())
        {
          var boardCount = broker.ExecuteScalar("SELECT COUNT(*) FROM Board");
          return Convert.ToInt64(boardCount) + 1;
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al obtener el último BoardId: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return 1; // valor por defecto si falla
      }
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
      broker.ExecuteNonQuery("INSERT INTO Board (BoardId, Name, Color, CreationDate, ModificationDate) VALUES (@BoardId, @Name, @Color, @CreationDate, @ModificationDate)", 
        insertParams);

      new Panel(inputBoard.BoardId, "Pendientes", "#FF6B6B").Add();
      new Panel(inputBoard.BoardId, "En Progreso", "#FFD93D").Add();
      new Panel(inputBoard.BoardId, "Realizadas", "#6BCB77").Add();
      new Panel(inputBoard.BoardId, "Archivadas", "#4D96FF").Add();
    }

    public void UpdateBoard(Board inputBoard)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var parameters = new Dictionary<string, object>
            {
                { "@BoardId", inputBoard.BoardId },
                { "@Name", inputBoard.Name },
                { "@Color", inputBoard.Color },
                { "@ModificationDate", inputBoard.ModificationDate },
            };

          broker.ExecuteNonQuery(
              "UPDATE Board SET Name = @Name, Color = @Color, ModificationDate = @ModificationDate WHERE BoardId = @BoardId",
              parameters
          );
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al actualizar board: " + ex.Message);
      }
    }

    public void DeleteBoard(Board inputBoard)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var parameters = new Dictionary<string, object>
            {
                { "@BoardId", inputBoard.BoardId }
            };

          broker.ExecuteNonQuery("DELETE FROM Board WHERE BoardId = @BoardId", parameters);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al eliminar board: " + ex.Message);
      }
    }

    public List<Board> GetAllBoards()
    {
      try
      {
        using (var broker = new DBBroker())
        {
          return broker.ExecuteQuery<Board>("SELECT * FROM Board");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al obtener boards: " + ex.Message);
        return new List<Board>();
      }
    }
  }
}
