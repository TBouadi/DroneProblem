namespace DroneProblem.Services
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;

    using DroneProblem.DataRepository;
    using DroneProblem.Helpers;

    #endregion

    public class CsvService : IService
    {
        #region

        /// <summary>
        ///     Giving the repository on the constructor for injection later on
        /// </summary>
        /// <param name="repository"> </param>
        public CsvService(IFileRepository repository) => Repository = repository;

        #endregion

        #region Properties

        /// <inheritdoc />
        public IFileRepository Repository { get; set; }

        #endregion

        /// <inheritdoc />
        public IEnumerable<ICsvFile> GetDrones()
        {
            //We know drone csvFile name is an int
            //var droneCsv = Repository.GetAllFiles().Where(csvFile => int.TryParse(csvFile.Id, out _));

            //We can identify if it is a drone csvFile as well with Id != "tube"
            //return Repository.GetAllFiles().Where(csvFile => !csvFile.Id.Equals("tube"));

            //But we also know that drone lines contains a Date so we get advantage of the implementation of the Interface ICsvLineDate
            //foo.CsvLines.Any(line => line is CsvDroneLine);
            return Repository.GetAllFiles().Where(csvFile => csvFile.IsDroneInfo());
        }

        /// <inheritdoc />
        public IEnumerable<ICsvFile> GetTubeStations() =>
            Repository.GetAllFiles().Where(csvFile => csvFile.IsTubeInfo());
    }
}