using System;
using System.Globalization;
using System.Linq;
using Abp.Extensions;

namespace SW10.SWMANAGER.Helper
{
    public static class FormatterHelper
    {
        public static readonly string[] DefaultFalse = { "false", "0", "off", "no", "não", "nao" };
        public static readonly string[] DefaultTrue = { "true", "1", "on", "yes", "sim" };


        public static bool ParseBoolean(this string value, bool defaultValue = false, string[] defaultFalse = null, string[] defaultTrue = null)
        {
            if(defaultFalse == null)
            {
                defaultFalse = DefaultFalse;
            }

            if (defaultTrue == null)
            {
                defaultTrue = DefaultTrue;
            }

            if (string.IsNullOrEmpty(value) || defaultFalse.Contains(value.ToLower()))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(value) && defaultTrue.Contains(value.ToLower()))
            {
                return true;
            }
            return defaultValue;

        }

        public static string Capitalize(this string word)
        {
            return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        }
        
        
        /// <summary>
        /// The string to double.
        /// </summary>
        /// <param name="valor">
        /// The valor.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        public static double StringToDouble(string valor, IFormatProvider culture)
        {
            var result = StringToDoubleNullable(valor, culture);
            return !result.HasValue ? 0d : result.Value;

        }
        
        public static double? StringToDoubleNullable(string valor, IFormatProvider culture)
        {
            var result = 0d;
            if (valor.IsNullOrEmpty())
            {
                return null;
            }

            var styles = NumberStyles.AllowParentheses | NumberStyles.AllowLeadingWhite |
                         NumberStyles.AllowTrailingWhite
                         | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;
            var resultEn = 0d;
            var resultPtbr = 0d;
            if (double.TryParse(valor, styles, CultureInfo.InvariantCulture, out resultEn) && double.TryParse(valor, styles, culture, out resultPtbr))
            {
                return resultEn <= resultPtbr? resultEn:resultPtbr;
            }

            return result;

        }
    }
}
