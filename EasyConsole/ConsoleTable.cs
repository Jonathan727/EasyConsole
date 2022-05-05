using System.Collections;
using System.Reflection;

namespace EasyConsole;

public class ConsoleTable<TData>
{
    private readonly List<ColumnInfo> _columnInfos = new();

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
        var publicProperties = typeof(TData).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.CanRead);
        foreach (var publicProperty in publicProperties)
        {
            var width = CalculateColumnWidth(publicProperty, tableData);
            _columnInfos.Add(new ColumnInfo(publicProperty.Name, width, 20, publicProperty));
        }
    }

    public void Render()
    {
        var currentTextColor = Console.ForegroundColor;

        // title
        var titleLength = new[]
        {
            Title.Length,
            Console.BufferWidth - 2,
        }.Min();

        Console.ForegroundColor = BorderColor;
        Console.Write(BorderStyle[10]);
        Console.Write(new string(BorderStyle[7], titleLength));
        Console.WriteLine(BorderStyle[2]);
        Console.Write(BorderStyle[0]);
        Console.ForegroundColor = TitleTextColor;
        Console.Write(Title.Length > titleLength ? Title[..titleLength] : Title);
        Console.ForegroundColor = BorderColor;
        Console.WriteLine(BorderStyle[0]);

        // border above the header row.
        Console.ForegroundColor = BorderColor;
        var renderIntersection = false;
        var visited = false;
        for (var i = 0; i < ColumnInfos.Count; i++)
        {
            var columnInfo = ColumnInfos[i];

            if (renderIntersection)
            {
                Console.Write(BorderStyle[8]);
                renderIntersection = false;
            }
            else
            {
                Console.Write(BorderStyle[i == 0 ? 6 : 5]);
            }

            if ((Console.CursorLeft + columnInfo.RenderWidth > titleLength && Console.CursorLeft < titleLength + 1)
                || (i == ColumnInfos.Count - 1 && !visited))
            {
                visited = true;
                var endOfColumn = Console.CursorLeft + columnInfo.RenderWidth;
                var x1 = titleLength + 1 - Console.CursorLeft;
                Console.Write(new string(BorderStyle[7], x1));

                if (Console.CursorLeft == endOfColumn)
                {
                    renderIntersection = true;
                }
                else
                {
                    Console.Write(BorderStyle[4]);
                }

                var x2 = endOfColumn - Console.CursorLeft;
                if (x2 > 0)
                {
                    Console.Write(new string(BorderStyle[7], x2));
                }
            }
            else
            {
                Console.Write(new string(BorderStyle[7], columnInfo.RenderWidth));
            }
        }

        RenderEndOfLine(BorderStyle[7], BorderStyle[2]);

        // header row with labels
        foreach (var columnInfo in ColumnInfos)
        {
            Console.ForegroundColor = BorderColor;
            Console.Write(BorderStyle[0]);
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
        for (var i = 0; i < TableData.Count; i++)
        {
            var rowData = new string[ColumnInfos.Count];

            // populate rowData
            for (var j = 0; j < ColumnInfos.Count; j++)
            {
                var columnInfo = ColumnInfos[j];
                var cellData = GetCellValue(columnInfo.PropertyInfo, TableData[i]);
                rowData[j] = cellData;
            }

            bool moreWordWrapNeeded;
            do
            {
                moreWordWrapNeeded = false;
                for (var j = 0; j < ColumnInfos.Count; j++)
                {
                    var columnInfo = ColumnInfos[j];
                    Console.ForegroundColor = BorderColor;
                    Console.Write(BorderStyle[0]);

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
            }
            while (moreWordWrapNeeded && EnableWordWrap);

            if (EnableRowSeparators
                && i < TableData.Count - 1)
            {
                RenderRowSeparator();
            }
        }

        // bottom border row
        Console.ForegroundColor = BorderColor;
        for (var i = 0; i < ColumnInfos.Count; i++)
        {
            var columnInfo = ColumnInfos[i];
            Console.Write(BorderStyle[i == 0 ? 3 : 4]);
            Console.Write(new string(BorderStyle[7], columnInfo.RenderWidth));
        }

        RenderEndOfLine(BorderStyle[7], BorderStyle[9]);

        Console.ForegroundColor = currentTextColor;

        void RenderRowSeparator()
        {
            Console.ForegroundColor = BorderColor;
            for (var i = 0; i < ColumnInfos.Count; i++)
            {
                var columnInfo = ColumnInfos[i];
                Console.Write(BorderStyle[i == 0 ? 6 : 8]);
                Console.Write(new string(BorderStyle[7], columnInfo.RenderWidth));
            }

            RenderEndOfLine(BorderStyle[7], BorderStyle[1]);
        }

        void RenderEndOfLine(char paddingCharacter = ' ', char? endingCharacter = null)
        {
            endingCharacter ??= BorderStyle[0];
            Console.ForegroundColor = BorderColor;
            if (Console.CursorLeft < Console.BufferWidth - 2)
            {
                var padCharCount = Console.BufferWidth - 2 - Console.CursorLeft;
                Console.Write(new string(paddingCharacter, padCharCount));
            }

            Console.WriteLine(endingCharacter);
        }
    }

    private static int CalculateColumnWidth(PropertyInfo publicProperty, IReadOnlyCollection<TData> tableData) =>
        tableData.Select(
                data => GetCellValue(publicProperty, data)
                    .Length)
            .Prepend(publicProperty.Name.Length)
            .Max();

    private static string GetCellValue(PropertyInfo propertyInfo, TData item)
    {
        var cellData = propertyInfo.GetValue(item, null);

        var enumerableCellDataValue = GetCommaSeparateStringFromEnumerableObject(cellData);

        if (enumerableCellDataValue != null)
        {
            return enumerableCellDataValue;
        }

        var cellValue = cellData?.ToString() ?? "null";
        return cellValue;
    }

    private static string? GetCommaSeparateStringFromEnumerableObject(object? data)
    {
        if (data == null
            || !data.GetType()
                .IsAssignableTo(typeof(IEnumerable))
            || data.GetType().IsAssignableTo(typeof(string)))
        {
            return null;
        }
        
        var items = (from object enumerableItem in (IEnumerable)data select enumerableItem.ToString() ?? string.Empty);
        return string.Join(',', items);
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

        public int RenderWidth =>
            new[]
            {
                DataWidth,
                MaxWidth
            }.Min();

        public PropertyInfo PropertyInfo { get; }
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
    }
}