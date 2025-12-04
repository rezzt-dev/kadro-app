using craftingTask.model.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace craftingTask.persistence.managers
{
  public class TaskManager
  {
    private List<model.objects.Task> taskList;
    private DBBroker broker;

    public TaskManager()
    {
      taskList = new List<model.objects.Task>();
    }

    public long GetTaskLastId()
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var taskCount = broker.ExecuteScalar("SELECT COUNT(*) FROM Task");
          return Convert.ToInt64(taskCount) + 1;
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al obtener el último TaskId: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return 1;
      }
    }

    public void AddTask(model.objects.Task inputTask)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var parameters = new Dictionary<string, object>
            {
                {"@PanelId", inputTask.PanelId },
                {"@Title", inputTask.Title },
                {"@Description", inputTask.Description },
                {"@Tag", inputTask.Tag },
                {"@CreationDate", inputTask.CreationDate },
                {"@EndDate", inputTask.EndDate },
                {"@Priority", inputTask.Priority },
                {"@StatusId", inputTask.StatusId },
                {"@Color", inputTask.Color }
            };

          broker.ExecuteNonQuery(
              "INSERT INTO Task (PanelId, Title, Description, Tag, CreationDate, EndDate, Priority, StatusId, Color) " +
              "VALUES (@PanelId, @Title, @Description, @Tag, @CreationDate, @EndDate, @Priority, @StatusId, @Color)",
              parameters
          );
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al agregar tarea: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void UpdateTask(model.objects.Task inputTask)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var parameters = new Dictionary<string, object>
            {
                {"@TaskId", inputTask.TaskId },
                {"@PanelId", inputTask.PanelId },
                {"@Title", inputTask.Title },
                {"@Description", inputTask.Description },
                {"@Tag", inputTask.Tag },
                {"@CreationDate", inputTask.CreationDate },
                {"@EndDate", inputTask.EndDate },
                {"@Priority", inputTask.Priority },
                {"@StatusId", inputTask.StatusId },
                {"@Color", inputTask.Color }
            };

          broker.ExecuteNonQuery(
              "UPDATE Task SET PanelId = @PanelId, Title = @Title, Description = @Description, Tag = @Tag, " +
              "CreationDate = @CreationDate, EndDate = @EndDate, Priority = @Priority, StatusId = @StatusId, Color = @Color " +
              "WHERE TaskId = @TaskId",
              parameters
          );
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al actualizar tarea: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void UpdateTaskState(model.objects.Task inputTask, long inputStatusId, long inputPanelId, string inputColor)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var parameters = new Dictionary<string, object>
            {
                {"@TaskId", inputTask.TaskId },
                {"@PanelId", inputPanelId },
                {"@StatusId", inputStatusId },
                {"@Color", inputColor }
            };

          broker.ExecuteNonQuery(
              "UPDATE Task SET StatusId = @StatusId, Color = @Color, PanelId = @PanelId WHERE TaskId = @TaskId",
              parameters
          );
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al actualizar estado de tarea: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void RemoveTask(model.objects.Task inputTask)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var parameters = new Dictionary<string, object>
            {
                {"@TaskId", inputTask.TaskId }
            };

          broker.ExecuteNonQuery(
              "DELETE FROM Task WHERE TaskId = @TaskId",
              parameters
          );
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al eliminar tarea: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public List<model.objects.Task> GetAllTasksFromPanel(long inputPanelId)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var parameters = new Dictionary<string, object>
            {
                {"@PanelId", inputPanelId }
            };

          return broker.ExecuteQuery<model.objects.Task>(
              "SELECT * FROM Task WHERE PanelId = @PanelId",
              parameters
          );
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al obtener tareas: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return new List<model.objects.Task>();
      }
    }

    public List<model.objects.Task> GetAllTasks()
    {
      try
      {
        using (var broker = new DBBroker())
        {
          return broker.ExecuteQuery<model.objects.Task>("SELECT * FROM Task");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al obtener tareas: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return new List<model.objects.Task>();
      }
    }

    public List<model.objects.Task> GetUpcomingTasks()
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var now = DateTime.UtcNow;
          var limit = DateTime.UtcNow.AddDays(7);

          var parameters = new Dictionary<string, object>
            {
                {"@Now", now},
                {"@Limit", limit}
            };
          string query = "SELECT * FROM Task WHERE EndDate >= @Now AND EndDate <= @Limit AND StatusId NOT IN (3, 4)";
          return broker.ExecuteQuery<model.objects.Task>(query, parameters);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al obtener tareas próximas: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return new List<model.objects.Task>();
      }
    }

    public List<model.objects.Task> SearchTasks(model.objects.SearchCriteria criteria)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var queryBuilder = new StringBuilder("SELECT DISTINCT t.* FROM Task t");
          var parameters = new Dictionary<string, object>();
          var whereConditions = new List<string>();

          // Join with Panel if filtering by BoardId
          if (criteria.HasBoardFilter)
          {
            queryBuilder.Append(" INNER JOIN Panel p ON t.PanelId = p.PanelId");
          }

          // Keyword search (Title, Description, Tag)
          if (criteria.HasKeyword)
          {
            whereConditions.Add("(t.Title LIKE @Keyword OR t.Description LIKE @Keyword OR t.Tag LIKE @Keyword)");
            parameters.Add("@Keyword", $"%{criteria.Keyword}%");
          }

          // Priority filter
          if (criteria.HasPriorities)
          {
            var priorityPlaceholders = new List<string>();
            for (int i = 0; i < criteria.Priorities.Count; i++)
            {
              var paramName = $"@Priority{i}";
              priorityPlaceholders.Add(paramName);
              parameters.Add(paramName, criteria.Priorities[i]);
            }
            whereConditions.Add($"t.Priority IN ({string.Join(", ", priorityPlaceholders)})");
          }

          // Date range filter
          if (criteria.StartDate.HasValue)
          {
            whereConditions.Add("t.EndDate >= @StartDate");
            parameters.Add("@StartDate", criteria.StartDate.Value);
          }
          if (criteria.EndDate.HasValue)
          {
            whereConditions.Add("t.EndDate <= @EndDate");
            parameters.Add("@EndDate", criteria.EndDate.Value);
          }

          // Status filter
          if (criteria.HasStatusFilter)
          {
            var statusPlaceholders = new List<string>();
            for (int i = 0; i < criteria.StatusIds.Count; i++)
            {
              var paramName = $"@Status{i}";
              statusPlaceholders.Add(paramName);
              parameters.Add(paramName, criteria.StatusIds[i]);
            }
            whereConditions.Add($"t.StatusId IN ({string.Join(", ", statusPlaceholders)})");
          }

          // Panel filter
          if (criteria.HasPanelFilter)
          {
            var panelPlaceholders = new List<string>();
            for (int i = 0; i < criteria.PanelIds.Count; i++)
            {
              var paramName = $"@Panel{i}";
              panelPlaceholders.Add(paramName);
              parameters.Add(paramName, criteria.PanelIds[i]);
            }
            whereConditions.Add($"t.PanelId IN ({string.Join(", ", panelPlaceholders)})");
          }

          // Board filter
          if (criteria.HasBoardFilter)
          {
            var boardPlaceholders = new List<string>();
            for (int i = 0; i < criteria.BoardIds.Count; i++)
            {
              var paramName = $"@Board{i}";
              boardPlaceholders.Add(paramName);
              parameters.Add(paramName, criteria.BoardIds[i]);
            }
            whereConditions.Add($"p.BoardId IN ({string.Join(", ", boardPlaceholders)})");
          }

          // Tag filter
          if (criteria.HasTagFilter)
          {
            var tagConditions = new List<string>();
            for (int i = 0; i < criteria.Tags.Count; i++)
            {
              var paramName = $"@Tag{i}";
              tagConditions.Add($"t.Tag LIKE {paramName}");
              parameters.Add(paramName, $"%{criteria.Tags[i]}%");
            }
            whereConditions.Add($"({string.Join(" OR ", tagConditions)})");
          }

          // Build final query
          if (whereConditions.Count > 0)
          {
            queryBuilder.Append(" WHERE ");
            queryBuilder.Append(string.Join(" AND ", whereConditions));
          }

          queryBuilder.Append(" ORDER BY t.EndDate ASC, t.Priority DESC");

          return broker.ExecuteQuery<model.objects.Task>(queryBuilder.ToString(), parameters);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al buscar tareas: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return new List<model.objects.Task>();
      }
    }

    public List<string> GetDistinctTags()
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var tasks = broker.ExecuteQuery<model.objects.Task>("SELECT DISTINCT Tag FROM Task WHERE Tag IS NOT NULL AND Tag != ''");
          return tasks.Where(t => !string.IsNullOrWhiteSpace(t.Tag))
                     .Select(t => t.Tag)
                     .Distinct()
                     .OrderBy(tag => tag)
                     .ToList();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al obtener tags: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return new List<string>();
      }
    }

    public List<string> GetTaskTitles(string keyword)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var parameters = new Dictionary<string, object>
            {
                {"@Keyword", $"%{keyword}%"}
            };

          var tasks = broker.ExecuteQuery<model.objects.Task>(
            "SELECT Title FROM Task WHERE Title LIKE @Keyword ORDER BY Title LIMIT 10",
            parameters
          );

          return tasks.Select(t => t.Title).ToList();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al obtener títulos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return new List<string>();
      }
    }
  }
}
