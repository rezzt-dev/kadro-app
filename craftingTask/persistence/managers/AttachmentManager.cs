using craftingTask.model.objects;
using craftingTask.persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace craftingTask.persistence.managers
{
  public class AttachmentManager
  {
    private DBBroker dbBroker;

    public AttachmentManager()
    {
      dbBroker = new DBBroker();
    }

    /// <summary>
    /// Verifica si se puede agregar un adjunto a la tarea (límite de 2 archivos)
    /// </summary>
    public async Task<bool> CanAddAttachmentAsync(long taskId)
    {
      try
      {
        string query = "SELECT COUNT(*) FROM Attachment WHERE TaskId = @taskId;";
        var parameters = new Dictionary<string, object>
        {
          { "@taskId", taskId }
        };

        var count = dbBroker.ExecuteScalar(query, parameters);
        long attachmentCount = Convert.ToInt64(count);

        return attachmentCount < 2;
      }
      catch (Exception ex)
      {
        throw new Exception($"Error verificando límite de adjuntos: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Agrega un adjunto guardando el archivo como BLOB en la base de datos
    /// </summary>
    public async System.Threading.Tasks.Task AddAttachmentAsync(long taskId, string filePath)
    {
      try
      {
        if (!File.Exists(filePath))
          throw new FileNotFoundException("El archivo no existe", filePath);

        byte[] fileData = await File.ReadAllBytesAsync(filePath);
        string fileName = Path.GetFileName(filePath);
        string contentType = MimeMapping.MimeUtility.GetMimeMapping(filePath);

        string query = @"
          INSERT INTO Attachment (TaskId, FileName, ContentType, Data, CreatedAt)
          VALUES (@taskId, @fileName, @contentType, @data, @createdAt);";

        var parameters = new Dictionary<string, object>
        {
          { "@taskId", taskId },
          { "@fileName", fileName },
          { "@contentType", contentType },
          { "@data", fileData },
          { "@createdAt", DateTime.UtcNow }
        };

        dbBroker.ExecuteNonQuery(query, parameters);
      }
      catch (Exception ex)
      {
        throw new Exception($"Error agregando adjunto: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Agrega un adjunto guardando solo la ruta del archivo (para ODT/DOCX)
    /// </summary>
    public async System.Threading.Tasks.Task AddAttachmentPathAsync(long taskId, string filePath)
    {
      try
      {
        if (!File.Exists(filePath))
          throw new FileNotFoundException("El archivo no existe", filePath);

        string contentType = MimeMapping.MimeUtility.GetMimeMapping(filePath);
        string fileName = Path.GetFileName(filePath);

        // Crear carpeta de adjuntos si no existe
        string attachmentsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Attachments");
        Directory.CreateDirectory(attachmentsFolder);

        // Copiar archivo con nombre único
        string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(filePath)}";
        string destPath = Path.Combine(attachmentsFolder, uniqueFileName);
        File.Copy(filePath, destPath, overwrite: true);

        string query = @"
          INSERT INTO Attachment (TaskId, FileName, ContentType, FilePath, CreatedAt)
          VALUES (@taskId, @fileName, @contentType, @filePath, @createdAt);";

        var parameters = new Dictionary<string, object>
        {
          { "@taskId", taskId },
          { "@fileName", fileName },
          { "@contentType", contentType },
          { "@filePath", destPath },
          { "@createdAt", DateTime.UtcNow }
        };

        dbBroker.ExecuteNonQuery(query, parameters);
      }
      catch (Exception ex)
      {
        throw new Exception($"Error agregando adjunto con ruta: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Obtiene todos los adjuntos de una tarea
    /// </summary>
    public async Task<List<Attachment>> GetAttachmentsForTaskAsync(long taskId)
    {
      try
      {
        string query = "SELECT * FROM Attachment WHERE TaskId = @taskId ORDER BY CreatedAt DESC;";
        var parameters = new Dictionary<string, object>
        {
          { "@taskId", taskId }
        };

        var attachments = dbBroker.ExecuteQuery<Attachment>(query, parameters);
        return attachments;
      }
      catch (Exception ex)
      {
        throw new Exception($"Error obteniendo adjuntos: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Elimina un adjunto y su archivo físico si existe
    /// </summary>
    public async System.Threading.Tasks.Task DeleteAttachmentAsync(long attachmentId)
    {
      try
      {
        // Primero obtener la info del adjunto para eliminar el archivo físico si existe
        string selectQuery = "SELECT * FROM Attachment WHERE Id = @id;";
        var selectParams = new Dictionary<string, object>
        {
          { "@id", attachmentId }
        };

        var attachments = dbBroker.ExecuteQuery<Attachment>(selectQuery, selectParams);

        if (attachments.Count > 0)
        {
          var attachment = attachments[0];

          // Eliminar archivo físico si existe
          if (!string.IsNullOrEmpty(attachment.FilePath) && File.Exists(attachment.FilePath))
          {
            File.Delete(attachment.FilePath);
          }
        }

        // Eliminar de la base de datos
        string deleteQuery = "DELETE FROM Attachment WHERE Id = @id;";
        var deleteParams = new Dictionary<string, object>
        {
          { "@id", attachmentId }
        };

        dbBroker.ExecuteNonQuery(deleteQuery, deleteParams);
      }
      catch (Exception ex)
      {
        throw new Exception($"Error eliminando adjunto: {ex.Message}", ex);
      }
    }
  }
}
