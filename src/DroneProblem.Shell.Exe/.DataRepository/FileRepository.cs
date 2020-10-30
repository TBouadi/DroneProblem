namespace DroneProblem.DataRepository
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using DroneProblem.Helpers;

    #endregion

    public class FileRepository : IFileRepository
    {
        private const string RSC_PATH = @".\.Resources";

        #region Fields

        private static readonly object _lock = new object();

        private static IFileRepository _instance;

        #endregion

        #region

        private FileRepository(string path) => FileDirectory = new DirectoryInfo(path);

        #endregion

        #region Properties

        /// <summary>
        /// Singleton
        /// </summary>
        public static IFileRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new FileRepository(RSC_PATH);
                            return _instance;
                        }
                    }
                }

                return _instance;
            }
            set => _instance = value;
        }

        private List<ICsvFile> CsvFiles { get; set; }

        private DirectoryInfo FileDirectory { get; }

        #endregion

        /// <inheritdoc />
        public IEnumerable<ICsvFile> GetAllFiles()
        {
            if (CsvFiles != null)
            {
                return CsvFiles;
            }

            if (!FileDirectory.Exists || !FileDirectory.GetFiles().Any())
            {
                throw new ArgumentException(
                    $"Directory {FileDirectory.FullName} doesn't exists or doesn't have any files.");
            }

            CsvFiles = new List<ICsvFile>();
            CsvFiles.AddRange(FileDirectory.GetFiles("*.csv").Select(csvFile => csvFile.LoadCsvFile()));

            return CsvFiles;
        }
    }
}