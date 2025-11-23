using craftingTask.persistence.managers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace craftingTask.model.services
{
  public class AttachmentService
  {
    private readonly AttachmentManager _attachmentManager;

    // Extensiones que se guardan como BLOB
    private static readonly string[] BlobExtensions = { ".png", ".jpg", ".jpeg", ".ico", ".webp", ".pdf" };

    // Extensiones que se guardan como FilePath
    private static readonly string[] FilePathExtensions = { ".odt", ".docx" };

    // Todas las extensiones permitidas
    private static readonly string[] AllowedExtensions = BlobExtensions.Concat(FilePathExtensions).ToArray();

    public AttachmentService()
    {
      _attachmentManager = new AttachmentManager();
    }

    /// <summary>
    /// Valida si el tipo de archivo está permitido
    /// </summary>
    public bool IsFileTypeAllowed(string filePath)
    {
      string extension = Path.GetExtension(filePath).ToLowerInvariant();
      return AllowedExtensions.Contains(extension);
    }

    /// <summary>
    /// Determina si el archivo debe guardarse como BLOB
    /// </summary>
    public bool ShouldSaveAsBlob(string filePath)
    {
      string extension = Path.GetExtension(filePath).ToLowerInvariant();
      return BlobExtensions.Contains(extension);
    }

    /// <summary>
    /// Agrega un adjunto, determinando automáticamente el método de almacenamiento
    /// </summary>
    public async Task<bool> AddAttachmentAsync(long taskId, string filePath)
    {
      try
      {
        // Validar que el archivo exista
        if (!File.Exists(filePath))
        {
          throw new FileNotFoundException("El archivo no existe", filePath);
        }

        // Validar tipo de archivo
        if (!IsFileTypeAllowed(filePath))
        {
          throw new InvalidOperationException(
            "Tipo de archivo no permitido. Solo se permiten: PNG, JPG, JPEG, ICO, WEBP, PDF, ODT, DOCX");
        }

        // Verificar límite de 2 archivos
        if (!await _attachmentManager.CanAddAttachmentAsync(taskId))
        {
          return false; // Límite alcanzado
        }

        // Guardar según el tipo de archivo
        if (ShouldSaveAsBlob(filePath))
        {
          await _attachmentManager.AddAttachmentAsync(taskId, filePath);
        }
        else
        {
          await _attachmentManager.AddAttachmentPathAsync(taskId, filePath);
        }

        return true;
      }
      catch (Exception ex)
      {
        throw new Exception($"Error en el servicio de adjuntos: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// Verifica si se puede agregar un adjunto
    /// </summary>
    public async Task<bool> CanAddAttachmentAsync(long taskId)
    {
      return await _attachmentManager.CanAddAttachmentAsync(taskId);
    }

    /// <summary>
    /// Obtiene los adjuntos de una tarea
    /// </summary>
    public async Task<System.Collections.Generic.List<objects.Attachment>> GetAttachmentsAsync(long taskId)
    {
      return await _attachmentManager.GetAttachmentsForTaskAsync(taskId);
    }

    /// <summary>
    /// Elimina un adjunto
    /// </summary>
    public async Task DeleteAttachmentAsync(long attachmentId)
    {
      await _attachmentManager.DeleteAttachmentAsync(attachmentId);
    }

    /// <summary>
    /// Obtiene el filtro para el diálogo de apertura de archivos
    /// </summary>
    public string GetFileDialogFilter()
    {
      return "Imágenes y documentos|*.png;*.jpg;*.jpeg;*.ico;*.webp;*.pdf;*.odt;*.docx";
    }
  }
}
