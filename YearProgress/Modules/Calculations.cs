using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YearProgress
{
    public class Calculations
    {
        /// <summary>
        /// Get total days of specified year.
        /// </summary>
        /// <param name="Input">Input year.</param>
        /// <returns></returns>
        public static int TotalDaysOfYear(int Year)
        {
            int ReturnValue = 0;
            for (int Month = 1; Month <= 12; Month++)
            {
                ReturnValue += DateTime.DaysInMonth(Year, Month);
            }
            return ReturnValue;
        }
        /// <summary>
        /// Convert 1 to 01
        /// </summary>
        /// <param name="Input">Input time.</param>
        /// <returns></returns>
        public static string OneToTwo(int Input)
        {
            if (Input.ToString().Length == 1)
            {
                return "0" + Input.ToString();
            }
            else
            {
                return Input.ToString();
            }
        }
        /// <summary>
        /// Get remaining time (string) of current year.
        /// </summary>
        /// <returns></returns>
        public static string GetRemainingTime()
        {
            DateTime NextYearFirstSec = new DateTime(DateTime.Now.Year + 1, 1, 1, 0, 0, 0);
            DateTime Now = DateTime.Now;
            TimeSpan Remaining = (NextYearFirstSec - Now);
            return Remaining.Days + "d/" + OneToTwo(Remaining.Hours) + ":" + OneToTwo(Remaining.Minutes) + ":" + OneToTwo(Remaining.Seconds);
        }
    }
}
