using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace craftingTask.persistence.managers
{
  public class KadroBackupManager
  {
    private const string DatabaseFileName = "CTDatabase.db";
    private const string AttachmentsFolderName = "Attachments";

    public async Task CreateBackupAsync(string destinationPath)
    {
      await Task.Run(() =>
      {
        string tempDir = Path.Combine(Path.GetTempPath(), "KadroBackup_" + Guid.NewGuid());
        Directory.CreateDirectory(tempDir);

        try
        {
          // 1. Copy Database
          string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFileName);
          if (File.Exists(dbPath))
          {
            File.Copy(dbPath, Path.Combine(tempDir, DatabaseFileName));
          }

          // 2. Copy Attachments
          string attachmentsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AttachmentsFolderName);
          if (Directory.Exists(attachmentsPath))
          {
            string destAttachments = Path.Combine(tempDir, AttachmentsFolderName);
            CopyDirectory(attachmentsPath, destAttachments);
          }

          // 3. Zip it
          if (File.Exists(destinationPath)) File.Delete(destinationPath);
          ZipFile.CreateFromDirectory(tempDir, destinationPath);
        }
        finally
        {
          // Cleanup temp
          if (Directory.Exists(tempDir))
            Directory.Delete(tempDir, true);
        }
      });
    }

    public async Task RestoreBackupAsync(string sourcePath)
    {
      await Task.Run(() =>
      {
        string tempDir = Path.Combine(Path.GetTempPath(), "KadroRestore_" + Guid.NewGuid());
        Directory.CreateDirectory(tempDir);

        try
        {
          // 1. Extract Zip
          ZipFile.ExtractToDirectory(sourcePath, tempDir);

          // 2. Restore Database
          string sourceDb = Path.Combine(tempDir, DatabaseFileName);
          string targetDb = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DatabaseFileName);

          if (File.Exists(sourceDb))
          {
            File.Copy(sourceDb, targetDb, true);
          }

          // 3. Restore Attachments
          string sourceAttachments = Path.Combine(tempDir, AttachmentsFolderName);
          string targetAttachments = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AttachmentsFolderName);

          if (Directory.Exists(sourceAttachments))
          {
            if (Directory.Exists(targetAttachments))
              Directory.Delete(targetAttachments, true);

            CopyDirectory(sourceAttachments, targetAttachments);
          }
        }
        finally
        {
          if (Directory.Exists(tempDir))
            Directory.Delete(tempDir, true);
        }
      });
    }

    private void CopyDirectory(string sourceDir, string destDir)
    {
      Directory.CreateDirectory(destDir);

      foreach (var file in Directory.GetFiles(sourceDir))
      {
        string destFile = Path.Combine(destDir, Path.GetFileName(file));
        File.Copy(file, destFile, true);
      }

      foreach (var subDir in Directory.GetDirectories(sourceDir))
      {
        string destSubDir = Path.Combine(destDir, Path.GetFileName(subDir));
        CopyDirectory(subDir, destSubDir);
      }
    }
  }
}
