using craftingTask.persistence.managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace craftingTask.model.objects
{
  public class Task
  {
    public long TaskId { get; set; }
    public long PanelId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Tag { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime EndDate {  get; set; }
    public int Priority { get; set; }
    public long StatusId { get; set; }
    public string Color { get; set; }

    private List<Task> taskList {  get; set; }
    private long LastTaskId { get; set; }
    private TaskManager taskManager { get; set; }

    public Task ()
    {
      taskManager = new TaskManager ();
      taskList = new List<Task> ();

      this.LastTaskId = taskManager.GetTaskLastId();
      this.TaskId = LastTaskId;
    }

    public Task (long inputPanelId, string inputTitle, string inputDescription, string inputTag, int inputPriority, long inputStatusId, string inputColor)
    {
      taskManager = new TaskManager();
      taskList = new List<Task>();

      this.LastTaskId = taskManager.GetTaskLastId();
      this.TaskId = LastTaskId;
      this.Title = inputTitle;
      this.Description = inputDescription;
      this.Tag = inputTag;
      this.Priority = inputPriority;
      this.CreationDate = DateTime.UtcNow;
      this.Priority = inputPriority;
      this.StatusId = inputStatusId;
      this.Color = inputColor;
    }

    public Task (long inputPanelId, string inputTitle, string inputDescription, string inputTag, 
      DateTime inputCreationDate, int inputPriority, long inputStatusId, string inputColor)
    {
      taskManager = new TaskManager();
      taskList = new List<Task>();

      this.LastTaskId = taskManager.GetTaskLastId();
      this.TaskId = LastTaskId;
      this.Title = inputTitle;
      this.Description = inputDescription;
      this.Tag = inputTag;
      this.Priority = inputPriority;
      this.CreationDate = inputCreationDate;
      this.Priority = inputPriority;
      this.StatusId = inputStatusId;
      this.Color = inputColor;
    }

    public Task(long inputPanelId, string inputTitle, string inputDescription, string inputTag,
      DateTime inputCreationDate, DateTime inputEndDate, int inputPriority, long inputStatusId, string inputColor)
    {
      taskManager = new TaskManager();
      taskList = new List<Task>();

      this.LastTaskId = taskManager.GetTaskLastId();
      this.TaskId = LastTaskId;
      this.Title = inputTitle;
      this.Description = inputDescription;
      this.Tag = inputTag;
      this.Priority = inputPriority;
      this.CreationDate = inputCreationDate;
      this.EndDate = inputEndDate;
      this.Priority = inputPriority;
      this.StatusId = inputStatusId;
      this.Color = inputColor;
    }

    public Task(long inputTaskId, long inputPanelId, string inputTitle, string inputDescription, string inputTag,
      DateTime inputCreationDate, DateTime inputEndDate, int inputPriority, long inputStatusId, string inputColor)
    {
      taskManager = new TaskManager();
      taskList = new List<Task>();

      this.TaskId = inputTaskId;
      this.Title = inputTitle;
      this.Description = inputDescription;
      this.Tag = inputTag;
      this.Priority = inputPriority;
      this.CreationDate = inputCreationDate;
      this.EndDate = inputEndDate;
      this.Priority = inputPriority;
      this.StatusId = inputStatusId;
      this.Color = inputColor;
    }

    public void Add ()
    {
      taskManager = new TaskManager();
      taskManager.AddTask(this);
    }
    public void Update ()
    {
      taskManager = new TaskManager();
      taskManager.UpdateTask(this);
    }
    public void Remove ()
    {
      taskManager = new TaskManager();
      taskManager.RemoveTask(this);
    }

    public void UpdateStateToDo()
    {
      taskManager = new TaskManager();
      taskManager.UpdateTaskState(this, 1, "#FF6B6B");
    }
    public void UpdateStateDoing()
    {
      taskManager = new TaskManager();
      taskManager.UpdateTaskState(this, 2, "#FFD93D");
    }
    public void UpdateStateDone()
    {
      taskManager = new TaskManager();
      taskManager.UpdateTaskState(this, 3, "#6BCB77");
    }
    public void UpdateStateArchived()
    {
      taskManager = new TaskManager();
      taskManager.UpdateTaskState(this, 4, "#4D96FF");
    }
  }
}
