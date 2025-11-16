using craftingTask.model.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
      try
      {
        using (var broker = new DBBroker())
        {
          var statusCount = broker.ExecuteScalar("SELECT COUNT(*) FROM Status");
          long lastStatusId = Convert.ToInt64(statusCount) + 1;
          return lastStatusId;
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al obtener el último StatusId: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return 1; // valor por defecto si falla
      }
    }

    public void AddStatus (Status inputStatus)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var insertParams = new Dictionary<string, object>
            {
                {"@StatusId", inputStatus.StatusId},
                {"@Name", inputStatus.Name}
            };

          broker.ExecuteNonQuery(
              "INSERT INTO Status (StatusId, Name) VALUES (@StatusId, @Name)",
              insertParams
          );
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al agregar status: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void UpdateStatus (Status inputStatus)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var insertParams = new Dictionary<string, object>
            {
                {"@StatusId", inputStatus.StatusId},
                {"@Name", inputStatus.Name}
            };

          broker.ExecuteNonQuery(
              "UPDATE Status SET Name = @Name WHERE StatusId = @StatusId",
              insertParams
          );
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al actualizar status: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void RemoveStatus(Status inputStatus)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var insertParams = new Dictionary<string, object>
            {
                {"@StatusId", inputStatus.StatusId}
            };

          broker.ExecuteNonQuery(
              "DELETE FROM Status WHERE StatusId = @StatusId",
              insertParams
          );
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al eliminar status: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public List<Status> GetAllStatus ()
    {
      try
      {
        using (var broker = new DBBroker())
        {
          return broker.ExecuteQuery<Status>("SELECT * FROM Status");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al obtener todos los status: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return new List<Status>();
      }
    }
  }
}
