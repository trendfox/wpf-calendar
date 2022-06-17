using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace CalendarControls;

/// <summary>
/// Interaction logic for Calendar.xaml
/// </summary>
public partial class MonthCalendar
    : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DateTime>? DateClicked;
    public event EventHandler<EventClickedEventArgs>? EventClicked;

    private readonly CalendarInfo _calendarInfo = new();
    private readonly ObservableCollection<CalendarDayViewModel> _calendarDays = new();

    private int _weeks;
    public int WeeksThisMonth
    {
        get { return _weeks; }
    }

    private int _maxEventRows;
    public int MaxEventRows
    {
        get { return _maxEventRows; }
        private set { _maxEventRows = value; RaisePropertyChanged(); }
    }

    /// <summary>
    /// Get or set the current date for the calendar.
    /// </summary>
    public DateTime Date
    {
        get { return (DateTime)GetValue(DateProperty); }
        set { SetValue(DateProperty, value); }
    }

    public static readonly DependencyProperty DateProperty = DependencyProperty.Register(
        nameof(Date),
        typeof(DateTime),
        typeof(MonthCalendar),
        new PropertyMetadata(DateTime.Today, DateChangedHandler));

    /// <summary>
    /// Get or set the events to be displayed in the calendar.
    /// </summary>
    public IEnumerable<IEvent> Events
    {
        get { return (IEnumerable<IEvent>)GetValue(EventsProperty); }
        set { SetValue(EventsProperty, value); }
    }

    public static readonly DependencyProperty EventsProperty = DependencyProperty.Register(
        nameof(Events),
        typeof(IEnumerable<IEvent>),
        typeof(MonthCalendar),
        new PropertyMetadata(null, EventsChangedHandler));

    /// <summary>
    /// Default constructor.
    /// </summary>
    public MonthCalendar()
    {
        InitializeComponent();
        Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);

        icItems.ItemsSource = _calendarDays;
        UpdateCalendar();
    }
    protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private static void DateChangedHandler(object self, DependencyPropertyChangedEventArgs e)
    {
        var cal = (MonthCalendar)self;
        cal.UpdateCalendar();
        cal.UpdateEvents();
    }

    private static void EventsChangedHandler(object self, DependencyPropertyChangedEventArgs e)
    {
        var cal = (MonthCalendar)self;
        cal.UpdateEvents();
    }

    private void UpdateCalendar()
    {
        _weeks = _calendarInfo
            .GetMonthWeekCount(Date.Year, Date.Month);
        
        RaisePropertyChanged(nameof(WeeksThisMonth));
        UpdateCalendarDays(_weeks);
    }

    private void UpdateEvents()
    {
        foreach (var day in _calendarDays)
            day.Events.Clear();

        var events = Events.ToArray();

        SplitEventsIntoDays(events);
    }

    private static Dictionary<int, Color> _colors = new()
    {
        { 0, Color.FromRgb(255, 100, 100) },
        { 1, Color.FromRgb(100, 255, 100) },
        { 2, Color.FromRgb(100, 100, 255) },
        { 3, Color.FromRgb(255, 100, 255) },
        { 4, Color.FromRgb(100, 255, 255) },
        { 5, Color.FromRgb(255, 255, 100) },
    };

    private void SplitEventsIntoDays(IEvent[] events)
    {
        var packedEvents = _calendarInfo
            .PackIntoMonth(events)
            .ToArray();

        // At least 3 rows, should always be
        // displayed, even if less events
        MaxEventRows = Math.Max(
            1,
            packedEvents.Length > 0
                ? packedEvents.Max(pe => pe.PackIndex) + 1 // +1 to get count, not index
                : 0);

        var colorIndexCount = Math.Max(
            1,
            packedEvents.Length > 0
                ? packedEvents.Max(pe => pe.ColorIndex)
                : 0);

        foreach (var dayVm in _calendarDays)
        {
            var packedEventsForThisDay = packedEvents
                    .Where(pe =>
                        pe.PackedItem.Start.Date <= dayVm.Date
                        && dayVm.Date <= pe.PackedItem.End.Date);

            dayVm.Events = new ObservableCollection<EventViewModel>(
                    packedEventsForThisDay.Select(pe =>
                        new EventViewModel(
                            pe.PackedItem,
                            pe.PackedItem.Title,
                            pe.PackIndex,
                            new SolidColorBrush(_colors[pe.ColorIndex % _colors.Count]),
                            dayVm.Date)
                        ));
        }
    }

    private void UpdateCalendarDays(int weeks)
    {
        var days = weeks * 7;
        
        while (_calendarDays.Count > days)
        {
            _calendarDays.RemoveAt(_calendarDays.Count - 1);
        }
        
        while (_calendarDays.Count < days)
        {
            _calendarDays.Add(new CalendarDayViewModel());
        }

        var firstDayOfMonth = new DateTime(Date.Year, Date.Month, 1);
        var daysInPreviousMonth = _calendarInfo.GetDaysInPreviousMonth(firstDayOfMonth);
        var calendarDay = firstDayOfMonth.AddDays(-daysInPreviousMonth);

        for (int i = 0; i < days; i++)
        {
            if (_calendarDays[i] is null)
            {
                _calendarDays[i] = new CalendarDayViewModel();
            }

            if (calendarDay.Year == Date.Year && calendarDay.Month == Date.Month)
            {
                _calendarDays[i].IsInMonth = true;
                _calendarDays[i].Text = $"{calendarDay.Day}.";
            }
            else
            {
                _calendarDays[i].IsInMonth = false;

                var dayIsFirstOrLast = calendarDay.Day == 1
                    || calendarDay.Day == DateTime.DaysInMonth(calendarDay.Year, calendarDay.Month);

                if (dayIsFirstOrLast)
                {
                    _calendarDays[i].Text = $"{calendarDay:m}";
                }
                else
                {
                    _calendarDays[i].Text = $"{calendarDay.Day}.";
                }
            }

            _calendarDays[i].Date = calendarDay;
            _calendarDays[i].CalendarColumn = i % 7;
            _calendarDays[i].CalendarRow = (int)Math.Floor(i / 7f);

            calendarDay = calendarDay.AddDays(1);
        }
    }

    private void btnPreviousMonth_Click(object sender, RoutedEventArgs e)
    {
        Date = Date.AddMonths(-1);
    }

    private void btnNextMonth_Click(object sender, RoutedEventArgs e)
    {
        Date = Date.AddMonths(1);
    }

    private void ActivateDay_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var day = (CalendarDayViewModel)e.Parameter;
        DateClicked?.Invoke(this, day.Date);
    }

    private void ActivateEvent_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var ev = (EventViewModel)e.Parameter;
        var args = new EventClickedEventArgs(ev.Original, ev.Date);
        EventClicked?.Invoke(this, args);
    }
}
