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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace craftingTask.resources.drawable
{
  /// <summary>
  /// Lógica de interacción para ModernColorPicker.xaml
  /// </summary>
  public partial class ModernColorPicker : UserControl, INotifyPropertyChanged
  {
    private Brush _selectedBrush = new SolidColorBrush(Colors.White);
    private string _colorHex = "#FFFFFF";

    public event PropertyChangedEventHandler PropertyChanged;

    public event Action<Color> ColorAccepted;
    public event Action Cancelled;

    public Brush SelectedBrush
    {
      get => _selectedBrush;
      set
      {
        _selectedBrush = value;
        OnPropertyChanged(nameof(SelectedBrush));
        if (value is SolidColorBrush scb)
        {
          _colorHex = $"#{scb.Color.R:X2}{scb.Color.G:X2}{scb.Color.B:X2}";
          OnPropertyChanged(nameof(ColorHex));
        }
      }
    }

    public string ColorHex
    {
      get => _colorHex;
      set
      {
        _colorHex = value;
        OnPropertyChanged(nameof(ColorHex));
        try
        {
          if (!string.IsNullOrWhiteSpace(value))
          {
            var conv = (Brush)new BrushConverter().ConvertFromString(value.Trim());
            if (conv != null) SelectedBrush = conv;
          }
        }
        catch { /* Ignorar si es inválido */ }
      }
    }

    public ModernColorPicker()
    {
      InitializeComponent();
      DataContext = this;

      ColorPalette.MouseDown += Palette_MouseDown;
      ColorPalette.MouseMove += Palette_MouseMove;
      ColorPalette.MouseUp += Palette_MouseUp;
    }

    private void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
      Cancelled?.Invoke();
    }

    private void BtnAccept_Click(object sender, RoutedEventArgs e)
    {
      if (SelectedBrush is SolidColorBrush scb)
        ColorAccepted?.Invoke(scb.Color);
    }

    private bool _isDragging = false;

    private void Palette_MouseDown(object sender, MouseButtonEventArgs e)
    {
      _isDragging = true;
      UpdateColorFromMouse(e.GetPosition(ColorPalette));
      ColorPalette.CaptureMouse();
    }

    private void Palette_MouseMove(object sender, MouseEventArgs e)
    {
      if (_isDragging && e.LeftButton == MouseButtonState.Pressed)
      {
        UpdateColorFromMouse(e.GetPosition(ColorPalette));
      }
    }

    private void Palette_MouseUp(object sender, MouseButtonEventArgs e)
    {
      _isDragging = false;
      ColorPalette.ReleaseMouseCapture();
    }

    private void UpdateColorFromMouse(Point position)
    {
      if (position.X < 0) position.X = 0;
      if (position.X > ColorPalette.ActualWidth) position.X = ColorPalette.ActualWidth;

      double ratio = position.X / ColorPalette.ActualWidth;
      Color color = GetColorFromHue(ratio);
      SelectedBrush = new SolidColorBrush(color);
    }

    private Color GetColorFromHue(double ratio)
    {
      if (ratio < 0) ratio = 0;
      if (ratio > 1) ratio = 1;

      Color[] colors = new Color[]
      {
                Colors.Red,
                Colors.Yellow,
                Colors.Lime,
                Colors.Cyan,
                Colors.Blue,
                Colors.Magenta,
                Colors.Red
      };

      double[] stops = new double[] { 0, 0.17, 0.33, 0.5, 0.66, 0.82, 1.0 };

      for (int i = 0; i < stops.Length - 1; i++)
      {
        if (ratio >= stops[i] && ratio <= stops[i + 1])
        {
          double localRatio = (ratio - stops[i]) / (stops[i + 1] - stops[i]);
          return InterpolateColor(colors[i], colors[i + 1], localRatio);
        }
      }

      return Colors.White;
    }

    private Color InterpolateColor(Color c1, Color c2, double t)
    {
      byte r = (byte)(c1.R + (c2.R - c1.R) * t);
      byte g = (byte)(c1.G + (c2.G - c1.G) * t);
      byte b = (byte)(c1.B + (c2.B - c1.B) * t);
      return Color.FromRgb(r, g, b);
    }
  }
}
