using CsvHelper.Configuration;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.IO.Abstractions;
using PompInspector.Model;

namespace PompInspector.Services;

internal class CsvParser : ICsvParser
{
    private readonly IFileSystem _fileSystem;

    public CsvParser(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public IReadOnlyCollection<Reading> Read(string path)
    {
        if (!_fileSystem.File.Exists(path))
        {
            throw new FileNotFoundException();
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        using var fileStream = _fileSystem.FileStream.Create(path, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(fileStream);
        using var csv = new CsvReader(reader, config);
        var readings = csv
            .GetRecords<Reading>()
            .OrderBy(item => item.Date)
            .ToList();

        return readings;
    }

    public void Write(List<Reading> readings, string path)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        using var fileStream = _fileSystem.FileStream.Create(path, FileMode.Append, FileAccess.Write);
        using var writer = new StreamWriter(fileStream);
        using var csv = new CsvWriter(writer, config);
        csv.WriteRecords(readings);
    }
}