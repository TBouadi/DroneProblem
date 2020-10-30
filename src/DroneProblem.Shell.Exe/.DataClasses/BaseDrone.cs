namespace DroneProblem.DataClasses
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Device.Location;
    using System.Threading.Tasks;

    using DroneProblem.DataRepository;

    #endregion

    public abstract class BaseDrone
    {
        private const int DRONE_SPEED = 1;

        #region

        protected BaseDrone(string inId) => Id = inId;

        #endregion

        #region Event Handling

        protected internal virtual event EventHandler<CsvDroneLine> DestinationReached;

        protected internal virtual event EventHandler ShuttingDown;

        protected internal virtual event EventHandler<TrafficReport> TrafficReport;

        #endregion

        #region Properties

        public string Id { get; }

        protected virtual Queue<CsvDroneLine> Path { get; set; }

        #endregion

        public virtual async Task QueuePath(IEnumerable<CsvDroneLine> inPath)
        {
            Path = new Queue<CsvDroneLine>(inPath);

            while (Path.Count != 0)
            {
                var csvDroneLine = Path.Dequeue();

                await MoveToNextPosition(csvDroneLine.GeoCoordinate);
                RaiseDestinationReached(csvDroneLine);
            }
        }

        public virtual void ReportTrafficConditions(DateTime inTime) =>
            RaiseTrafficReport(new TrafficReport {DroneId = Id, Speed = 1, Time = inTime});

        public virtual void Shutdown() => ShuttingDown?.Invoke(this, EventArgs.Empty);

        protected virtual Task MoveToNextPosition(GeoCoordinate inGeoCoordinate) => Task.Delay(DRONE_SPEED);

        protected virtual void RaiseDestinationReached(CsvDroneLine inInfo) =>
            DestinationReached?.Invoke(this, inInfo);

        protected virtual void RaiseTrafficReport(TrafficReport inTrafficReport) =>
            TrafficReport?.Invoke(this, inTrafficReport);
    }
}