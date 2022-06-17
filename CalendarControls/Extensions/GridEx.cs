using System.Windows;
using System.Windows.Controls;

namespace CalendarControls;

/// <summary>
/// Adds grid extensions using attached properties.
/// </summary>
public static class GridEx
{
    /// <summary>
    /// Set the desired number of rows for the grid.
    /// </summary>
    public static readonly DependencyProperty RowCountProperty = DependencyProperty.RegisterAttached(
            "RowCount",
            typeof(int),
            typeof(GridEx),
            new PropertyMetadata(-1, RowCountChanged));

    // Required for getting the attached property
    public static int GetRowCount(DependencyObject obj)
    {
        return (int)obj.GetValue(RowCountProperty);
    }

    // Required for setting the attached property
    public static void SetRowCount(DependencyObject obj, int value)
    {
        obj.SetValue(RowCountProperty, value);
    }

    public static void RowCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (!(obj is Grid) || (int)e.NewValue < 0)
            return;

        Grid grid = (Grid)obj;
        grid.RowDefinitions.Clear();

        for (int i = 0; i < (int)e.NewValue; i++)
            grid.RowDefinitions.Add(
                new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Star)
                });
    }
}
