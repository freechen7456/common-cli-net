using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace org.apache.commons.cli
{
    internal static class ExtensionMethod
    {
        internal static bool isEmpty(this IList list)
        {
            return (list.Count == 0);
        }

        internal static bool isEmpty(this IList<Option> list)
        {
            return (list.Count == 0);
        }

        internal static IListIterator listIterator(this IList list)
        {
            return new ListIterator(list);
        }

        internal static IListIterator iterator(this IList list)
        {
            return new ListIterator(list);
        }

        internal static string ToDumpString(this IDictionary<string, Option> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Option option in dict.Values)
            {
                sb.Append(option.ToString());
            }
            return sb.ToString();
        }

        internal static IList<OptionGroup> RemoveDuplicated(this IList<OptionGroup> groups)
        {
            List<OptionGroup> groupList = new List<OptionGroup>();

            foreach (var group in groups)
            {
                if (!groupList.Contains(group))
                {
                    groupList.Add(group);
                }
            }

            return groupList;
        }

    }

    public interface IIterator
    {
        bool hasNext();

        object next();
    }

    public interface IListIterator : IIterator
    {
        object previous();
    }

    internal class ListIterator : IListIterator, IIterator
    {
        IList _list;
        int _current;

        internal ListIterator(IList list)
        {
            _list = list;
            _current = -1;
        }

        bool IIterator.hasNext()
        {
            return (_current + 1 < _list.Count);
        }

        object IIterator.next()
        {
            return _list[++_current];
        }

        object IListIterator.previous()
        {
            return _list[--_current];
        }
    }


}
