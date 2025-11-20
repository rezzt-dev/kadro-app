using craftingTask.persistence.managers;
using System;
using System.Text.Json;

namespace craftingTask.model.objects
{
  public class SavedFilter
  {
    public long FilterId { get; set; }
    public string Name { get; set; }
    public string CriteriaJson { get; set; }
    public DateTime CreationDate { get; set; }

    private SavedFilterManager? filterManager;

    public SavedFilter()
    {
      filterManager = new SavedFilterManager();
      Name = string.Empty;
      CriteriaJson = string.Empty;
      CreationDate = DateTime.UtcNow;
    }

    public SavedFilter(string inputName, SearchCriteria criteria)
    {
      filterManager = new SavedFilterManager();

      this.FilterId = filterManager.GetFilterLastId();
      this.Name = inputName;
      this.CriteriaJson = SerializeCriteria(criteria);
      this.CreationDate = DateTime.UtcNow;
    }

    public SavedFilter(long inputFilterId, string inputName, string inputCriteriaJson, DateTime inputCreationDate)
    {
      filterManager = new SavedFilterManager();

      this.FilterId = inputFilterId;
      this.Name = inputName;
      this.CriteriaJson = inputCriteriaJson;
      this.CreationDate = inputCreationDate;
    }

    public void Add()
    {
      filterManager = new SavedFilterManager();
      filterManager.AddFilter(this);
    }

    public void Update()
    {
      filterManager = new SavedFilterManager();
      filterManager.UpdateFilter(this);
    }

    public void Delete()
    {
      filterManager = new SavedFilterManager();
      filterManager.RemoveFilter(this);
    }

    public SearchCriteria GetCriteria()
    {
      return DeserializeCriteria(this.CriteriaJson);
    }

    public void SetCriteria(SearchCriteria criteria)
    {
      this.CriteriaJson = SerializeCriteria(criteria);
    }

    private static string SerializeCriteria(SearchCriteria criteria)
    {
      return JsonSerializer.Serialize(criteria);
    }

    private static SearchCriteria DeserializeCriteria(string json)
    {
      try
      {
        return JsonSerializer.Deserialize<SearchCriteria>(json) ?? new SearchCriteria();
      }
      catch
      {
        return new SearchCriteria();
      }
    }
  }
}
