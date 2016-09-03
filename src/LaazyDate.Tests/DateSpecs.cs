using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Threading;
using GeorgeCloney;
using NUnit.Specifications;
using Should;
// ReSharper disable UnusedMember.Local

namespace LaazyDate.Tests
{
    [Unit, Subject("Date")]
    public class When_using_a_given_date : ContextSpecification
    {
        It should_be_equal_to_a_datetime_of_the_same_value = () =>
            (date.Equals(testValue.Date)).ShouldBeTrue();

        It should_be_equal_to_a_datetime_of_the_same_value_using_equality_operator = () =>
            (date == (Date)testValue.Date).ShouldBeTrue();

        It should_be_equal_to_a_date_of_the_same_value = () =>
            (date.Equals(new Date(testValue))).ShouldBeTrue();

        It should_be_equal_to_a_date_of_the_same_value_using_equality_operator = () =>
            (date == new Date(testValue)).ShouldBeTrue();

        It should_be_equal_to_itself = () =>
            (date.Equals(date)).ShouldBeTrue();

        It should_be_unequal_to_a_different_date_value = () =>
            (date != new Date(TestValue2)).ShouldBeTrue();

        It should_be_unequal_to_a_datetime_with_a_time_value = () =>
            ((DateTime)date != testValue).ShouldBeTrue();

        It should_be_castable_as_a_datetime = () =>
            ((DateTime)date == testValue.Date).ShouldBeTrue();

        It should_have_the_same_hashcode_as_another_date_of_the_same_value = () =>
            date.GetHashCode().ShouldEqual(new Date(testValue).GetHashCode());

        Because of = () =>
            date = new Date(testValue);

        static Date date;
        static DateTime testValue = new DateTime(2007, 06, 30, 4, 51, 45);
        static readonly DateTime TestValue2 = new DateTime(1977, 12, 20, 17, 27, 32);
    }

    [Unit, Subject("Date")]
    public class When_expressing_a_date_as_a_short_date_string : ContextSpecification
    {
        It should_output_as_a_short_date_string_format_using_the_current_culture_when_neither_format_nor_format_provider_are_specified = () =>
            date.ToShortDateString().ShouldEqual(testValue.ToShortDateString()); // Default for eSpares is en-GB which is dd/MM/yyyy

        It should_output_as_a_short_date_string_format_using_the_culture_when_specified_as_english_united_states = () =>
            date.ToShortDateString(new CultureInfo("en-US")).ShouldEqual("6/8/2011"); // Default for en-US is M/d/yyyy

        It should_output_as_a_short_date_string_format_using_the_culture_when_specified_as_german_germany = () =>
            date.ToShortDateString(new CultureInfo("de-DE")).ShouldEqual("08.06.2011"); // Default for de-DE is dd.MM.yyyy

        Because of = () =>
            date = new Date(testValue);

        static Date date;
        static DateTime testValue = new DateTime(2011, 6, 8, 4, 51, 45);
    }

    [Unit, Subject("Date")]
    public class When_expressing_a_date_as_a_long_date_string : ContextSpecification
    {
        It should_output_as_a_short_date_string_format_using_the_current_culture_when_neither_format_nor_format_provider_are_specified = () =>
            date.ToLongDateString().ShouldEqual(testValue.ToLongDateString()); // Default system culture (en-GB) which is dd/MM/yyyy

        It should_output_as_a_short_date_string_format_using_the_culture_when_specified_as_english_united_states = () =>
            date.ToLongDateString(new CultureInfo("en-US")).ShouldEqual("Sunday, June 12, 2011"); // Default for en-US is dddd, MMMM d, yyyy

        It should_output_as_a_short_date_string_format_using_the_culture_when_specified_as_german_germany = () =>
            date.ToLongDateString(new CultureInfo("de-DE")).ShouldEqual("Sonntag, 12. Juni 2011"); // Default for de-DE is dddd, d. MMMM yyyy

        Because of = () =>
            date = new Date(testValue);

        static Date date;
        static DateTime testValue = new DateTime(2011, 6, 12, 4, 51, 45);
    }

    [Unit, Subject("Date")]
    public class When_expressing_a_date_as_a_string_and_the_current_culture_is_english_great_britain : ContextSpecification
    {
        It should_output_as_a_short_date_string_format_using_the_english_great_britain_culture_when_neither_format_nor_format_provider_are_specified = () =>
            date.ToShortDateString().ShouldEqual("08/06/2011"); // Default for en-GB is dd/MM/yyyy

        It should_output_as_a_long_date_string_format_using_the_english_great_britain_culture_when_neither_format_nor_format_provider_are_specified = () =>
            date.ToLongDateString().ShouldEqual("08 June 2011"); // Default for en-GB is dd MMMM yyyy

        Establish context = () =>
        {
            previousCultureInfo = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        };

        Because of = () =>
            date = new Date(TestValue);

        Cleanup stuff = () =>
            Thread.CurrentThread.CurrentCulture = previousCultureInfo;

        static CultureInfo previousCultureInfo;
        static Date date;
        static readonly DateTime TestValue = new DateTime(2011, 6, 8, 4, 51, 45);
    }

    [Unit, Subject("Date")]
    public class When_expressing_a_date_as_a_string_with_a_specified_format_and_culture : ContextSpecification
    {
        It should_output_as_a_short_date_string_format_using_the_specified_culture_if_the_format_string_is_null = () =>
            date.ToString(null, new CultureInfo("en-CA")).ShouldEqual(date.ToShortDateString(new CultureInfo("en-CA")));

        It should_output_as_the_specified_length_string_format_using_the_system_default_culture_if_the_format_provider_is_null = () =>
            date.ToString("D", null).ShouldEqual(date.ToString("D", SystemCulture.Current));

        It should_output_as_a_short_date_string_format_using_the_system_default_culture_if_both_the_format_string_and_the_format_provider_are_null = () =>
            date.ToString(null, null).ShouldEqual(date.ToShortDateString(SystemCulture.Current));

        It should_support_custom_date_formats = () =>
            date.ToString("yyyy-MM-dd", null).ShouldEqual("2011-06-30");

        Because of = () =>
            date = new Date(TestValue);

        static Date date;
        static readonly DateTime TestValue = new DateTime(2011, 6, 30, 4, 51, 45);
    }

    [Unit, Subject("Date")]
    public class When_expressing_a_date_as_an_unspecified_length_string : ContextSpecification
    {
        It should_output_as_the_same_as_when_expressing_it_as_a_short_date_string = () =>
            date.ToString().ShouldEqual(date.ToShortDateString());

        Because of = () =>
            date = new Date(TestValue);

        static Date date;
        static readonly DateTime TestValue = new DateTime(2011, 6, 30, 4, 51, 45);
    }

    [Unit, Subject("Date")]
    public class When_parsing_a_date_from_a_string : ContextSpecification
    {
        It should_accept_a_string_in_yyyy_mm_dd_format = () =>
        {
            var date = Date.Parse("2011-05-04", Thread.CurrentThread.CurrentCulture);
            date.Year.ShouldEqual(2011);
            date.Month.ShouldEqual(05);
            date.Day.ShouldEqual(04);
        };

        It should_fail_on_invalid_date = () =>
            // ReSharper disable once ObjectCreationAsStatement
            Catch.Exception(() => Date.Parse("2011-02-29", Thread.CurrentThread.CurrentCulture)).ShouldBeType<FormatException>();

        It should_fail_on_an_invalid_date_for_the_given_culture = () =>
            // ReSharper disable once ObjectCreationAsStatement
            Catch.Exception(() => Date.Parse("10-31-99", new CultureInfo("en-GB"))).ShouldBeType<FormatException>();

        It should_accept_dates_that_are_valid_for_the_specified_culture = () =>
            Date.Parse("10-31-99", new CultureInfo("en-US")).ShouldEqual((Date)new DateTime(1999, 10, 31));
    }

    [Unit, Subject("Date")]
    public class When_creating_a_date_from_its_components : ContextSpecification
    {
        It should_accept_valid_yyyy_mm_dd_components = () =>
        {
            var date = new Date(2016, 04, 30);
            date.Year.ShouldEqual(2016);
            date.Month.ShouldEqual(4);
            date.Day.ShouldEqual(30);
        };

        It should_fail_on_invalid_components = () =>
            // ReSharper disable once ObjectCreationAsStatement
            Catch.Exception(() => new Date(3055, 13, 10)).ShouldBeType<ArgumentOutOfRangeException>();
    }

    [Unit, Subject("Date")]
    public class When_using_a_collection_of_dates : ContextSpecification
    {
        It should_correctly_identify_dates_contained_in_the_collection = () =>
            List.IndexOf(new Date(TestDateValue)).ShouldEqual(0);

        It should_correctly_identify_dates_not_contained_in_the_collection = () =>
            List.IndexOf(new Date(1900, 01, 26)).ShouldEqual(-1);

        It should_correctly_identify_a_date_when_cast_as_an_object = () =>
        {
            object obj = new Date(TestDateValue);
            List.IndexOf(obj).ShouldEqual(0);
        };

        It should_correctly_identify_a_datetime_contained_in_the_collection = () =>
            List.Contains(TestDateValue).ShouldBeTrue();

        Establish context = () =>
            List.Add(new Date(TestDateValue));

        static readonly ArrayList List = new ArrayList();
        static readonly DateTime TestDateValue = new DateTime(1943, 04, 6);
    }

    [Unit, Subject("Date")]
    public class When_comparing_two_null_date_instances : ContextSpecification
    {
        // ReSharper disable once EqualExpressionComparison
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        // ReSharper disable RedundantCast
        It they_should_be_equal = () =>
            ((Date)null == (Date)null).ShouldBeTrue();
        // ReSharper restore RedundantCast
    }

    [Unit, Subject("Date")]
    public class When_casting_a_null_date_to_another_type : ContextSpecification
    {
        It should_return_datetime_minvalue_when_cast_to_datetime = () =>
            ((DateTime)Subject).ShouldEqual(DateTime.MinValue);

        It should_return_null_when_cast_to_nullable_datetime = () =>
            ((DateTime?)Subject).ShouldBeNull();

        // ReSharper disable once RedundantDefaultFieldInitializer
        static readonly Date Subject = null;
    }

    [Unit, Subject("Date")]
    public class When_casting_a_date_to_another_type : ContextSpecification
    {
        It should_return_the_date_value_when_cast_to_datetime = () =>
            ((DateTime)Subject).ShouldEqual(value.Date);

        It should_return_the_date_value_when_cast_to_nullable_datetime = () =>
            ((DateTime?)Subject).ShouldEqual(value.Date);

        static DateTime value = new DateTime(1022, 04, 01, 12, 45, 00);
        static readonly Date Subject = new Date(value);
    }

    public abstract class DateAdditionContext : ContextSpecification
    {
        protected static readonly Date Today = new Date(2011, 10, 26);
        protected static readonly Date Tomorrow = new Date(2011, 10, 27);
        protected static readonly Date Yesterday = new Date(2011, 10, 25);
    }

    [Unit, Subject("Date")]
    public class When_adding_one_day_to_a_date : DateAdditionContext
    {
        It should_return_the_following_day = () =>
            Today.AddDays(1).ShouldEqual(Tomorrow);
    }

    [Unit, Subject("Date")]
    public class When_subtracting_one_day_from_a_date : DateAdditionContext
    {
        It should_return_the_previous_day = () =>
            Today.AddDays(-1).ShouldEqual(Yesterday);
    }

    [Unit, Subject("Date")]
    public class When_adding_time_to_a_date : DateAdditionContext
    {
        It should_return_the_correct_date_and_time_from_hours_minutes_seconds = () =>
            Today.AddTime(12, 31, 0).ShouldEqual(new DateTime(Today.Year, Today.Month, Today.Day, 12, 31, 0));

        It should_return_the_correct_date_and_time_from_timespan = () =>
            Today.AddTime(new TimeSpan(12, 31, 0)).ShouldEqual(new DateTime(Today.Year, Today.Month, Today.Day, 12, 31, 0));
    }

#pragma warning disable 1718
    [Unit, Subject("Date")]
    public class When_comparing_ordinality_of_two_dates : DateAdditionContext
    {
        It should_show_tomorrow_is_greater_than_or_equal_to_today = () =>
            (Tomorrow >= Today).ShouldBeTrue();
        
        // ReSharper disable once EqualExpressionComparison
        It should_show_today_is_greater_than_or_equal_to_today = () =>
            (Today >= Today).ShouldBeTrue();

        It should_not_show_yesterday_is_greater_than_or_equal_to_today = () =>
            (Yesterday >= Today).ShouldBeFalse();

        It should_show_yesterday_is_less_than_or_equal_to_today = () =>
            (Yesterday <= Today).ShouldBeTrue();
        
        // ReSharper disable once EqualExpressionComparison
        It should_show_today_is_less_than_or_equal_to_today = () =>
            (Today <= Today).ShouldBeTrue();

        It should_not_show_tomorrow_is_less_than_or_equal_to_today = () =>
            (Tomorrow <= Today).ShouldBeFalse();

        It should_show_tomorrow_is_greater_than_today = () =>
            (Tomorrow > Today).ShouldBeTrue();

        It should_not_show_yesterday_is_greater_than_today = () =>
            (Yesterday > Today).ShouldBeFalse();
        
        // ReSharper disable once EqualExpressionComparison
        It should_not_show_today_is_greater_than_today = () =>
            (Today > Today).ShouldBeFalse();

        It should_show_yesterday_is_less_than_today = () =>
            (Yesterday < Today).ShouldBeTrue();
        It should_not_show_tomorrow_is_less_than_today = () =>
            (Tomorrow < Today).ShouldBeFalse();
        
        // ReSharper disable once EqualExpressionComparison
        It should_not_show_today_is_less_than_today = () =>
            (Today < Today).ShouldBeFalse();

        It should_return_false_when_testing_greater_than_null = () =>
            (Today > null).ShouldBeFalse();

        It should_return_false_when_testing_less_than_null = () =>
            (Today < null).ShouldBeFalse();

        It should_return_false_when_testing_greater_than_or_equal_to_null = () =>
            (Today >= null).ShouldBeFalse();

        It should_return_false_when_testing_less_than_or_equal_to_null = () =>
            (Today <= null).ShouldBeFalse();
    }
#pragma warning restore 1718

    [Unit, Subject("Date")]
    public class When_serializing_a_date : ContextSpecification
    {
        It should_have_the_type_marked_as_serializable = () =>
            typeof(Date).IsSerializable.ShouldBeTrue();

        It should_serialize_and_deserialize_to_the_same_value = () =>
            serialized.Deserialize<Date>().ShouldEqual(subject);

        Because of = () =>
            serialized = subject.Serialize();

        Establish context = () =>
            subject = new Date(2012, 06, 22);

        static Date subject;
        static Stream serialized;
    }

    [Unit, Subject("Date")]
    public class When_comparing_date_to_other_dates : ContextSpecification
    {
        It should_be_greater_when_comparing_to_earlier_dates = () =>
            subject.ShouldBeGreaterThan(new Date(2013, 5, 5));

        It should_be_less_when_comparing_to_later_dates = () =>
            subject.ShouldBeLessThan(new Date(2013, 5, 30));

        It should_be_equal_when_comparing_to_same_date = () =>
            subject.CompareTo(new Date(2013, 5, 28)).ShouldEqual(0);

        Establish context = () =>
            subject = new Date(2013, 5, 28);

        static Date subject;
    }

    [Unit, Subject("Date")]
    public class When_comparing_date_to_other_datetimes : ContextSpecification
    {
        It should_be_greater_when_comparing_to_earlier_datetimes = () =>
            subject.CompareTo(new DateTime(2013, 5, 5)).ShouldEqual(1);

        It should_be_less_when_comparing_to_later_datetimes = () =>
            subject.CompareTo(new DateTime(2013, 5, 30)).ShouldEqual(-1);

        It should_be_equal_when_comparing_to_same_datetime = () =>
            subject.CompareTo(new DateTime(2013, 5, 28)).ShouldEqual(0);

        Establish context = () =>
            subject = new Date(2013, 5, 28);

        static Date subject;
    }
}
