using CalendarControls;
using System;

namespace CalendarDemo;

public class DebitEvent
    : IEvent
{
    public string Title { get; set; } = "";

    public DateTime Start { get; set; }

    public DateTime End { get; set; }
}
