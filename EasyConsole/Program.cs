using System.Diagnostics;

namespace EasyConsole
{
    public abstract class Program
    {
        protected string Title { get; }

        public bool BreadcrumbHeader { get; }

        protected Page CurrentPage => History.Peek();

        private Dictionary<Type, Page> Pages { get; }

        public Stack<Page> History { get; }

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
            await CurrentPage.Display();
        }

        public async Task<T> NavigateTo<T>() where T : Page
        {
            SetPage<T>();

            Console.Clear();

            await CurrentPage.Display();
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

            await CurrentPage.Display();
            return CurrentPage;
        }

        public T SetPage<T>() where T : Page
        {
            if (History.Any() && CurrentPage is T currentPage)
            {
                return currentPage;
            }

            // leave the current page

            // select the new page
            if (!Pages.TryGetValue(typeof(T), out var nextPage))
            {
                throw new KeyNotFoundException($"The given page '{typeof(T)}' was not present in the program");
            }

            // enter the new page
            History.Push(nextPage);

            return CurrentPage as T ?? throw new InvalidOperationException();
        }
    }
}