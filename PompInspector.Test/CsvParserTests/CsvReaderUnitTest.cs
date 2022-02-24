using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;
using PompInspector.Model;
using PompInspector.Services;
using FluentAssertions;
using System.IO;
using CsvHelper.Configuration;
using System.Globalization;

namespace PompInspector.Test.CsvParserTests;

[TestFixture]
internal class CsvReaderUnitTest
{
    #region Test fixture

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

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        using (var fileStream = _fileSystem.FileStream.Create(_path, FileMode.Create, FileAccess.Write))
        using (var writer = new StreamWriter(fileStream))
        using (var csv = new CsvHelper.CsvWriter(writer, config))
        {
            csv.WriteRecords(_items);
        }

        _csvParser = new CsvParser(_fileSystem);
    }

    #endregion

    #region Tests

    [Test]
    public void ShouldReturnListReadFromCsv()
    {
        _csvParser.Read(_path).Should().BeEquivalentTo(_items);
    }

    [Test]
    public void WhenFileNotFound_ShouldThrowFileNotFoundException()
    {
        Action act = () => _csvParser.Read("incorrectPath");
        act.Should().Throw<FileNotFoundException>();
    }

    #endregion
}