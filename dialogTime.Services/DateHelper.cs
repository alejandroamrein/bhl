using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace pem.pemTime.Services
{
    public class DateHelper
    {
        public static int WeekNumber(DateTime Day)
        {
            GregorianCalendar cal = new GregorianCalendar();
            return cal.GetWeekOfYear(Day, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime WeekFirstDay(int wn, int year)
        {
            int adjust = 0;

            GregorianCalendar cal = new GregorianCalendar();
            DateTime jan1st = new DateTime(year, 1, 1);
            if (jan1st.DayOfWeek == DayOfWeek.Monday ||
                jan1st.DayOfWeek == DayOfWeek.Tuesday ||
                jan1st.DayOfWeek == DayOfWeek.Wednesday ||
                jan1st.DayOfWeek == DayOfWeek.Thursday)
            {
                adjust = 1;
            }
            else
            {
                adjust = 0;
            }
            DateTime date = cal.AddWeeks(jan1st, wn - adjust);
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }
            return date;
        }

        public static DateTime WeekLastDay(int wn, int year)
        {
            //GregorianCalendar cal = new GregorianCalendar();
            //DateTime jan1st = new DateTime(year, 1, 1);
            //DateTime date = cal.AddWeeks(jan1st, wn - 1);
            //while (date.DayOfWeek != DayOfWeek.Sunday)
            //{
            //    date = date.AddDays(1);
            //}
            //return date;
            return WeekFirstDay(wn, year).AddDays(6);
        }
    }
}
