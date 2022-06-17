using System;
using System.Windows.Media;

namespace CalendarControls;

internal class EventViewModel
    : BindableBase
{
    private readonly IEvent _original;
    public IEvent Original
    {
        get { return _original; }
    }

    private string _title;
    public string Title
    {
        get { return _title; }
        set { _title = value; RaisePropertyChanged(); }
    }

    private int _eventRow;
    public int EventRow
    {
        get { return _eventRow; }
        set { _eventRow = value; RaisePropertyChanged(); }
    }

    private Brush _color;

    public Brush Color
    {
        get { return _color; }
        set { _color = value; RaisePropertyChanged(); }
    }

    private readonly DateTime _date;
    public DateTime Date
    {
        get { return _date; }
    }

    public bool IsFirst
    {
        get { return _date == _original.Start; }
    }

    public bool IsLast
    {
        get { return _date == _original.End; }
    }

    public EventViewModel(IEvent original, string title, int eventRow, Brush color, DateTime date)
    {
        _original = original;
        _title = title;
        _eventRow = eventRow;
        _color = color;
        _date = date;
    }
}
