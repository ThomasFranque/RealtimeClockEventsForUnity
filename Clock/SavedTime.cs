using System;

namespace Clock
{
    /// <summary>
    /// Stores a given time, used by the event system and main clock to check
    /// and store certain times.
    /// ! MOVE THIS TO THE EVENTS CLASS
    /// When used with the event system, only the given times will be taken into
    /// account, for example, if there is a saved time only with the minutes 
    /// and hours set, means that that event will occur everyday at that given
    /// hour and those given minutes
    /// </summary>
    public struct SavedTime
    {
        /// <summary>
        /// Stored Year
        /// </summary>
        /// <value>Year</value>
        public int? Year { get; }
        /// <summary>
        /// Stored Month
        /// </summary>
        /// <value>Month</value>
        public int? Month { get; }
        /// <summary>
        /// Stored Day of Week
        /// </summary>
        /// <value>Day of week</value>
        public DayOfWeek? WeekDay { get; }
        /// <summary>
        /// Stored Day
        /// </summary>
        /// <value>Day</value>
        public int? Day { get; }
        /// <summary>
        /// Stored Day phase
        /// </summary>
        /// <value>Day phase</value>
        public DayPhase? DayPhase { get; }
        /// <summary>
        /// Stored hours in 24 format
        /// </summary>
        /// <value>Hours in 24 format</value>
        public int? Hours24 { get; }
        /// <summary>
        /// Stored Minutes
        /// </summary>
        /// <value>Minutes</value>
        public int? Minutes { get; }
        /// <summary>
        /// Stored Seconds
        /// </summary>
        /// <value>Seconds</value>
        public int? Seconds { get; }

        /// <summary>
        /// Stored Hours in 12 format
        /// </summary>
        /// <value>Hours in 12 format</value>
        public int? Hours12
        {
            get
            {
                // Check if hours exist
                if (Hours24 == null) return null;

                // Prevent midday from being 0
                if (Hours24.Value != 12)
                    return Hours24 % 12;
                else
                    return 12;
            }
        }

        /// <summary>
        /// Get the original System DateTime class (only available when reading
        /// from the main clock, else, it will never exist unless given)
        /// </summary>
        /// <value></value>
        public DateTime? SystemDateTime { get; }

        /// <summary>
        /// True if this saved time is No Time
        /// </summary>
        /// <value>Saved time is No Time</value>
        public bool NoParameters { get; }

        /// <summary>
        /// A saved time without any values.
        /// If an event with this time is passed as a clock event time, that
        /// event will occur every given repetition type.
        /// </summary>
        /// <returns>A SavedTime without parameters</returns>
        public static SavedTime NoTime =>
            new SavedTime(null, null, null, null, null, null, null, null, null);

        /// <summary>
        /// Create a new saved time from given values
        /// </summary>
        public SavedTime(int? year = null, int? month = null,
            DayOfWeek? weekDay = null, int? day = null, int? hours24 = null,
            int? minutes = null, int? seconds = null, DayPhase? dayPhase = null,
            DateTime? dateTime = null)
        {
            // Set all
            Year = year;
            Month = month;
            WeekDay = weekDay;
            Day = day;
            DayPhase = dayPhase;
            Hours24 = hours24;
            Minutes = minutes;
            Seconds = seconds;

            SystemDateTime = dateTime;

            // Check if parameters exist
            NoParameters = (Year == null && Month == null && WeekDay == null &&
                Day == null && Hours24 == null && Minutes == null &&
                Seconds == null && DayPhase == null);
        }

        /// <summary>
        /// Create saved time from given datetime
        /// </summary>
        /// <param name="dateTime"></param>
        public SavedTime(DateTime dateTime)
        {
            Year = dateTime.Year;
            Month = dateTime.Month;
            WeekDay = dateTime.DayOfWeek;
            Day = dateTime.Day;
            Hours24 = dateTime.Hour;
            DayPhase = MainClock.GetDayPhase(dateTime.Hour);
            Minutes = dateTime.Minute;
            Seconds = dateTime.Second;

            SystemDateTime = dateTime;

            NoParameters = false;
        }

        /// <summary>
        /// Custom comparator method that compares two dates, taking only the
        /// assigned parameters into account. Use MatchesWithDeep(SavedTime) 
        /// for a deep compare.
        /// </summary>
        /// <param name="dateToCompare">Other date to compare to</param>
        /// <returns>True if they match</returns>
        public bool MatchesWithShallow(SavedTime dateToCompare)
        {
            // Prevent further actions if there is one without parameters
            if (NoParameters || dateToCompare.NoParameters) return true;

            // Check Year
            if (this.Year != null && dateToCompare.Year != null)
            {
                if (this.Year != dateToCompare.Year)
                    return false;
            }

            // Check Month
            if (this.Month != null && dateToCompare.Month != null)
            {
                if (this.Month != dateToCompare.Month)
                    return false;
            }

            // Check Weekday
            if (this.WeekDay != null && dateToCompare.WeekDay != null)
            {
                if (this.WeekDay != dateToCompare.WeekDay)
                    return false;
            }

            // Check Day
            if (this.Day != null && dateToCompare.Day != null)
            {
                if (this.Day != dateToCompare.Day)
                    return false;
            }

            // Check Dayphase
            if (this.DayPhase != null && dateToCompare.DayPhase != null)
            {
                if (this.DayPhase != dateToCompare.DayPhase)
                    return false;
            }

            // Check Hours
            if (this.Hours24 != null && dateToCompare.Hours24 != null)
            {
                if (this.Hours24 != dateToCompare.Hours24)
                    return false;
            }

            // Check Minutes
            if (this.Minutes != null && dateToCompare.Minutes != null)
            {
                if (this.Minutes != dateToCompare.Minutes)
                    return false;
            }

            // Check Seconds
            if (this.Seconds != null && dateToCompare.Seconds != null)
            {
                if (this.Seconds != dateToCompare.Seconds)
                    return false;
            }

            // If we get here, the dates are considered the same
            return true;
        }

        /// <summary>
        /// Custom comparator method that compares two dates, all parameters 
        /// into account. Use MatchesWithShallow(SavedTime) 
        /// for a shallow compare.
        /// </summary>
        /// <param name="dateToCompare">Other date to compare to</param>
        /// <returns>True if they match</returns>
        public bool MatchesWithDeep(SavedTime dateToCompare)
        {
            return 
                Year == dateToCompare.Year && 
                Month == dateToCompare.Month && 
                WeekDay == dateToCompare.WeekDay &&
                Day == dateToCompare.Day && 
                Hours24 == dateToCompare.Hours24 && 
                Minutes == dateToCompare.Minutes &&
                Seconds == dateToCompare.Seconds && 
                DayPhase == dateToCompare.DayPhase;
        }
    }
}