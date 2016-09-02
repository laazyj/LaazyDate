using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using LaazyDate.Utils;

namespace LaazyDate
{
    [Serializable]
    [TypeConverter(typeof(DateTypeConverter))]
    public class Date : IEquatable<Date>, IEquatable<DateTime>, IFormattable, ISerializable, IComparable, IComparable<Date>, IComparable<DateTime>
    {
        public static readonly Date Empty = new Date(DateTime.MinValue);

        readonly DateTime _value;

        public Date(DateTime dateTime)
        {
            _value = dateTime.Date;
        }

        public Date(int year, int month, int day)
        {
            _value = new DateTime(year, month, day);
        }

        public static Date Parse(string dateValue, CultureInfo culture)
        {
            Guard.Against<FormatException>(!IsValidDateString(dateValue, culture),
                "Value '{0}' cannot be parsed as a Date.", dateValue);
            return new Date(DateTime.Parse(dateValue, culture));
        }

        public int Year => _value.Year;

        public int Month => _value.Month;

        public int Day => _value.Day;

        public DayOfWeek DayOfWeek => _value.DayOfWeek;

        public int DayOfYear => _value.DayOfYear;

        public override string ToString()
        {
            return ToShortDateString();
        }

        public string ToShortDateString()
        {
            return _value.ToShortDateString();
        }

        public string ToShortDateString(IFormatProvider formatProvider)
        {
            return ToString("d", formatProvider);
        }

        public string ToLongDateString()
        {
            return _value.ToLongDateString();
        }

        public string ToLongDateString(IFormatProvider formatProvider)
        {
            return ToString("D", formatProvider);
        }

        /// <summary>
        /// Returns the value as a string, using the specified format string and <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="format">"d" or "D", representing ShortDateString() or LongDateString(), or a custom date format string.</param>
        /// <param name="formatProvider">Any valid <see cref="IFormatProvider"/>.</param>
        /// <returns>A string representing the value of the current instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
                format = "d";

            if (formatProvider == null) 
                formatProvider = SystemCulture.Current;

            return _value.ToString(format, formatProvider);
        }

        public Date AddDays(int days)
        {
            return new Date(_value.AddDays(days));
        }

        public Date AddMonths(int months)
        {
            return new Date(_value.AddMonths(months));
        }

        public DateTime AddTime(int hours, int minutes, int seconds)
        {
            return _value.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
        }

        public DateTime AddTime(TimeSpan timeSpan)
        {
            return _value.Add(timeSpan);
        }

        static bool IsValidDateString(string dateValue, CultureInfo culture)
        {
            DateTime test;
            return DateTime.TryParse(dateValue, culture, DateTimeStyles.None, out test);
        }

        #region Operators

        public static explicit operator Date(DateTime dateTime)
        {
            return new Date(dateTime);
        }

        public static explicit operator DateTime(Date date)
        {
            if (ReferenceEquals(date, null)) return DateTime.MinValue;
            return date._value;
        }

        public static explicit operator DateTime?(Date date)
        {
            if (ReferenceEquals(date, null)) return null;
            return date._value;
        }

        public static bool operator ==(Date date1, Date date2)
        {
            if (ReferenceEquals(date1, null) && ReferenceEquals(date2, null)) return true;
            if (ReferenceEquals(null, date1)) return false;
            return date1.Equals(date2);
        }

        public static bool operator !=(Date date1, Date date2)
        {
            if (ReferenceEquals(date1, null) && ReferenceEquals(date2, null)) return false;
            if (ReferenceEquals(null, date1)) return false;
            return !date1.Equals(date2);
        }

        public static bool operator >(Date date1, Date date2)
        {
            if (ReferenceEquals(date1, null) || ReferenceEquals(date2, null)) return false;
            return date1._value > date2._value;
        }

        public static bool operator <(Date date1, Date date2)
        {
            if (ReferenceEquals(date1, null) || ReferenceEquals(date2, null)) return false;
            return date1._value < date2._value;
        }

        public static bool operator >=(Date date1, Date date2)
        {
            if (ReferenceEquals(date1, null) || ReferenceEquals(date2, null)) return false;
            return date1._value >= date2._value;
        }

        public static bool operator <=(Date date1, Date date2)
        {
            if (ReferenceEquals(date1, null) || ReferenceEquals(date2, null)) return false;
            return date1._value <= date2._value;
        }

        #endregion

        #region Equality

        /// <summary>
        /// IEquatable&lt;Date&gt;
        /// </summary>
        public bool Equals(Date other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other._value == _value;
        }

        /// <summary>
        /// IEquatable&lt;DateTime&gt;
        /// </summary>
        public bool Equals(DateTime other)
        {
            return other == _value;
        }

        public override bool Equals(object obj)
        {
            if (obj is DateTime) return Equals((DateTime)obj);
            return Equals(obj as Date);
        }

        public override int GetHashCode()
        {
            var hash = 23;
            hash = (hash * 37) + _value.GetHashCode();
            return hash;
        }

        #endregion

        #region ISerializable

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("value", _value);
        }

        protected Date(SerializationInfo info, StreamingContext context)
        {
            Guard.AgainstNullArgument(info);
            _value = info.GetDateTime("value");
        }

        #endregion

        # region IComparable

        public int CompareTo(object other)
        {
            if (other is Date) return CompareTo((Date)other);
            if (other is DateTime) return CompareTo((DateTime)other);
            throw new ArgumentException("Date can only be compared to other Date or DateTime objects", nameof(other));
        }

        public int CompareTo(Date other)
        {
            return CompareTo((DateTime)other);
        }

        public int CompareTo(DateTime other)
        {
            return ((DateTime)this).CompareTo(other);
        }

        # endregion
    }

    public static class DateExtensions
    {
        public static bool IsEmpty(this Date date)
        {
            return ReferenceEquals(date, null) || date == Date.Empty;
        }
    }
}
