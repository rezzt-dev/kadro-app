using craftingTask.model.objects;
using craftingTask.model.services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using craftingTask.persistence.managers;
using System.Collections.ObjectModel;
using Task = craftingTask.model.objects.Task;

namespace craftingTask.view.windows.windows_dialogs.task_dialogs
{
  /// <summary>
  /// Lógica de interacción para TaskDetailsDialog.xaml
  /// </summary>
  public partial class TaskDetailsDialog : Window
  {
    private Task selectedTask;
    private SubtaskManager subtaskManager = new SubtaskManager();
    private AttachmentService attachmentService = new AttachmentService();

    public TaskDetailsDialog(Task inputTask)
    {
      InitializeComponent();
      DataContext = this;
      this.selectedTask = inputTask;
      LoadTaskDetails();
    }

    private void LoadTaskDetails()
    {
      if (selectedTask == null)
        return;

      // Cargar título, descripción y tag
      txtTaskTitle.Text = selectedTask.Title;
      txtTaskDescription.Text = selectedTask.Description;
      txtTaskTag.Text = selectedTask.Tag;

      // Formatear y mostrar fechas
      txtCreationDate.Text = selectedTask.CreationDate.ToString("dd/MM/yyyy HH:mm");
      txtEndDate.Text = selectedTask.EndDate.ToString("dd/MM/yyyy");

      // Convertir prioridad a texto
      txtPriority.Text = ConvertPriorityToString(selectedTask.Priority);

      // Mostrar color del panel
      try
      {
        colorIndicator.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(selectedTask.Color));
      }
      catch
      {
        colorIndicator.Background = new SolidColorBrush(Colors.Gray);
      }

      LoadSubtasks();
      LoadAttachments();
    }

    private void LoadSubtasks()
    {
      var subtasks = subtaskManager.GetSubtasksForTask(selectedTask.TaskId);
      selectedTask.Subtasks = new ObservableCollection<Subtask>(subtasks);
      lstSubtasks.ItemsSource = selectedTask.Subtasks;
      UpdateProgress();
    }

    private void UpdateProgress()
    {
      if (selectedTask.Subtasks == null || selectedTask.Subtasks.Count == 0)
      {
        txtProgress.Text = "0/0";
        progressBar.Value = 0;
        return;
      }

      int total = selectedTask.Subtasks.Count;
      int completed = selectedTask.Subtasks.Count(s => s.IsCompleted);
      txtProgress.Text = $"{completed}/{total}";

      progressBar.Value = (double)completed / total * 100;
    }

    private void BtnAddSubtask_Click(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrWhiteSpace(txtNewSubtask.Text)) return;

      var newSubtask = new Subtask(selectedTask.TaskId, txtNewSubtask.Text, selectedTask.Subtasks.Count);

      // Guardar en BD
      subtaskManager.AddSubtask(newSubtask);

      // Actualizar UI (Recargar es lo más seguro para obtener el ID generado)
      LoadSubtasks();
      txtNewSubtask.Text = "";
    }

    private void Subtask_Checked(object sender, RoutedEventArgs e)
    {
      var checkBox = sender as CheckBox;
      if (checkBox?.Tag is long subtaskId)
      {
        subtaskManager.UpdateSubtaskStatus(subtaskId, true);
        UpdateProgress();
      }
    }

    private void Subtask_Unchecked(object sender, RoutedEventArgs e)
    {
      var checkBox = sender as CheckBox;
      if (checkBox?.Tag is long subtaskId)
      {
        subtaskManager.UpdateSubtaskStatus(subtaskId, false);
        UpdateProgress();
      }
    }

    private void BtnDeleteSubtask_Click(object sender, RoutedEventArgs e)
    {
      var button = sender as Button;
      if (button?.Tag is long subtaskId)
      {
        if (MessageBox.Show("¿Eliminar subtarea?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
          subtaskManager.DeleteSubtask(subtaskId);
          LoadSubtasks();
        }
      }
    }

    private string ConvertPriorityToString(int priority)
    {
      return priority switch
      {
        1 => "Baja",
        2 => "Media",
        3 => "Alta",
        4 => "Inmediata",
        _ => "Desconocida"
      };
    }

    private void btnMinimizeWindowClick(object sender, RoutedEventArgs e)
    {
      this.WindowState = WindowState.Minimized;
    }

    private void btnCloseWindowClick(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    // ========== MÉTODOS DE ADJUNTOS ==========

    private async void LoadAttachments()
    {
      try
      {
        var attachments = await attachmentService.GetAttachmentsAsync(selectedTask.TaskId);
        lstAttachments.ItemsSource = attachments;

        // Mostrar mensaje si no hay adjuntos
        txtNoAttachments.Visibility = attachments.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error cargando adjuntos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private async void BtnAttachFile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
          Filter = attachmentService.GetFileDialogFilter(),
          Multiselect = false,
          Title = "Seleccionar archivo para adjuntar"
        };

        if (dialog.ShowDialog() == true)
        {
          // Verificar límite de 2 archivos
          if (!await attachmentService.CanAddAttachmentAsync(selectedTask.TaskId))
          {
            MessageBox.Show("Solo se pueden adjuntar 2 archivos por tarea.", "Límite alcanzado",
              MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
          }

          // Validar tipo de archivo
          if (!attachmentService.IsFileTypeAllowed(dialog.FileName))
          {
            MessageBox.Show("Tipo de archivo no permitido.\nSolo se permiten: PNG, JPG, JPEG, ICO, WEBP, PDF, ODT, DOCX",
              "Archivo no válido", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
          }

          // Agregar adjunto
          bool success = await attachmentService.AddAttachmentAsync(selectedTask.TaskId, dialog.FileName);

          if (success)
          {
            LoadAttachments(); // Recargar lista
            MessageBox.Show("Archivo adjuntado correctamente.", "Éxito",
              MessageBoxButton.OK, MessageBoxImage.Information);
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error adjuntando archivo: {ex.Message}", "Error",
          MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void AttachmentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (lstAttachments.SelectedItem is Attachment attachment)
      {
        try
        {
          // Imágenes y PDFs: mostrar en ventana de vista previa
          if (attachment.ContentType.StartsWith("image/") ||
              attachment.ContentType == "application/pdf" ||
              attachment.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
          {
            var previewWindow = new AttachmentPreviewWindow(attachment);
            previewWindow.ShowDialog();
          }
          // ODT, DOCX: abrir con aplicación del sistema
          else if (!string.IsNullOrEmpty(attachment.FilePath) && File.Exists(attachment.FilePath))
          {
            OpenExternalFile(attachment);
          }
          else
          {
            MessageBox.Show("El archivo no está disponible para vista previa.", "Información",
              MessageBoxButton.OK, MessageBoxImage.Information);
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Error abriendo archivo: {ex.Message}", "Error",
            MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
          // Deseleccionar para permitir volver a hacer clic en el mismo item
          lstAttachments.SelectedIndex = -1;
        }
      }
    }

    private void OpenExternalFile(Attachment attachment)
    {
      if (string.IsNullOrEmpty(attachment.FilePath))
        return;

      var processStartInfo = new ProcessStartInfo
      {
        FileName = attachment.FilePath,
        UseShellExecute = true // Abre con la aplicación asociada del SO
      };

      Process.Start(processStartInfo);
    }

    private async void BtnDeleteAttachment_Click(object sender, RoutedEventArgs e)
    {
      var button = sender as Button;
      if (button?.Tag is long attachmentId)
      {
        var result = MessageBox.Show("¿Está seguro de que desea eliminar este adjunto?",
          "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
          try
          {
            await attachmentService.DeleteAttachmentAsync(attachmentId);
            LoadAttachments(); // Recargar lista
            MessageBox.Show("Adjunto eliminado correctamente.", "Éxito",
              MessageBoxButton.OK, MessageBoxImage.Information);
          }
          catch (Exception ex)
          {
            MessageBox.Show($"Error eliminando adjunto: {ex.Message}", "Error",
              MessageBoxButton.OK, MessageBoxImage.Error);
          }
        }
      }
    }
  }
}
