using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CalendarControls;

/// <summary>
/// Provides some basic calendar calculations.
/// </summary>
public class CalendarInfo
{
    private CultureInfo _culture;
    private DayOfWeek _firstDayOfWeek;

    public CalendarInfo()
    {
        _culture = CultureInfo.CurrentCulture;
        _firstDayOfWeek = _culture
            .DateTimeFormat
            .FirstDayOfWeek;
    }

    public int GetDaysInPreviousMonth(DateTime firstDayOfMonth)
    {
        var firstDayDelta = firstDayOfMonth.DayOfWeek - _firstDayOfWeek;

        if (firstDayDelta < 0)
            firstDayDelta += 7;

        return firstDayDelta;
    }

    public int GetMonthWeekCount(int year, int month)
    {
        if (year < 1)
            throw new ArgumentException(
                $"Parameter \"{nameof(year)}\" has to be larger than 0, but was {year}.");

        if (month < 1 || month > 12)
            throw new ArgumentException(
                $"Parameter \"{nameof(month)}\" has to be between 1 and 12, but was {month}.");

        var daysInMonth = _culture.Calendar.GetDaysInMonth(year, month);
        var firstDayOfMonth = new DateTime(year, month, 1);
        var firstDayDelta = GetDaysInPreviousMonth(firstDayOfMonth);
        var totalDays = daysInMonth + firstDayDelta;

        return (int)Math.Ceiling(totalDays / 7d);
    }

    public IEnumerable<PackedDateRange<TRange>> PackIntoMonth<TRange>(IEnumerable<TRange> ranges)
        where TRange : IDateRange
    {
        var remainingRanges = ranges.ToList();
        var result = new List<PackedDateRange<TRange>>(remainingRanges.Count);

        var rowIndex = 0;
        var colorIndex = 0;
        DateTime? lastEnd = null;
        while (remainingRanges.Count > 0)
        {
            var earliestFittingAfterLastEnd = remainingRanges
                .Where(r => lastEnd == null || r.Start > lastEnd.Value)
                .OrderBy(r => r.Start)
                .ThenBy(r => r.End)
                .FirstOrDefault();

            if (earliestFittingAfterLastEnd is not null)
            {
                lastEnd = earliestFittingAfterLastEnd.End;
                remainingRanges.Remove(earliestFittingAfterLastEnd);
                result.Add(new PackedDateRange<TRange>(
                    rowIndex,
                    colorIndex++,
                    earliestFittingAfterLastEnd
                    ));
            }
            else
            {
                lastEnd = null;
                rowIndex++;
            }
        }

        return result;
    }
}
