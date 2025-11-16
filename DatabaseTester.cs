using craftingTask.persistence;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static craftingTask.DatabaseTester;

namespace craftingTask
{
  public class DatabaseTester
  {
    public class Usuario
    {
      public int Id { get; set; }
      public string Nombre { get; set; }
      public int Edad { get; set; }
    }

    static void Main()
    {
      var db = new DBBroker("CTDatabase.db");

       //---------------------------------------------------------------------------------------
       // SELECT con parámetros (protegido contra SQL injection)
      var parameters = new Dictionary<string, object>
      {
        {"@nombre", "Juan"},
        {"@edad", 25}
      };

      var results = db.ExecuteQuery<Usuario>(
        "SELECT * FROM usuarios WHERE nombre = @nombre AND edad > @edad",
        parameters
        );

      foreach (Usuario usuario in results)
      {
        Console.WriteLine($"ID: {usuario.Id}, Nombre: {usuario.Nombre}");
      }

       //---------------------------------------------------------------------------------------
       // INSERT
      var insertParams = new Dictionary<string, object>
      {
        {"@nombre", "María"},
        {"@email", "maria@example.com"},
        {"@edad", 30},
      };

      int affected = db.ExecuteNonQuery(
        "INSERT INTO usuarios (nombre, email, edad) VALUES (@nombre, @email, @edad)",
        insertParams
        );

      Console.WriteLine($"Filas insertadas: {affected}");

       //---------------------------------------------------------------------------------------
       // UPDATE
      var updateParams = new Dictionary<string, object>
      {
        {"@email", "nuevo@example.com"},
        { "@id", 1 }
      };

      db.ExecuteNonQuery(
        "UPDATE usuarios SET email = @email WHERE id = @id",
        updateParams
        );

       //---------------------------------------------------------------------------------------
       // COUNT
      var count = db.ExecuteScalar("SELECT COUNT(*) FROM usuarios");
      Console.WriteLine($"Total usuarios: {count}");
    }
  }
}
