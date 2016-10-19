using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insect.Authentication
{
    public static class StringExtensions
    {
        internal static bool NotEmpty(this string val)
        {
            return !string.IsNullOrWhiteSpace(val);
        }
    }
}
