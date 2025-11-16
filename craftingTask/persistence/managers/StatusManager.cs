using craftingTask.model.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftingTask.persistence.managers
{
  public class StatusManager
  {
    private List<Status> statusList;
    private DBBroker broker;

    public StatusManager()
    {
      statusList = new List<Status>();
    }

    public long GetStatusLastId ()
    {
      broker = new DBBroker();
      var statusCount = broker.ExecuteScalar("SELECT COUNT(*) FROM Status");
      long lastStatusId = Convert.ToInt64(statusCount) + 1;
      return lastStatusId;
    }

    public void AddStatus (Status inputStatus)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        {"@StatusId", inputStatus.StatusId},
        {"@Name", inputStatus.Name}
      };
      broker.ExecuteNonQuery("INSERT INTO Status (StatusId, Name) VALUES (@StatusId, @Name)",
        insertParams);
    }

    public void UpdateStatus (Status inputStatus)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        {"@StatusId", inputStatus.StatusId},
        {"@Name", inputStatus.Name}
      };
      broker.ExecuteNonQuery("UPDATE Status SET Name = @Name WHERE StatusId = @StatusId", insertParams);
    }

    public void RemoveStatus(Status inputStatus)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        {"@StatusId", inputStatus.StatusId}
      };
      broker.ExecuteNonQuery("DELETE FROM Status WHERE StatusId = @StatusId", insertParams);
    }

    public List<Status> GetAllStatus ()
    {
      broker = new DBBroker();
      var returnedStatusList = broker.ExecuteQuery<Status>("SELECT (*) FROM Status");
      return returnedStatusList;
    }
  }
}
