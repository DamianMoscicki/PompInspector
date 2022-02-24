using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using PompInspector.Model;
using CsvParser = PompInspector.Services.CsvParser;

namespace PompInspector.Test.CsvParserTests;

[TestFixture]
internal class CsvWriterUnitTest
{
    #region Test Fixture

    private CsvParser _csvParser;
    private string _path;
    private List<Reading> _items;
    private MockFileSystem _fileSystem;

    [SetUp]
    public void SetUp()
    {
        _path = @"c:\test.csv";

        _fileSystem = new MockFileSystem();

        _items = new List<Reading>
        {
            new()
            {
                Date = Convert.ToDateTime("02/02/2020"), InitialReading = 100, FinalReading = 101, Production = 102,
                Voltage = 103, Amperage = 104, CubicMeterPerHour = 105, PompRuntime = 106
            }
        };

        _csvParser = new CsvParser(_fileSystem);
    }

    #endregion

    #region Tests

    [Test]
    public void ShouldWriteModelToCsv()
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        _csvParser.Write(_items, _path);

        using var reader = new StreamReader(_fileSystem.File.Open(_path, FileMode.Open, FileAccess.Read));
        using var csv2 = new CsvReader(reader, config);
        var readings = csv2
            .GetRecords<Reading>()
            .OrderBy(item => item.Date)
            .ToList();

        readings.Should().BeEquivalentTo(_items);
    }

    #endregion
}