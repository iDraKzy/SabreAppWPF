using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SabreAppWPF.Classrooms
{
    public static class ClassroomsShared
    {
        public static int GetNextSessionTimestamp(int weekDay, int hour, int minute)
        {
            DateTime currentDay = DateTime.Today;
            int currentWeekDay = (int)currentDay.DayOfWeek;
            int daysToNextSession = 0;
            while (currentWeekDay != weekDay)
            {
                currentWeekDay++;
                if (currentWeekDay > 6) currentWeekDay = 0;
                daysToNextSession++;
            }

            currentDay.AddDays(daysToNextSession);
            currentDay.AddHours(hour);
            currentDay.AddMinutes(minute);
            int nextSessionTimestamp = (int)new DateTimeOffset(currentDay).ToUnixTimeSeconds();
            return nextSessionTimestamp;
        }
    }
}
