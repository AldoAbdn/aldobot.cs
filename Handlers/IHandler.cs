namespace aldobot.Handlers
{
    interface IHandler
    {
        Task SetupAsync();
        Task RunAsync();
    }
}
