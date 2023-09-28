using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;

namespace SW10.SWMANAGER.Helpers
{
    public static class MathHelper
    {
        public static double TruncateTwoDigits(double value)
        {
            return Math.Truncate(value * 100) / 100;
        }

        public static double? TruncateTwoDigits(double? value)
        {
            if (!value.HasValue)
            {
                return value;
            }

            return Math.Truncate(value.Value * 100) / 100;
        }

        public static decimal TruncateTwoDigits(decimal value)
        {
            return Math.Truncate(value * 100) / 100;
        }

        public static decimal? TruncateTwoDigits(decimal? value)
        {
            if (!value.HasValue)
            {
                return value;
            }

            return Math.Truncate(value.Value * 100) / 100;
        }
        
    }
}
