using NascentiaFlow.Services;
using NodaTime.Calendars;

namespace NascentiaFlow.ViewModels;

public class TimeSlotInfoViewModel : ViewModelBase
{
    public string TextInfo { get; }

    public TimeSlotInfoViewModel()
    {
        var today = DateTimeService.GetToday().LocalDateTime.Date;
        var weekYear = WeekYearRules.Iso.GetWeekYear(today);
        var week = WeekYearRules.Iso.GetWeekOfWeekYear(today);

        TextInfo = $"{weekYear}年第{week}周";
    }
}
