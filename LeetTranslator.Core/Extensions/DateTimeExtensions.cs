using System;
using System.Globalization;

namespace LeetTranslator.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToShortDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static bool IsPast(this DateTime date)
        {
            return date < DateTime.Now;
        }

        public static bool IsFuture(this DateTime date)
        {
            return date > DateTime.Now;
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        public static DateTime WithTime(this DateTime date, int hour, int minute, int second = 0)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, minute, second);
        }

        public static string ToTimeAgo(this DateTime date)
        {
            var span = DateTime.Now - date;

            if (span.TotalDays > 365)
            {
                int years = (int)(span.TotalDays / 365);
                return years + (years == 1 ? " year ago" : " years ago");
            }
            if (span.TotalDays > 30)
            {
                int months = (int)(span.TotalDays / 30);
                return months + (months == 1 ? " month ago" : " months ago");
            }
            if (span.TotalDays > 7)
            {
                int weeks = (int)(span.TotalDays / 7);
                return weeks + (weeks == 1 ? " week ago" : " weeks ago");
            }
            if (span.TotalDays > 1)
            {
                return $"{span.Days} days ago";
            }
            if (span.TotalHours > 24)
            {
                return "yesterday";
            }
            if (span.TotalHours > 1)
            {
                return $"{span.Hours} hours ago";
            }
            if (span.TotalMinutes > 1)
            {
                return $"{span.Minutes} minutes ago";
            }

            return "just now";
        }
    }
}
