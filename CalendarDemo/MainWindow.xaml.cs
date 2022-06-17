using CalendarControls;
using System.Windows;

namespace CalendarDemo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    private void MonthCalendar_DateClicked(object sender, System.DateTime e)
    {
        // Communicated this event to the ViewModel - usually in MVVM with event binding
        MessageBox.Show($"Date {e.ToShortDateString()} was clicked!");
    }

    private void MonthCalendar_EventClicked(object sender, EventClickedEventArgs e)
    {
        // Communicated this event to the ViewModel - usually in MVVM with event binding
        MessageBox.Show(
            $"Event \"{e.Event.Title}\" ({e.Event.Start.ToShortDateString()} - {e.Event.End.ToShortDateString()}) was clicked!\n" +
            $"Clicked day was {e.ClickedDay.ToShortDateString()}");
    }
}
