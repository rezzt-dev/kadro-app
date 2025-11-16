using craftingTask.model.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace craftingTask.persistence.managers
{
  public class TaskManager
  {
    private List<model.objects.Task> taskList;
    private DBBroker broker;

    public TaskManager ()
    {
      taskList = new List<model.objects.Task> ();
    }

    public long GetTaskLastId()
    {
      broker = new DBBroker();
      var taskCount = broker.ExecuteScalar("SELECT COUNT(*) FROM Task");
      long lastTaskId = Convert.ToInt64(taskCount) + 1;
      return lastTaskId;
    }

    public void AddTask (model.objects.Task inputTask)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        {"@TaskId", inputTask.TaskId },
        {"@PanelId", inputTask.TaskId },
        {"@Title", inputTask.TaskId },
        {"@Description", inputTask.TaskId },
        {"@Tag", inputTask.TaskId },
        {"@CreationDate", inputTask.TaskId },
        {"@EndDate", inputTask.TaskId },
        {"@Priority", inputTask.TaskId },
        {"@StatusId", inputTask.TaskId },
        {"@Color", inputTask.TaskId }
      };

      broker.ExecuteNonQuery("INSERT INTO Task (TaskId, PanelId, Title, Description, Tag, CreationDate, EndDate, Priority, StatusId, Color) VALUES (@TaskId, @PanelId, @Title, @Description, @Tag, @CreationDate, @EndDate, @Priority, @StatusId, @Color)", insertParams);
    }

    public void UpdateTask(model.objects.Task inputTask)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        {"@TaskId", inputTask.TaskId },
        {"@PanelId", inputTask.TaskId },
        {"@Title", inputTask.TaskId },
        {"@Description", inputTask.TaskId },
        {"@Tag", inputTask.TaskId },
        {"@CreationDate", inputTask.TaskId },
        {"@EndDate", inputTask.TaskId },
        {"@Priority", inputTask.TaskId },
        {"@StatusId", inputTask.TaskId },
        {"@Color", inputTask.TaskId }
      };
      broker.ExecuteNonQuery("UPDATE Task SET PanelId = @PanelId, Title = @Title, Description = @Description, Tag = @Tag, CreationDate = @CreationDate, EndDate = @EndDate, Priority = @Priority, StatusId = @StatusId, Color = @Color WHERE TaskId = @TaskId", insertParams);
    }

    public void UpdateTaskState(model.objects.Task inputTask, long inputStatusId, string inputColor)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        {"@TaskId", inputTask.TaskId },
        {"@StatusId", inputStatusId },
        {"@Color", inputColor }
      };
      broker.ExecuteNonQuery("UPDATE Task SET StatusId = @StatusId WHERE TaskId = @TaskId", insertParams);
    }

    public void RemoveTask(model.objects.Task inputTask)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        {"@TaskId", inputTask.TaskId }
      };
      broker.ExecuteNonQuery("DELETE FROM Task WHERE TaskId = @TaskId", insertParams);
    }

    public List<model.objects.Task> GetAllTasksFromPanel(long inputPanelId)
    {
      broker = new DBBroker();
      var insertParams = new Dictionary<string, object>
      {
        {"@PanelId", inputPanelId }
      };
      var returnedTaskList = broker.ExecuteQuery<model.objects.Task>("SELECT * FROM 'Task' WHERE PanelId = @PanelId", insertParams);
      return returnedTaskList;
    }
  }
}
