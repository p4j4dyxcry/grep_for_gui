using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Grepper.Extensions
{
    public static class EnumrableExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
          => items is null ? new HashSet<T>() : new HashSet<T>(items);

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
            => items is null ? new ObservableCollection<T>() : new ObservableCollection<T>(items);
    }
}
