using craftingTask.model;
using craftingTask.model.objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace craftingTask.view.windows.windows_dialogs
{
  /// <summary>
  /// Lógica de interacción para CreateBoardDialog.xaml
  /// </summary>
  public partial class CreateBoardDialog : Window, INotifyPropertyChanged
  {
    private Brush _selectedColorBrush;
    private string _selectedColorHex;
    public event PropertyChangedEventHandler PropertyChanged;

    public Board CreatedBoard { get; private set; }

    public Brush SelectedColorBrush
    {
      get => _selectedColorBrush;
      set
      {
        _selectedColorBrush = value;
        OnPropertyChanged(nameof(SelectedColorBrush));
      }
    }
    public string SelectedColorHex
    {
      get => _selectedColorHex;
      set
      {
        _selectedColorHex = value;
        OnPropertyChanged(nameof(SelectedColorHex));
      }
    }
    private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    public CreateBoardDialog()
    {
      InitializeComponent();
      DataContext = this;

      SelectedColorBrush = new SolidColorBrush(Color.FromRgb(58, 58, 58));
      SelectedColorHex = "#3A3A3A";

      ModernPicker.ColorAccepted += (color) =>
      {
        SelectedColorBrush = new SolidColorBrush(color);
        SelectedColorHex = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        ColorPopup.IsOpen = false;
      };

      ModernPicker.Cancelled += () =>
      {
        ColorPopup.IsOpen = false;
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
    private void btnCancelCreationClick(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
    private void btnCreateBoardClick(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrWhiteSpace(txtBoardNameSelector.Text) || string.IsNullOrWhiteSpace(txtColorSelector.Text))
      {
        MessageBox.Show("El nombre y el color del tablero no pueden estar vacíos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      string selectedBoardColor = SelectedColorHex;
      string selectedBoardName = txtBoardNameSelector.Text;

      Board board = new Board(selectedBoardName, selectedBoardColor);
      board.Add();

      this.CreatedBoard = board;
      this.DialogResult = true;
      this.Close();
    }

    private void BtnOpenColor_Click(object sender, RoutedEventArgs e)
    {
      if (ModernPicker != null)
      {
        ModernPicker.ColorHex = SelectedColorHex;
      }
      ColorPopup.IsOpen = true;
    }
    private void ApplyColorFromPicker()
    {
      if (ModernPicker == null) return;
      SelectedColorHex = ModernPicker.ColorHex;
      SelectedColorBrush = ModernPicker.SelectedBrush;
      DataContext = null;
      DataContext = this;
    }
    private void ColorPopup_Closed(object sender, EventArgs e)
    {
      ApplyColorFromPicker();
    }
  }
}
