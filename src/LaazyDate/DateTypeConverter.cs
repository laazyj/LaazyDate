using System;
using System.ComponentModel;
using System.Linq;

namespace LaazyDate
{
    public class DateTypeConverter : TypeConverter
    {
        static readonly Type[] ConvertableTypes = { typeof(Date), typeof(DateTime), typeof(DateTime?) };

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (ConvertableTypes.Contains(sourceType))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (ConvertableTypes.Contains(destinationType))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value == null) return null;
            if (ConvertableTypes.Contains(value.GetType()))
                return new Date((DateTime)value);
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value == null) return null;
            if (ConvertableTypes.Contains(destinationType))
                return (DateTime)(Date)value;
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
