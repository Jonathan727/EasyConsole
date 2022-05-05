namespace EasyConsole
{
    public abstract class Page
    {
        public string Title { get; }

        public Program Program { get; }

        protected Page(string title, Program program)
        {
            Title = title;
            Program = program;
        }

        public virtual Task Display()
        {
            if (Program.BreadcrumbHeader)
            {
                var breadcrumb = string.Join(" > ", Program.History.Reverse().Select(page => page.Title));
                Console.WriteLine(breadcrumb);
            }
            else
            {
                Console.WriteLine(Title);
            }

            Console.WriteLine("---");

            return Task.CompletedTask;
        }
    }
}