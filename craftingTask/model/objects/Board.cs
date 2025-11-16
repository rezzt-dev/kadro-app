using craftingTask.persistence.managers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftingTask.model.objects
{
  public class Board
  {
    public long BoardId { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModificationDate {  get; set; }

    private List<Board> boardList { get; set; }
    private long LastBoardId { get; set; }
    private BoardManager boardManager { get; set; }

    public Board ()
    {
      boardManager = new BoardManager();
      boardList = new List<Board> ();

      LastBoardId = boardManager.GetBoardLastId();
      this.BoardId = LastBoardId;
    }

    public Board (string inputName, string inputColor)
    {
      boardManager = new BoardManager();
      boardList = new List<Board>();

      LastBoardId = boardManager.GetBoardLastId();
      this.BoardId = LastBoardId;
      this.Name = inputName;
      this.Color = inputColor;
      this.CreationDate = DateTime.UtcNow;
    }

    public Board (string inputName, string inputColor, DateTime inputModificationDate)
    {
      boardManager = new BoardManager();
      boardList = new List<Board>();

      LastBoardId = boardManager.GetBoardLastId();
      this.BoardId = LastBoardId;
      this.Name = inputName;
      this.Color = inputColor;
      this.ModificationDate = inputModificationDate;
      this.CreationDate = DateTime.UtcNow;
    }

    public Board (long inputBoardId, string inputName, string inputColor, DateTime inputCreationDate, DateTime inputModificationDate)
    {
      boardManager = new BoardManager();
      boardList = new List<Board>();

      this.BoardId = inputBoardId;
      this.Name = inputName;
      this.Color = inputColor;
      this.ModificationDate = inputModificationDate;
      this.CreationDate = inputCreationDate;
    }
  }
}
