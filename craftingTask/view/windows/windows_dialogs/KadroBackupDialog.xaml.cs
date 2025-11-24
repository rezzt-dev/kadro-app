using craftingTask.persistence.managers;
using Microsoft.Win32;
using System;
using System.Windows;

namespace craftingTask.view.windows.windows_dialogs
{
  public partial class KadroBackupDialog : Window
  {
    private KadroBackupManager backupManager;

    public KadroBackupDialog()
    {
      InitializeComponent();
      backupManager = new KadroBackupManager();
    }

    private async void btnCreateBackup_Click(object sender, RoutedEventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog
      {
        Filter = "Kadro Backup (*.kdb)|*.kdb",
        FileName = $"KadroBackup_{DateTime.Now:yyyyMMdd_HHmmss}.kdb"
      };

      if (saveFileDialog.ShowDialog() == true)
      {
        try
        {
          this.Cursor = System.Windows.Input.Cursors.Wait;
          await backupManager.CreateBackupAsync(saveFileDialog.FileName);
          MessageBox.Show("Backup creado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
          this.Close();
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Error al crear backup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
          this.Cursor = System.Windows.Input.Cursors.Arrow;
        }
      }
    }

    private async void btnRestoreBackup_Click(object sender, RoutedEventArgs e)
    {
      var result = MessageBox.Show(
          "Restaurar un backup sobrescribirá todos los datos actuales. La aplicación se cerrará al finalizar. ¿Deseas continuar?",
          "Advertencia",
          MessageBoxButton.YesNo,
          MessageBoxImage.Warning);

      if (result == MessageBoxResult.Yes)
      {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
          Filter = "Kadro Backup (*.kdb)|*.kdb"
        };

        if (openFileDialog.ShowDialog() == true)
        {
          try
          {
            this.Cursor = System.Windows.Input.Cursors.Wait;
            await backupManager.RestoreBackupAsync(openFileDialog.FileName);
            MessageBox.Show("Backup restaurado correctamente. La aplicación se cerrará para aplicar los cambios.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            Application.Current.Shutdown();
          }
          catch (Exception ex)
          {
            MessageBox.Show($"Error al restaurar backup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          }
          finally
          {
            this.Cursor = System.Windows.Input.Cursors.Arrow;
          }
        }
      }
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
