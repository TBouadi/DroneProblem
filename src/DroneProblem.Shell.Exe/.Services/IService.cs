namespace DroneProblem.Services
{
    #region Using

    using System.Collections.Generic;

    using DroneProblem.DataRepository;

    #endregion

    internal interface IService
    {
        /// <summary>
        /// Repository to get the Information
        /// </summary>
        IFileRepository Repository { get; set; }

        IEnumerable<ICsvFile> GetDrones();

        IEnumerable<ICsvFile> GetTubeStations();
    }
}