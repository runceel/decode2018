using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Helpers
{
    public static class StringUtils
    {
        public static bool IsNotNullOrEmpty(this string self) => !string.IsNullOrEmpty(self);
        public static bool IsNotNullOrWhiteSpace(this string self) => !string.IsNullOrWhiteSpace(self);
    }
}
