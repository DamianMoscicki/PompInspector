using PompInspector.Model;
using System.Collections.Generic;

namespace PompInspector.Services;

internal interface ICsvParser
{
    IReadOnlyCollection<Reading> Read(string path);
    public void Write(List<Reading> readings, string path);
}