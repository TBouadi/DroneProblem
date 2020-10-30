namespace DroneProblem
{
    #region Using

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DroneProblem.DataClasses;
    using DroneProblem.DataRepository;
    using DroneProblem.Services;

    #endregion

    internal class Dispatcher : IDispatcher
    {
        #region

        public Dispatcher(IService service) => Service = service;

        #endregion

        #region Properties

        private IService Service { get; set; }

        public ConcurrentDictionary<BaseDrone, Queue<ICsvLine>> DronesPathInfo { get; }
            =
            new ConcurrentDictionary<BaseDrone, Queue<ICsvLine>>();

        public List<ICsvFile> TubeStations { get; set; } = new List<ICsvFile>();

        #endregion

        public async Task Dispatch()
        {
            var droneControTaskList = DronesPathInfo.Select(droneInfo => DroneControl(droneInfo.Key, droneInfo.Value))
                .ToList();

            await Task.WhenAll(droneControTaskList);
        }

        public void Drone_DestinationReached(object sender, CsvDroneLine inCsvDroneLine)
        {
            var drone = (BaseDrone) sender;

            Console.WriteLine(
                $"Drone {drone.Id} reached coordinate {inCsvDroneLine.GeoCoordinate} at {inCsvDroneLine.Date.TimeOfDay}.");

            //Search for nearby stations
            foreach (var _ in TubeStations.Where(pair =>
                inCsvDroneLine.GeoCoordinate.GetDistanceTo(pair.CsvLines.First().GeoCoordinate) < 350))
            {
                //Tell the drone to report the traffic
                ((BaseDrone) sender).ReportTrafficConditions(inCsvDroneLine.Date);
            }
        }

        public async Task DroneControl(BaseDrone inBaseDrone, Queue<ICsvLine> droneFile)
        {
            DateTime shutdownTime = default;
            shutdownTime = shutdownTime.AddHours(8);
            shutdownTime = shutdownTime.AddMinutes(10);

            var shutdown = false;

            while (droneFile.Count > 0 || !shutdown)
            {
                var buffer = new List<CsvDroneLine>(10);
                for (var i = 0; i < 10; i++)
                {
                    if (!droneFile.Any())
                    {
                        //No more points on Path
                        shutdown = true;
                        break;
                    }

                    var path = (CsvDroneLine) droneFile.Dequeue();

                    //Check time 08:10
                    if (shutdownTime.TimeOfDay < path.Date.TimeOfDay)
                    {
                        shutdown = true;
                        break;
                    }

                    buffer.Add(path);
                }

                await inBaseDrone.QueuePath(buffer);
            }

            //Shutdown drone command
            inBaseDrone.Shutdown();
        }

        public void Init()
        {
            var drones = Service.GetDrones();
            foreach (var droneInfo in drones)
            {
                var id = droneInfo.Id;
                var coords = droneInfo.CsvLines;

                var baseDrone = new Drone(id);

                baseDrone.DestinationReached += Drone_DestinationReached;
                baseDrone.TrafficReport += Drone_TrafficReportReceived;
                baseDrone.ShuttingDown += Drone_ShuttingDown;

                DronesPathInfo.TryAdd(baseDrone, new Queue<ICsvLine>(coords));
            }
        }

        private static void Drone_ShuttingDown(object sender, EventArgs e) =>
            Console.WriteLine($"Drone {((BaseDrone) sender).Id} shutting down.");

        private static void Drone_TrafficReportReceived(object sender, TrafficReport e) =>
            Console.WriteLine(
                $"Drone {e.DroneId} reporting traffic condition {e.TrafficCondition} at {e.Time.TimeOfDay}.");
    }
}