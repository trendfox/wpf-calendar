using System;

namespace CalendarControls;

public interface IDateRange
{
    DateTime Start { get; }
    DateTime End { get; }
}
