namespace EasyConsole
{
    /// <summary>
    /// Similar to <see cref="Menu"/>, but allows multiple choices
    /// </summary>
    public class MultiChoiceActionMenu : MultiChoiceMenuBase<Func<Task>, Option, Task>
    {
        public CancellationToken CancellationToken { get; init; }
        public int MaxConcurrentTasks { get; init; } = 10;
        public TimeSpan DelayBeforeStartingEachTask { get; init; } = TimeSpan.Zero;

        public MultiChoiceActionMenu()
        {
        }

        public MultiChoiceActionMenu(string? prompt, string defaultOption, Func<Task> defaultValue) : this(prompt, new Option(defaultOption, defaultValue))
        {
        }

        public MultiChoiceActionMenu(string? prompt, Option defaultOption) : base(prompt)
        {
            DefaultOption = defaultOption;
        }

        protected override async Task OnUserAnsweredPrompt(IEnumerable<Option> selection)
        {
            if (MaxConcurrentTasks < 1)
            {
                throw new InvalidOperationException($"{nameof(MaxConcurrentTasks)} must be 1 or greater but was {MaxConcurrentTasks:N0}");
            }

            var tasksToRunNotStarted = selection.Select(x => x.Value).ToList();

            var tasksRunning = new List<Task>();
            while (tasksToRunNotStarted.Any() && tasksRunning.Count < MaxConcurrentTasks)
            {
                CancellationToken.ThrowIfCancellationRequested();
                var taskToRun = tasksToRunNotStarted.First();
                tasksRunning.Add(taskToRun.Invoke());
                tasksToRunNotStarted.Remove(taskToRun);

                await Task.Delay(DelayBeforeStartingEachTask, CancellationToken);
            }
            while (tasksRunning.Any())
            {
                Output.WriteLine($"Waiting for {tasksRunning.Count:N0} task(s) to finish. {tasksToRunNotStarted.Count:N0} task(s) queued but not started.");
                var finishedTask = await Task.WhenAny(tasksRunning);
                //await the finished task so that exceptions are thrown
                await finishedTask;
                tasksRunning.Remove(finishedTask);

                //Start more tasks
                while (tasksToRunNotStarted.Any() && tasksRunning.Count < MaxConcurrentTasks)
                {
                    CancellationToken.ThrowIfCancellationRequested();
                    var taskToRun = tasksToRunNotStarted.First();
                    tasksRunning.Add(taskToRun.Invoke());
                    tasksToRunNotStarted.Remove(taskToRun);

                    await Task.Delay(DelayBeforeStartingEachTask, CancellationToken);
                }
            }
            await Task.WhenAll();
        }

        public void Add(string name, Func<Task> callback)
        {
            Add(new Option(name, callback));
        }
    }
}