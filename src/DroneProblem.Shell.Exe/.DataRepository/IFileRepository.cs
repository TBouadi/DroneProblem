namespace DroneProblem.DataRepository
{
    #region Using

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     FileRepository
    /// </summary>
    public interface IFileRepository
    {
        /// <summary>
        ///     Get all the CsvFiles
        /// </summary>
        /// <returns> </returns>
        IEnumerable<ICsvFile> GetAllFiles();
    }
}