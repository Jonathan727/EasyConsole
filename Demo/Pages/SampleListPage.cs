using EasyConsole;

namespace Demo.Pages;

internal class SampleListPage : Page
{
    public SampleListPage(Program program) : base("List", program)
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
                    3,
                },
            },
            new SampleData { Id = 2, Name = "Steve",Description = "Stephen Glenn Martin (born August 14, 1945) is an American actor, comedian, writer, producer, and musician. He has earned five Grammy Awards, a Primetime Emmy Award, and was awarded an Honorary Academy Award at the Academy's 5th Annual Governors Awards in 2013.[1] Among many honors, he has received the Mark Twain Prize for American Humor, the Kennedy Center Honors, and an AFI Life Achievement Award. In 2004, Comedy Central ranked Martin at sixth place in a list of the 100 greatest stand-up comics.", Inserted = DateTime.Now.AddMinutes(23), RoleIds = null },
            new SampleData
            {
                Id = 3,
                Name = "Greg",
                Inserted = DateTime.Now.AddHours(-4),
                RoleIds = new[]
                {
                    3,
                },
            },
        };

        new ConsoleList<SampleData>(sampleData).Render();

        Input.ReadString("Press [Enter] to show a very long list");

        List<SampleData> longSampleData = new List<SampleData>();

        var id = 0;
        for (var i = 0; i < 2000; i++)
        {
            foreach (var data in sampleData)
            {
                longSampleData.Add(new SampleData
                {
                    Id = id++,
                    Description = data.Description,
                    Inserted = data.Inserted,
                    RoleIds = data.RoleIds,
                    Name = data.Name,
                });
            }
        }

        new ConsoleList<SampleData>(longSampleData).Render();

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