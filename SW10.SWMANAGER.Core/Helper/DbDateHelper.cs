namespace SW10.SWMANAGER.Helper
{
    using System;
    using System.Data.SqlTypes;

    public static class DbDateHelper
    {
        /// <summary>
        /// Replaces any date before 01.01.1753 with a Nullable of 
        /// DateTime with a value of null.
        /// </summary>
        /// <param name="date">Date to check</param>
        /// <returns>Input date if valid in the DB, or Null if date is 
        /// too early to be DB compatible.</returns>
        public static DateTime? ToNullIfTooEarlyForDb(this DateTime date)
        {
            return (date >= (DateTime)SqlDateTime.MinValue) ? date : (DateTime?)null;
        }

        public static DateTime? ToNullIfTooEarlyForDb(this DateTime? date)
        {
            if(!date.HasValue)
            {
                return (DateTime?)null;
            }

            return (date.Value >= (DateTime)SqlDateTime.MinValue) ? date.Value : (DateTime?)null;
        }
    }
}
