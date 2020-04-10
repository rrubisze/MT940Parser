using System;
using System.Collections.Generic;
using System.Text;

namespace MT940Parser
{
    public static class DateExtensions
    {
        public static string ToIso8601DateOnly(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
            // System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssK");
        }
        public static string ToIso8601DateTime(this DateTime value)
        {
            return value.ToString("yyyy-MM-ddTHH:mm:ssK");
        }
    }
    
}
