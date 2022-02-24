using CsvHelper.Configuration.Attributes;
using System;

namespace PompInspector.Model;

public class Reading
{
    [Index(0)]
    public DateTime Date { get; set; }
    [Index(1)]
    public long InitialReading { get; set; }
    [Index(2)]
    public long FinalReading { get; set; }
    [Index(3)]
    public int Production { get; set; }
    [Index(4)]
    public int Voltage { get; set; }
    [Index(5)]
    public int Amperage { get; set; }
    [Index(6)]
    public int CubicMeterPerHour { get; set; }
    [Index(7)]
    public long PompRuntime { get; set; }
}