using craftingTask.model.objects;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace craftingTask.view.windows.windows_dialogs.task_dialogs
{
  public partial class AttachmentPreviewWindow : Window
  {
    private Attachment _attachment;

    public AttachmentPreviewWindow(Attachment attachment)
    {
      InitializeComponent();
      _attachment = attachment;

      txtFileName.Text = attachment.FileName;
      LoadContent();
    }

    private void LoadContent()
    {
      try
      {
        txtLoading.Visibility = Visibility.Visible;

        if (_attachment.ContentType.StartsWith("image/"))
        {
          LoadImage();
        }
        else if (_attachment.ContentType == "application/pdf")
        {
          LoadPdf();
        }
        else
        {
          txtLoading.Text = "Tipo de archivo no soportado para vista previa";
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error cargando vista previa: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        Close();
      }
      finally
      {
        txtLoading.Visibility = Visibility.Collapsed;
      }
    }

    private void LoadImage()
    {
      if (_attachment.Data == null || _attachment.Data.Length == 0)
      {
        MessageBox.Show("No hay datos de imagen disponibles", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      using (var ms = new MemoryStream(_attachment.Data))
      {
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = ms;
        bitmapImage.EndInit();
        bitmapImage.Freeze(); // Para poder usar en otro thread si es necesario

        ImgViewer.Source = bitmapImage;
        ImgViewer.Visibility = Visibility.Visible;
      }
    }

    private void LoadPdf()
    {
      if (_attachment.Data == null || _attachment.Data.Length == 0)
      {
        MessageBox.Show("No hay datos de PDF disponibles", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      // Crear archivo temporal para el PDF
      string tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");
      File.WriteAllBytes(tempPath, _attachment.Data);

      // Navegar al PDF
      PdfViewer.Navigate(new Uri(tempPath));
      PdfViewer.Visibility = Visibility.Visible;

      // Nota: El archivo temporal se limpiará cuando se cierre la aplicación
      // o se puede implementar un mecanismo de limpieza en el evento Closed
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton == MouseButton.Left)
      {
        DragMove();
      }
    }

    private void btnMinimize_Click(object sender, RoutedEventArgs e)
    {
      WindowState = WindowState.Minimized;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
  }
}
