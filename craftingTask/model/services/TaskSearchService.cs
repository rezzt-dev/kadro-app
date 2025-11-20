using craftingTask.model.objects;
using craftingTask.persistence.managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace craftingTask.model.services
{
  public class TaskSearchService : ITaskSearchService
  {
    private readonly TaskManager taskManager;

    public TaskSearchService()
    {
      taskManager = new TaskManager();
    }

    public List<objects.Task> Search(objects.SearchCriteria criteria)
    {
      if (criteria == null || criteria.IsEmpty)
      {
        return new List<objects.Task>();
      }

      // Use database search for better performance
      return taskManager.SearchTasks(criteria);
    }

    public List<string> GetAutocompleteSuggestions(string keyword, int maxResults = 10)
    {
      if (string.IsNullOrWhiteSpace(keyword))
      {
        return new List<string>();
      }

      var suggestions = new HashSet<string>();

      // Get title suggestions
      var titleSuggestions = taskManager.GetTaskTitles(keyword);
      foreach (var title in titleSuggestions.Take(maxResults))
      {
        suggestions.Add(title);
      }

      // Get tag suggestions if we haven't reached max results
      if (suggestions.Count < maxResults)
      {
        var tagSuggestions = taskManager.GetDistinctTags()
          .Where(tag => tag.Contains(keyword, StringComparison.OrdinalIgnoreCase))
          .Take(maxResults - suggestions.Count);

        foreach (var tag in tagSuggestions)
        {
          suggestions.Add(tag);
        }
      }

      return suggestions.Take(maxResults).ToList();
    }
  }
}
