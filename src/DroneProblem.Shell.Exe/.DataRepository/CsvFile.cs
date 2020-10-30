namespace DroneProblem.DataRepository
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Device.Location;

    #endregion

    public class CsvFile : ICsvFile
    {
        #region Implementation of ICsvFile

        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public List<ICsvLine> CsvLines { get; set; } = new List<ICsvLine>();

        #endregion
    }

    public class CsvTubeLine : ICsvLine
    {
        #region Properties

        /// <inheritdoc />
        public GeoCoordinate GeoCoordinate { get; set; }

        /// <inheritdoc />
        public string Id { get; set; }

        #endregion
    }

    public class CsvDroneLine : ICsvLineDate
    {
        #region Properties

        /// <inheritdoc />
        public DateTime Date { get; set; }

        /// <inheritdoc />
        public GeoCoordinate GeoCoordinate { get; set; }

        /// <inheritdoc />
        public string Id { get; set; }

        #endregion
    }
}