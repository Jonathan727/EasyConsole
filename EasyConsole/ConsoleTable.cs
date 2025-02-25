using System.Collections;
using System.Reflection;

namespace EasyConsole;

public class ConsoleTable<TData>
{
    private static readonly IEnumerable<PropertyInfo> PublicProperties = typeof(TData).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(property => property.CanRead);

    private readonly List<ColumnInfo> _columnInfos = new();
    private readonly List<List<string>> _dataValues;

    public IReadOnlyList<ColumnInfo> ColumnInfos => _columnInfos;

    public ConsoleColor TitleTextColor { get; set; } = ConsoleColor.White;

    public ConsoleColor HeaderTextColor { get; set; } = ConsoleColor.Cyan;

    public ConsoleColor RowTextColor { get; set; } = ConsoleColor.Gray;

    public ConsoleColor BorderColor { get; set; } = ConsoleColor.DarkGray;

    public BorderStyleOption BorderStyle { get; set; } = BorderStyleOption.SingleLine;

    public string Title { get; set; }

    public IReadOnlyList<TData> TableData { get; }

    public bool EnableRowSeparators { get; set; }

    public bool EnableWordWrap { get; set; }

    public ConsoleTable(string title, IReadOnlyList<TData> tableData)
    {
        Title = title;
        TableData = tableData;

        _dataValues = TableData.Select(row => GetRowValues(row).ToList()).ToList();

        var i = 0;
        foreach (var publicProperty in PublicProperties)
        {
            var columnValues = _dataValues.Select(row => row[i]).ToList();
            var width = CalculateColumnWidth(publicProperty, columnValues);
            _columnInfos.Add(new ColumnInfo(publicProperty.Name, width, 20, publicProperty));
            i++;
        }
    }

    public void Render()
    {
        var currentTextColor = Console.ForegroundColor;

        // title
        var titleLength = Min(Title.Length, Console.BufferWidth - 1);

        Console.ForegroundColor = BorderColor;
        Console.Write(BorderStyle[BorderCharacter.DownAndRight]);
        Console.Write(new string(BorderStyle[BorderCharacter.Horizontal], titleLength));
        Console.WriteLine(BorderStyle[BorderCharacter.DownAndLeft]);
        Console.Write(BorderStyle[BorderCharacter.Vertical]);
        Console.ForegroundColor = TitleTextColor;
        Console.Write(Title.Length > titleLength ? Title[..titleLength] : Title);
        Console.ForegroundColor = BorderColor;
        Console.WriteLine(BorderStyle[BorderCharacter.Vertical]);

        // border above the header row.
        Console.ForegroundColor = BorderColor;
        var renderIntersection = false;
        var visited = false;
        for (var i = 0; i < ColumnInfos.Count; i++)
        {
            var columnInfo = ColumnInfos[i];

            if (renderIntersection)
            {
                Console.Write(BorderStyle[BorderCharacter.VerticalAndHorizontal]);
                renderIntersection = false;
            }
            else
            {
                Console.Write(BorderStyle[i == 0 ? BorderCharacter.VerticalAndRight : BorderCharacter.DownAndHorizontal]);
            }

            if ((Console.CursorLeft + columnInfo.RenderWidth > titleLength && Console.CursorLeft <= titleLength + 1) || (i == ColumnInfos.Count - 1 && !visited))
            {
                visited = true;
                var endOfColumn = Console.CursorLeft + columnInfo.RenderWidth;
                var x1 = titleLength + 1 - Console.CursorLeft;
                Console.Write(new string(BorderStyle[BorderCharacter.Horizontal], x1));

                if (Console.CursorLeft == endOfColumn)
                {
                    renderIntersection = true;
                }
                else
                {
                    Console.Write(BorderStyle[BorderCharacter.UpAndHorizontal]);
                }

                var x2 = endOfColumn - Console.CursorLeft;
                if (x2 > 0)
                {
                    Console.Write(new string(BorderStyle[BorderCharacter.Horizontal], x2));
                }
            }
            else
            {
                Console.Write(new string(BorderStyle[BorderCharacter.Horizontal], columnInfo.RenderWidth));
            }
        }

        RenderEndOfLine(BorderStyle[BorderCharacter.Horizontal], BorderStyle[BorderCharacter.DownAndLeft]);

        // header row with labels
        foreach (var columnInfo in ColumnInfos)
        {
            Console.ForegroundColor = BorderColor;
            Console.Write(BorderStyle[BorderCharacter.Vertical]);
            Console.ForegroundColor = HeaderTextColor;

            Console.Write(
                columnInfo.Name.Length > columnInfo.RenderWidth
                    ? columnInfo.Name[..columnInfo.RenderWidth]
                    : columnInfo.Name.PadRight(columnInfo.RenderWidth));
        }

        RenderEndOfLine();
        if (EnableRowSeparators)
        {
            RenderRowSeparator();
        }

        // data rows
        for (var row = 0; row < TableData.Count; row++)
        {
            var rowData = new string[ColumnInfos.Count];

            // populate rowData
            for (var column = 0; column < ColumnInfos.Count; column++)
            {
                var cellData = _dataValues[row][column];
                rowData[column] = cellData;
            }

            bool moreWordWrapNeeded;
            do
            {
                moreWordWrapNeeded = false;
                for (var j = 0; j < ColumnInfos.Count; j++)
                {
                    var columnInfo = ColumnInfos[j];
                    Console.ForegroundColor = BorderColor;
                    Console.Write(BorderStyle[BorderCharacter.Vertical]);

                    Console.ForegroundColor = RowTextColor;
                    var cellData = rowData[j];

                    rowData[j] = cellData;

                    if (cellData.Length > columnInfo.RenderWidth)
                    {
                        Console.Write(cellData[..columnInfo.RenderWidth]);
                        rowData[j] = cellData.Substring(columnInfo.RenderWidth, cellData.Length - columnInfo.RenderWidth);
                        moreWordWrapNeeded = true;
                    }
                    else
                    {
                        Console.Write(cellData.PadRight(columnInfo.RenderWidth));
                        rowData[j] = string.Empty;
                    }
                }

                RenderEndOfLine();
            } while (moreWordWrapNeeded && EnableWordWrap);

            if (EnableRowSeparators && row < TableData.Count - 1)
            {
                RenderRowSeparator();
            }
        }

        // bottom border row
        Console.ForegroundColor = BorderColor;
        for (var i = 0; i < ColumnInfos.Count; i++)
        {
            var columnInfo = ColumnInfos[i];
            Console.Write(BorderStyle[i == 0 ? BorderCharacter.UpAndRight : BorderCharacter.UpAndHorizontal]);
            Console.Write(new string(BorderStyle[BorderCharacter.Horizontal], columnInfo.RenderWidth));
        }

        RenderEndOfLine(BorderStyle[BorderCharacter.Horizontal], BorderStyle[BorderCharacter.UpAndLeft]);

        Console.ForegroundColor = currentTextColor;

        void RenderRowSeparator()
        {
            Console.ForegroundColor = BorderColor;
            for (var i = 0; i < ColumnInfos.Count; i++)
            {
                var columnInfo = ColumnInfos[i];
                Console.Write(BorderStyle[i == 0 ? BorderCharacter.VerticalAndRight : BorderCharacter.VerticalAndHorizontal]);
                Console.Write(new string(BorderStyle[BorderCharacter.Horizontal], columnInfo.RenderWidth));
            }

            RenderEndOfLine(BorderStyle[BorderCharacter.Horizontal], BorderStyle[BorderCharacter.VerticalAndLeft]);
        }

        void RenderEndOfLine(char paddingCharacter = ' ', char? endingCharacter = null)
        {
            endingCharacter ??= BorderStyle[BorderCharacter.Vertical];
            Console.ForegroundColor = BorderColor;
            if (Console.CursorLeft < Console.BufferWidth - 2)
            {
                var padCharCount = Console.BufferWidth - 2 - Console.CursorLeft;
                Console.Write(new string(paddingCharacter, padCharCount));
            }

            Console.WriteLine(endingCharacter);
        }
    }

    private static int CalculateColumnWidth(PropertyInfo publicProperty, IReadOnlyCollection<TData> tableData)
    {
        return tableData.Select(data => GetCellValue(publicProperty, data).Length)
            .Prepend(publicProperty.Name.Length)
            .Max();
    }

    private static int CalculateColumnWidth(PropertyInfo publicProperty, IEnumerable<string> propertyValues)
    {
        return Max(propertyValues.Max(x => x.Length), publicProperty.Name.Length);
    }

    private static IEnumerable<string> GetRowValues(TData rowData)
    {
        return PublicProperties.Select(propertyInfo => GetCellValue(propertyInfo, rowData));
    }

    private static string GetCellValue(PropertyInfo propertyInfo, TData item)
    {
        var value = propertyInfo.GetValue(item);
        return value switch
        {
            string => value.ToString() ?? "null",
            IEnumerable enumerable => string.Join(", ", enumerable.Cast<object>().Select(x => x?.ToString() ?? "null")),
            _ => value?.ToString() ?? "null",
        };
    }

    private static T Min<T>(T first, T second) where T : IComparable<T>
    {
        return Comparer<T>.Default.Compare(first, second) < 0 ? first : second;
    }

    private static T Max<T>(T first, T second) where T : IComparable<T>
    {
        return Comparer<T>.Default.Compare(first, second) > 0 ? first : second;
    }

    public class ColumnInfo
    {
        public ColumnInfo(string name, int dataWidth, int maxWidth, PropertyInfo propertyInfo)
        {
            Name = name;
            DataWidth = dataWidth;
            MaxWidth = maxWidth;
            PropertyInfo = propertyInfo;
        }

        public string Name { get; }

        public int DataWidth { get; }

        public int MaxWidth { get; set; }

        /// <summary>
        /// Calculates the smaller of <see cref="DataWidth"/> or <see cref="MaxWidth"/>
        /// </summary>
        public int RenderWidth => Min(MaxWidth, DataWidth);

        public PropertyInfo PropertyInfo { get; }
    }

    public enum BorderCharacter
    {
        /// <summary>
        /// │
        /// </summary>
        Vertical,

        /// <summary>
        /// ┤
        /// </summary>
        VerticalAndLeft,

        /// <summary>
        /// ┐
        /// </summary>
        DownAndLeft,

        /// <summary>
        /// └
        /// </summary>
        UpAndRight,

        /// <summary>
        /// ┴
        /// </summary>
        UpAndHorizontal,

        /// <summary>
        /// ┬
        /// </summary>
        DownAndHorizontal,

        /// <summary>
        /// ├
        /// </summary>
        VerticalAndRight,

        /// <summary>
        /// ─
        /// </summary>
        Horizontal,

        /// <summary>
        /// ┼
        /// </summary>
        VerticalAndHorizontal,

        /// <summary>
        /// ┘
        /// </summary>
        UpAndLeft,

        /// <summary>
        /// ┌
        /// </summary>
        DownAndRight,
    }

    public class BorderStyleOption
    {
        public static readonly BorderStyleOption SingleLine = new("│┤┐└┴┬├─┼┘┌");

        public static readonly BorderStyleOption DoubleLine = new("║╣╗╚╩╦╠═╬╝╔");

        public static readonly BorderStyleOption DoubleLineHorizontal = new("│╡╕╘╧╤╞═╪╛╒");

        public static readonly BorderStyleOption DoubleLineVertical = new("║╢╖╙╨╥╟─╫╜╓");

        private readonly string _characters;

        private BorderStyleOption(string characters)
        {
            _characters = characters;
        }

        public char this[int index] => _characters[index];

        public char this[BorderCharacter which] => _characters[(int)which];
    }
}