using System;
using System.Collections.Generic;

namespace craftingTask.model.objects
{
  public class SearchCriteria
  {
    public string? Keyword { get; set; }
    public List<int>? Priorities { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<long>? StatusIds { get; set; }
    public List<long>? PanelIds { get; set; }
    public List<long>? BoardIds { get; set; }
    public List<string>? Tags { get; set; }

    public SearchCriteria()
    {
      Priorities = new List<int>();
      StatusIds = new List<long>();
      PanelIds = new List<long>();
      BoardIds = new List<long>();
      Tags = new List<string>();
    }

    public bool HasKeyword => !string.IsNullOrWhiteSpace(Keyword);
    public bool HasPriorities => Priorities != null && Priorities.Count > 0;
    public bool HasDateRange => StartDate.HasValue || EndDate.HasValue;
    public bool HasStatusFilter => StatusIds != null && StatusIds.Count > 0;
    public bool HasPanelFilter => PanelIds != null && PanelIds.Count > 0;
    public bool HasBoardFilter => BoardIds != null && BoardIds.Count > 0;
    public bool HasTagFilter => Tags != null && Tags.Count > 0;

    public bool IsEmpty => !HasKeyword && !HasPriorities && !HasDateRange &&
                           !HasStatusFilter && !HasPanelFilter && !HasBoardFilter && !HasTagFilter;

    public void Clear()
    {
      Keyword = null;
      Priorities?.Clear();
      StartDate = null;
      EndDate = null;
      StatusIds?.Clear();
      PanelIds?.Clear();
      BoardIds?.Clear();
      Tags?.Clear();
    }
  }
}
