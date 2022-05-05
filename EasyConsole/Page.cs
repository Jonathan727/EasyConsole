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
            if (Program.History.Count > 1 && Program.BreadcrumbHeader)
            {
                //TODO: just use string.join?
                string breadcrumb = null;
                foreach (var title in Program.History.Select(page => page.Title).Reverse())
                {
                    breadcrumb += title + " > ";
                }

                breadcrumb = breadcrumb.Remove(breadcrumb.Length - 3);
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