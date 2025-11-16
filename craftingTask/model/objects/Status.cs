using craftingTask.persistence.managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftingTask.model.objects
{
  public class Status
  {
    public long StatusId { get; set; }
    public string Name { get; set; }

    private List<Status> statusList {  get; set; }
    private long LastStatusId { get; set; }
    private StatusManager statusManager { get; set; }

    public Status ()
    {
      statusManager = new StatusManager ();
      statusList = new List<Status> ();

      this.LastStatusId = statusManager.GetStatusLastId();
      this.StatusId = this.LastStatusId;
    }

    public Status (string inputName)
    {
      statusManager = new StatusManager();
      statusList = new List<Status>();

      this.LastStatusId = statusManager.GetStatusLastId();
      this.StatusId = this.LastStatusId;
      this.Name = inputName;
    }

    public Status (long inputStatusId, string inputName)
    {
      statusManager = new StatusManager();
      statusList = new List<Status> ();

      this.StatusId = inputStatusId;
      this.Name = inputName;
    }

    public void Add ()
    {
      statusManager = new StatusManager();
      statusManager.AddStatus (this);
    }
    public void Update ()
    {
      statusManager = new StatusManager();
      statusManager.UpdateStatus (this);
    }
    public void Delete ()
    {
      statusManager = new StatusManager ();
      statusManager.RemoveStatus (this);
    }
  }
}
