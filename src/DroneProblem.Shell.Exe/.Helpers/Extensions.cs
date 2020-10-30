namespace DroneProblem.Helpers
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Device.Location;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using DroneProblem.DataRepository;

    using Microsoft.VisualBasic.FileIO;

    #endregion

    public static class Extensions
    {
        private const NumberStyles STYLE_FLAGS = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;

        public static bool IsDroneInfo(this ICsvFile csvFile) => csvFile.CsvLines.Any(line => line is CsvDroneLine);
        public static bool IsTubeInfo(this ICsvFile csvFile) => csvFile.CsvLines.Any(line => line is CsvTubeLine);

        public static ICsvFile LoadCsvFile(this FileSystemInfo fileInfo)
        {
            using (var csvParser = new TextFieldParser(fileInfo.FullName))
            {
                csvParser.SetDelimiters(",");
                csvParser.HasFieldsEnclosedInQuotes = true;

                var csvLines = new List<ICsvLine>();
                while (!csvParser.EndOfData)
                {
                    try
                    {
                        csvLines.Add(csvParser.ReadFields().ParseLineFields());
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException($"CsvFile {fileInfo.FullName} parsing exception: {ex.Message}", ex);
                    }
                }

                //We can get the Id from the lines
                //csvFile.Id = csvFile.CsvLines.GroupBy(line => line.Id).First().Key;
                //csvFile.Id = csvFile.CsvLines.First().Id;

                //Or we can get it from the FileName
                var csvFile = new CsvFile
                {
                    Id = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length),
                    CsvLines = csvLines
                };

                return csvFile;
            }
        }

        private static GeoCoordinate ParseCsvGeoCoordinate(this (string Lat, string Long) coordinate)
        {
            if (!double.TryParse(coordinate.Lat, STYLE_FLAGS, CultureInfo.InvariantCulture, out var parsedLat))
            {
                throw new ArgumentException("Could not parse Latitude.");
            }

            if (!double.TryParse(coordinate.Long, STYLE_FLAGS, CultureInfo.InvariantCulture, out var parsedLong))
            {
                throw new ArgumentException("Could not parse Longitude.");
            }

            return new GeoCoordinate(parsedLat, parsedLong);
        }

        private static ICsvLine ParseLineFields(this IReadOnlyList<string> lineFields)
        {
            ICsvLine csvLine;

            var id = lineFields?[0];
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Could not read Id.");
            }

            var lat = lineFields[1];
            var lon = lineFields[2];

            var geoCoordinate = (lat, lon).ParseCsvGeoCoordinate();

            if (lineFields.Count > 3)
            {
                if (!DateTime.TryParse(lineFields[3], out var parsedDate))
                {
                    throw new ArgumentException("Could not parse Date.");
                }

                csvLine = new CsvDroneLine
                {
                    Id = id,
                    Date = parsedDate,
                    GeoCoordinate = geoCoordinate
                };
            }
            else
            {
                csvLine = new CsvTubeLine
                {
                    Id = id,
                    GeoCoordinate = geoCoordinate
                };
            }

            return csvLine;
        }
    }
}