using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using CsvHelper;
using Newtonsoft.Json;
using NUnit.Framework;
using ScotlandsMountains.Api.Resources;
using File = ScotlandsMountains.Api.Resources.File;

namespace ScotlandsMountains.Api.Loader
{
    [TestFixture]
    public class LoadHarness
    {
        [Test]
        public void Load()
        {
            using (var fileStream = new File(FileNames.HillCsvZip))
            using (var zipStream = new ZipArchive(fileStream))
            using (var csvStream = zipStream.Entries[0].Open())
            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                var records = csv
                    .GetRecords<dynamic>()
                    .Where(x => x.Country == "S" || x.Country == "ES")
                    .ToList();

                var classifications = records
                    .SelectMany(x => (string[])x.Classification.Split(",", StringSplitOptions.RemoveEmptyEntries))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                
                
                
                var benNevis = records.Single(x => x.Name.StartsWith("Ben Nevis"));

                Console.WriteLine(JsonConvert.SerializeObject(benNevis));

                Console.WriteLine(string.Join('\n', classifications));
            }


            Assert.Pass();
        }
    }
}