using System;

namespace Clock
{
    public class MainClock
    {
        #region Dev Properties
        /// <summary>
        /// Added Minutes by the dev
        /// </summary>
        /// <value>Added Minutes</value>
        public static int DevAddedMinutes { get; private set; }
        /// <summary>
        /// Added Days by the dev
        /// </summary>
        /// <value>Added Days</value>
        public static int DevAddedDays { get; private set; }
        /// <summary>
        /// Added Years by the dev
        /// </summary>
        /// <value>Added Years</value>
        public static int DevAddedYears { get; private set; }
        /// <summary>
        /// Get the dev time, use this for debugging if necessary
        /// </summary>
        /// <returns>Dev date</returns>
        private static DateTime GetDevDateTime =>
            DateTime.Now
            .AddMinutes(DevAddedMinutes)
            .AddDays(DevAddedDays)
            .AddYears(DevAddedYears);
        #endregion

        /// <summary>
        /// Get the system date time
        /// </summary>
        private static DateTime GetDateTime => DateTime.Now;
        /// <summary>
        /// Get the year
        /// </summary>
        private static int Year => GetDateTime.Year;
        /// <summary>
        /// Get the month
        /// </summary>
        private static int Month => GetDateTime.Month;
        /// <summary>
        /// Get day of week
        /// </summary>
        private static DayOfWeek WeekDay => GetDateTime.DayOfWeek;
        /// <summary>
        /// Get day
        /// </summary>
        private static int Day => GetDateTime.Day;
        /// <summary>
        /// Get Hours
        /// </summary>
        private static int Hours => GetDateTime.Hour;
        /// <summary>
        /// Get Minutes
        /// </summary>
        private static int Minutes => GetDateTime.Minute;
        /// <summary>
        /// Get Seconds
        /// </summary>
        private static int Seconds => GetDateTime.Second;

        private static DayPhase CurrentDayPhase => GetDayPhase(Hours);

        /// <summary>
        /// Get dayphase from hours
        /// </summary>
        /// <param name="hour">Hours to get from</param>
        /// <returns>Respective day phase</returns>
        public static DayPhase GetDayPhase(int hour)
        {
            DayPhase phase = default;

            if (hour <= 2)
                phase = DayPhase.Late_Night;
            else if (hour <= 5)
                phase = DayPhase.Night_Owl;
            else if (hour <= 9)
                phase = DayPhase.Early_Morning;
            else if (hour <= 13)
                phase = DayPhase.Late_Morning;
            else if (hour <= 16)
                phase = DayPhase.Early_Afternoon;
            else if (hour <= 20)
                phase = DayPhase.Late_Afternoon;
            else if (hour <= 23)
                phase = DayPhase.Early_Night;

            return phase;
        }

        /// <summary>
        /// Static constructor
        /// </summary>
        static MainClock()
        {
            ResetDevTime();
        }

        /// <summary>
        /// Get a SavedTime containing the now time
        /// </summary>
        /// <returns>a SavedTime containing the now time</returns>
        public static SavedTime NowTime => new SavedTime(GetDateTime);

        /// <summary>
        /// Get a SavedTime containing the now time plus the dev values
        /// </summary>
        /// <returns>a SavedTime containing the now time plus dev values</returns>
        public static SavedTime DevNowTime => new SavedTime(GetDevDateTime);

        /// <summary>
        /// Add time to the dev time (Call DevNowTime to access the dev times)
        /// </summary>
        public static void DevAddTime(
            int minutes = 0,
            int days = 0,
            int years = 0)
        {
            DevAddedMinutes += minutes;
            DevAddedDays += days;
            DevAddedYears += years;
        }

        /// <summary>
        /// Reset the added dev values
        /// </summary>
        public static void ResetDevTime()
        {
            DevAddedMinutes = 0;
            DevAddedDays = 0;
            DevAddedYears = 0;
        }
    }
}

//            _________________________
//          ,'        _____            `.
//        ,'       _.'_____`._           `.
//       :       .'.-'  12 `-.`.           \
//       |      /,' 11  .   1 `.\           :
//       ;     // 10    |     2 \\          |
//     ,'     ::        |        ::         |
//   ,'       || 9   ---O      3 ||         |
//  /         ::                 ;;         |
// :           \\ 8           4 //          |
// |            \`. 7       5 ,'/           |
// |             '.`-.__6__.-'.'            |
// :              ((-._____.-))             ;
//  \             _))       ((_            /
//   `.          '--'       '--'         ,'
//     `.______________________________,'
//         ,-.
//         `-'
//            O
//             o
//              .     ____________
//             ,('`)./____________`-.-,|
//            |'-----\\--------------| |
//            |_______^______________|,|
//            |                      |   