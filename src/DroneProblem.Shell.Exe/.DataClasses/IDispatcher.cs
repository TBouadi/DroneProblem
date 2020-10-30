using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneProblem.DataClasses
{
    using System.Collections.Concurrent;

    using DroneProblem.DataRepository;

    internal interface IDispatcher
    {
        #region Properties

        ConcurrentDictionary<BaseDrone, Queue<ICsvLine>> DronesPathInfo { get; }

        List<ICsvFile> TubeStations { get; set; }

        #endregion

        Task Dispatch();

        void Drone_DestinationReached(object sender, CsvDroneLine e);

        Task DroneControl(BaseDrone inBaseDrone, Queue<ICsvLine> inDronePath);

        void Init();
    }
}
