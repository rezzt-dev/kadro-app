using craftingTask.model.objects;
using System;
using System.Collections.Generic;
using System.Windows;

namespace craftingTask.persistence.managers
{
  public class SavedFilterManager
  {
    private DBBroker? broker;

    public SavedFilterManager()
    {
    }

    public long GetFilterLastId()
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var filterCount = broker.ExecuteScalar("SELECT COUNT(*) FROM SavedFilter");
          return Convert.ToInt64(filterCount) + 1;
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al obtener el último FilterId: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return 1;
      }
    }

    public void AddFilter(SavedFilter inputFilter)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var parameters = new Dictionary<string, object>
            {
                {"@FilterId", inputFilter.FilterId },
                {"@Name", inputFilter.Name },
                {"@CriteriaJson", inputFilter.CriteriaJson },
                {"@CreationDate", inputFilter.CreationDate }
            };

          broker.ExecuteNonQuery(
              "INSERT INTO SavedFilter (FilterId, Name, CriteriaJson, CreationDate) " +
              "VALUES (@FilterId, @Name, @CriteriaJson, @CreationDate)",
              parameters
          );
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al agregar filtro guardado: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void UpdateFilter(SavedFilter inputFilter)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var parameters = new Dictionary<string, object>
            {
                {"@FilterId", inputFilter.FilterId },
                {"@Name", inputFilter.Name },
                {"@CriteriaJson", inputFilter.CriteriaJson }
            };

          broker.ExecuteNonQuery(
              "UPDATE SavedFilter SET Name = @Name, CriteriaJson = @CriteriaJson WHERE FilterId = @FilterId",
              parameters
          );
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al actualizar filtro guardado: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void RemoveFilter(SavedFilter inputFilter)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var parameters = new Dictionary<string, object>
            {
                {"@FilterId", inputFilter.FilterId }
            };

          broker.ExecuteNonQuery(
              "DELETE FROM SavedFilter WHERE FilterId = @FilterId",
              parameters
          );
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al eliminar filtro guardado: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public List<SavedFilter> GetAllFilters()
    {
      try
      {
        using (var broker = new DBBroker())
        {
          return broker.ExecuteQuery<SavedFilter>("SELECT * FROM SavedFilter ORDER BY CreationDate DESC");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al obtener filtros guardados: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return new List<SavedFilter>();
      }
    }

    public SavedFilter? GetFilterById(long filterId)
    {
      try
      {
        using (var broker = new DBBroker())
        {
          var parameters = new Dictionary<string, object>
            {
                {"@FilterId", filterId }
            };

          var filters = broker.ExecuteQuery<SavedFilter>(
              "SELECT * FROM SavedFilter WHERE FilterId = @FilterId",
              parameters
          );

          return filters.Count > 0 ? filters[0] : null;
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error al obtener filtro: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return null;
      }
    }
  }
}
