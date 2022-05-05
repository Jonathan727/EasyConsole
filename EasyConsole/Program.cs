using System.Diagnostics;

namespace EasyConsole
{
    public abstract class Program
    {
        protected string Title { get; set; }

        public bool BreadcrumbHeader { get; private set; }

        protected Page? CurrentPage => History.Any() ? History.Peek() : null;

        private Dictionary<Type, Page> Pages { get; set; }

        public Stack<Page> History { get; private set; }

        public bool NavigationEnabled => History.Count > 1;

        protected Program(string title, bool breadcrumbHeader)
        {
            Title = title;
            Pages = new Dictionary<Type, Page>();
            History = new Stack<Page>();
            BreadcrumbHeader = breadcrumbHeader;
        }

        public virtual async Task Run()
        {
            try
            {
                Console.Title = Title;

                if (CurrentPage == null)
                {
                    throw new NullReferenceException("CurrentPage is null");
                }

                await CurrentPage.Display();
            }
            catch (Exception e)
            {
                Output.WriteLine(ConsoleColor.Red, e.ToString());
            }
            finally
            {
                if (Debugger.IsAttached)
                {
                    Input.ReadString("Press [Enter] to exit");
                }
            }
        }

        public void AddPage(Page page)
        {
            var pageType = page.GetType();

            if (Pages.ContainsKey(pageType))
            {
                Pages[pageType] = page;
            }
            else
            {
                Pages.Add(pageType, page);
            }
        }

        public async Task NavigateHome()
        {
            while (History.Count > 1)
            {
                History.Pop();
            }

            Console.Clear();
            if (CurrentPage == null)
            {
                throw new InvalidOperationException();
            }
            await CurrentPage.Display();
        }

        public T SetPage<T>() where T : Page
        {
            var pageType = typeof(T);

            if (CurrentPage is T currentPage)
            {
                return currentPage;
            }

            // leave the current page

            // select the new page
            if (!Pages.TryGetValue(pageType, out var nextPage))
            {
                throw new KeyNotFoundException($"The given page '{typeof(T)}' was not present in the program");
            }

            // enter the new page
            History.Push(nextPage);

            return CurrentPage as T ?? throw new InvalidOperationException();
        }

        public async Task<T> NavigateTo<T>() where T : Page
        {
            Console.Clear();

            await SetPage<T>().Display();
            return CurrentPage as T ?? throw new InvalidOperationException();
        }

        public async Task<Page> NavigateBack()
        {
            if (!NavigationEnabled)
            {
                throw new InvalidOperationException("Cannot navigate back, navigation history is already empty");
            }
            History.Pop();

            Console.Clear();
            if (CurrentPage == null)
            {
                throw new InvalidOperationException();
            }

            await CurrentPage.Display();
            return CurrentPage;
        }
    }
}