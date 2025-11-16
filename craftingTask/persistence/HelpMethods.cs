using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace craftingTask.persistence
{
  public static class HelpMethods
  {
    public static Frame? FindParentFrame(DependencyObject child)
    {
      DependencyObject parent = VisualTreeHelper.GetParent(child);

      while (parent != null)
      {
        if (parent is Frame frame)
          return frame;

        parent = VisualTreeHelper.GetParent(parent);
      }

      return null;
    }
  }
}
