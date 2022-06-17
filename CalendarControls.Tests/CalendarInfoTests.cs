namespace CalendarControls.Tests;

public class CalendarInfoTests
{
    private readonly CalendarInfo _calendarInfo = new();

    [Theory]
    [InlineData(2021, 2, 4)]
    [InlineData(2022, 1, 6)]
    [InlineData(2022, 4, 5)]
    [InlineData(2022, 5, 6)]
    [InlineData(2022, 6, 5)]
    [InlineData(2022, 7, 5)]
    public void GetMonthlyWeekCountReturnsCorrectCount(int year, int month, int expectedWeekCount)
    {
        var actualWeekCount = _calendarInfo.GetMonthWeekCount(year, month);
        Assert.Equal(expectedWeekCount, actualWeekCount);
    }
}
