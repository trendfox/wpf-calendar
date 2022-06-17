using System.Windows.Input;

namespace CalendarControls;

internal static class CalendarCommands
{
	public static readonly RoutedUICommand ActivateDay = new(
			"Activate day",
			"ActivateDay",
			typeof(CalendarCommands),
			null);

	public static readonly RoutedUICommand ActivateEvent = new(
		"Activate event",
		"ActivateEvent",
		typeof(CalendarCommands),
		null);
}