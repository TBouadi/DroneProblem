namespace DroneProblem
{
    #region Using

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DroneProblem.DataClasses;
    using DroneProblem.DataRepository;
    using DroneProblem.Services;

    #endregion

    internal static class Program
    {
        

        private static async Task Main(string[] args)
        {
            var service = new CsvService(FileRepository.Instance);
            var dispatcher = new Dispatcher(service);
            dispatcher.Init();

            var dispatcherTask = new Task(async () =>
            {
                await dispatcher.Dispatch();
            });

            dispatcherTask.Wait();

            Console.WriteLine("Simulation finished, press key to close.");
            Console.ReadKey();
        }
    }
}