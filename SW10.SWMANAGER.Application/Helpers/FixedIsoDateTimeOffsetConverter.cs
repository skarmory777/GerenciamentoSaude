using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace SW10.SWMANAGER.Helpers
{
    public class FixedIsoDateTimeOffsetConverter : IsoDateTimeConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?);
        }

        public FixedIsoDateTimeOffsetConverter() : base()
        {
            this.DateTimeStyles = DateTimeStyles.AssumeUniversal;
        }
    }
}
