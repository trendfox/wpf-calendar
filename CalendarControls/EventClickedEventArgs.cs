using System;

namespace CalendarControls;

public class EventClickedEventArgs
{
    public IEvent Event { get; }
    public DateTime ClickedDay { get; }

    public EventClickedEventArgs(IEvent @event, DateTime clickedDay)
    {
        Event = @event;
        ClickedDay = clickedDay;
    }
}