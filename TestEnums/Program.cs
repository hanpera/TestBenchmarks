// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System.Text.Json.Serialization;
using static BenchmarkClass;


var summary = BenchmarkRunner.Run(typeof(Program).Assembly);

//[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
public class BenchmarkClass
{
    //[Benchmark]
    //public void Test1()
    //{
    //    _ = Colors.Red.ToString();
    //    _ = Colors.Green.ToString();
    //    _ = Colors.Blue.ToString();
    //}

    //[Benchmark]
    //public void Test2()
    //{
    //    _ = Colors.Red.ToStringFast();
    //    _ = Colors.Green.ToStringFast();
    //    _ = Colors.Blue.ToStringFast();
    //}

    [Benchmark]
    public void TestNewtonsoft()
    {
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(Data());
    }

    [Benchmark]
    public void TestSystemText()
    {
        var json = System.Text.Json.JsonSerializer.Serialize(Data());
    }

    [Benchmark]
    public void TestSystemTextSourceGen()
    {
        var json = System.Text.Json.JsonSerializer.Serialize(Data(), 
            new SourceGenerationContext().WeatherForecastArray);
    }


    public  record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

    string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"


    };

    WeatherForecast[] forecast = new WeatherForecast[5];

    public BenchmarkClass()
    {
        forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateTime.Now.AddDays(index),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
    }

    public WeatherForecast[] Data()
    {
        return forecast;
    }


};

[JsonSourceGenerationOptions(WriteIndented = true, GenerationMode = JsonSourceGenerationMode.Serialization)]
[JsonSerializable(typeof(WeatherForecast))]
[JsonSerializable(typeof(WeatherForecast[]))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}
