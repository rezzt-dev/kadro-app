using System;

namespace craftingTask.model.objects
{
  public class Attachment
  {
    public long Id { get; set; }
    public long TaskId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string? FilePath { get; set; }
    public byte[]? Data { get; set; }
    public DateTime CreatedAt { get; set; }

    // Constructor vacío para mapeo desde BD
    public Attachment()
    {
    }

    // Constructor para nuevo adjunto (BLOB)
    public Attachment(long taskId, string fileName, string contentType, byte[] data)
    {
      TaskId = taskId;
      FileName = fileName;
      ContentType = contentType;
      Data = data;
      CreatedAt = DateTime.UtcNow;
    }

    // Constructor para nuevo adjunto (FilePath)
    public Attachment(long taskId, string fileName, string contentType, string filePath)
    {
      TaskId = taskId;
      FileName = fileName;
      ContentType = contentType;
      FilePath = filePath;
      CreatedAt = DateTime.UtcNow;
    }

    // Constructor completo (para cargar desde BD)
    public Attachment(long id, long taskId, string fileName, string contentType,
      string filePath, byte[] data, DateTime createdAt)
    {
      Id = id;
      TaskId = taskId;
      FileName = fileName;
      ContentType = contentType;
      FilePath = filePath;
      Data = data;
      CreatedAt = createdAt;
    }
  }
}
