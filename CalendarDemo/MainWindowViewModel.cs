using CalendarControls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CalendarDemo;

public class MainWindowViewModel
    : BindableBase
{
    private DateTime _calendarDate;
    public DateTime CalendarDate
    {
        get
        {
            return _calendarDate;
        }
        set
        {
            _calendarDate = value;
            LoadEvents(_calendarDate);
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<DebitEvent> _debitEvents = new();
    public ObservableCollection<DebitEvent> DebitEvents
    {
        get { return _debitEvents; }
        set { _debitEvents = value; RaisePropertyChanged(); }
    }

    public MainWindowViewModel()
    {
        CalendarDate = DateTime.Today;
    }

    // Pseudo-Database
    private static DebitEvent[] _data = new[]
    {
        new DebitEvent { Title = "BERI", Start = new DateTime(2022, 5, 8), End = new DateTime(2022, 5, 11) },
        new DebitEvent { Title = "OREN", Start = new DateTime(2022, 5, 31), End = new DateTime(2022, 6, 2) },
        new DebitEvent { Title = "AKUN", Start = new DateTime(2022, 6, 13), End = new DateTime(2022, 6, 16) },
        new DebitEvent { Title = "BEDO", Start = new DateTime(2022, 6, 14), End = new DateTime(2022, 6, 18) },
        new DebitEvent { Title = "DINO", Start = new DateTime(2022, 6, 18), End = new DateTime(2022, 6, 19) },
        new DebitEvent { Title = "DALA", Start = new DateTime(2022, 6, 13), End = new DateTime(2022, 6, 19) },
        new DebitEvent { Title = "YEPO", Start = new DateTime(2022, 6, 17), End = new DateTime(2022, 6, 18) },
        new DebitEvent { Title = "UNER", Start = new DateTime(2022, 7, 20), End = new DateTime(2022, 7, 26) },
        // Holidays
        new DebitEvent { Title = "Nationalfeiertag", Start = new DateTime(2022, 10, 26), End = new DateTime(2022, 10, 26) },
     };

    /// <summary>
    /// Cancel any previous loading operations, and start a new one.
    /// </summary>
    private void LoadEvents(DateTime day)
    {
        DebitEvents = new ObservableCollection<DebitEvent>(
            _data.Where(d => (d.Start.Year == day.Year && d.Start.Month == day.Month)
            || (d.End.Year == day.Year && d.End.Month == day.Month)));
    }
}
