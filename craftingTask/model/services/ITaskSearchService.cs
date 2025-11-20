using craftingTask.model.objects;
using System.Collections.Generic;

namespace craftingTask.model.services
{
  public interface ITaskSearchService
  {
    List<objects.Task> Search(objects.SearchCriteria criteria);
    List<string> GetAutocompleteSuggestions(string keyword, int maxResults = 10);
  }
}
