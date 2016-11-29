using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace org.apache.commons.cli
{
    static class UnitTestHelper
    {
        internal static bool containsAll(this IList<Option> list, IList<Option> list2)
        {
            foreach (var item in list2)
            {
                if (!list.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        internal static void setLength(this StringBuilder sb, int length)
        {
            sb.Length = length;
        }

        internal static void reset(this StringWriter writer)
        {
            writer.GetStringBuilder().Length = 0;
        }
    }
}
