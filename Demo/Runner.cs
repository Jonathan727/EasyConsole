namespace Demo
{
    class Runner
    {
        private static async Task Main(string[] args)
        {
            await new DemoProgram().Run();
        }
    }
}
