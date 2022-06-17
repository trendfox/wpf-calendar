using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace CalendarControls;

internal class CalendarDayViewModel
    : BindableBase
{
    private int _calendarColumn;
    public int CalendarColumn
    {
        get { return _calendarColumn; }
        set { _calendarColumn = value; RaisePropertyChanged(); }
    }

    private int _calendarRow;
    public int CalendarRow
    {
        get { return _calendarRow; }
        set { _calendarRow = value; RaisePropertyChanged(); }
    }

    private bool _isInMonth;
    public bool IsInMonth
    {
        get { return _isInMonth; }
        set { _isInMonth = value; RaisePropertyChanged(); }
    }

    public bool IsToday
    {
        get { return _date.Date == DateTime.Today; }
    }

    private static List<DayOfWeek> _weekends = new()
    {
        DayOfWeek.Saturday,
        DayOfWeek.Sunday
    };

    public bool IsWeekend
    {
        get { return _weekends.Contains(_date.DayOfWeek); }
    }

    private DateTime _date;
    public DateTime Date
    {
        get { return _date; }
        set { _date = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(IsToday)); }
    }

    private string _text = "";
    public string Text
    {
        get { return _text; }
        set { _text = value; RaisePropertyChanged(); }
    }

    private ObservableCollection<EventViewModel> _events = new();
    public ObservableCollection<EventViewModel> Events
    {
        get { return _events; }
        set { _events = value; RaisePropertyChanged(); }
    }
}
