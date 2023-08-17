using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.WPF.ErrorUtilities
{
    public static class ErrorUtilities
    {
        public static IEnumerable<string> NotNull(this IEnumerable<string> errors)
        {
            return errors.Where(s => !string.IsNullOrEmpty(s));
        }

        public static string ToError(this IEnumerable<string> errors)
        {
            return string.Join(Environment.NewLine, errors.NotNull());
        }

        private static string toError(params string[] errors)
        {
            return ToError(errors);
        }
    }
}