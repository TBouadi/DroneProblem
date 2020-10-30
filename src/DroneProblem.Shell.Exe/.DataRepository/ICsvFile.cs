namespace DroneProblem.DataRepository
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Device.Location;

    #endregion

    public interface ICsvFile
    {
        #region Properties

        string Id { get; set; }

        List<ICsvLine> CsvLines { get; set; }

        #endregion
    }

    public interface ICsvLine
    {
        GeoCoordinate GeoCoordinate { get; set; }

        string Id { get; set; }
    }

    public interface ICsvLineDate : ICsvLine
    {
        DateTime Date { get; set; }
    }
}