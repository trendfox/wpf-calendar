using System;

namespace CalendarControls;

public interface IEvent
    : IDateRange
{
    string Title { get; }
}
