using System.Collections;
using System.Reflection;

namespace EasyConsole
{
    public class ConsoleList<T>
    {
        private const int DefaultDisplayAtOneTime = 100;
        private static readonly IEnumerable<PropertyInfo> PropertyInfos = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.CanRead);

        public IReadOnlyList<T> Items { get; }
        public int DisplayAtOneTime { get; }

        public ConsoleList(IReadOnlyList<T> items, int displayAtOneTime = DefaultDisplayAtOneTime)
        {
            if (displayAtOneTime == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(displayAtOneTime), displayAtOneTime, "Cannot be zero");
            }
            Items = items;
            DisplayAtOneTime = displayAtOneTime;
        }


        public void Render()
        {
            var i = 0;
            foreach (var item in Items)
            {
                i++;
                Output.WriteLine(string.Empty);

                foreach (var propertyInfo in PropertyInfos)
                {
                    var value = GetPropertyValue(item, propertyInfo);
                    Output.WriteLine($"{propertyInfo.Name}: {value}");
                }

                Output.WriteLine(string.Empty);
                if (i % DisplayAtOneTime == 0 && !Input.ReadBool($"Finished Displaying items 1 through {i:N0} of {Items.Count:N0}. Display More?", true))
                {
                    return;
                }
            }
        }

        private static string GetPropertyValue(T item, PropertyInfo propertyInfo)
        {
            var value = propertyInfo.GetValue(item);
            return value switch
            {
                string => value.ToString() ?? "null",
                IEnumerable enumerable => string.Join(", ", enumerable.Cast<object>().Select(x => x?.ToString() ?? "null")),
                _ => value?.ToString() ?? "null",
            };
        }
    }
}
