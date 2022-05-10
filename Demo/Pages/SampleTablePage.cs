using EasyConsole;

namespace Demo.Pages;

internal class SampleTablePage : Page
{
    public SampleTablePage(Program program) : base("Input", program)
    {
    }

    public override async Task Display()
    {
        await base.Display();

        var sampleData = new[]
        {
            new SampleData
            {
                Id = 1,
                Name = "Fido",
                Inserted = DateTime.Now,
                RoleIds = new[]
                {
                    1,
                    2,
                    3
                }
            },
            new SampleData { Id = 2, Name = "Steve",Description = "Stephen Glenn Martin (born August 14, 1945) is an American actor, comedian, writer, producer, and musician. He has earned five Grammy Awards, a Primetime Emmy Award, and was awarded an Honorary Academy Award at the Academy's 5th Annual Governors Awards in 2013.[1] Among many honors, he has received the Mark Twain Prize for American Humor, the Kennedy Center Honors, and an AFI Life Achievement Award. In 2004, Comedy Central ranked Martin at sixth place in a list of the 100 greatest stand-up comics.", Inserted = DateTime.Now.AddMinutes(23), RoleIds = null },
            new SampleData
            {
                Id = 3,
                Name = "Greg",
                Inserted = DateTime.Now.AddHours(-4),
                RoleIds = new[]
                {
                    3
                }
            },
        };

        var table = new ConsoleTable<SampleData>("Sample Table with default styling", sampleData);
        table.Render();

        table.BorderStyle = ConsoleTable<SampleData>.BorderStyleOption.DoubleLine;
        table.Title = "DoubleLine border styling";
        table.Render();

        table.BorderStyle = ConsoleTable<SampleData>.BorderStyleOption.DoubleLineHorizontal;
        table.Title = "DoubleLineHorizontal border styling";
        table.Render();

        table.BorderStyle = ConsoleTable<SampleData>.BorderStyleOption.DoubleLineVertical;
        table.Render();

        table.Title = "DoubleLineVertical table style with EnableRowSeparators set to true";
        table.EnableRowSeparators = true;
        table.Render();

        table.Title = "Word wrap enabled";
        table.EnableRowSeparators = true;
        table.EnableWordWrap = true;
        table.ColumnInfos[2]
            .MaxWidth = 60;
        table.Render();

        Input.ReadString("Press [Enter] to navigate home");
        await Program.NavigateHome();
    }

    public class SampleData
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime Inserted { get; set; }

        public int[]? RoleIds { get; set; }
    }
}