using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Helper
{
    public static  class ParserHelper
    {

        public static int? TryParseNullable(this string val)
        {
            int outValue;
            return int.TryParse(val, out outValue) ? (int?)outValue : null;
        }
    }
}
