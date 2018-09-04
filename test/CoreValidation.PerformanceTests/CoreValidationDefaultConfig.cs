using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;

namespace CoreValidation.PerformanceTests
{
    public class CoreValidationDefaultConfig : ManualConfig
    {
        public CoreValidationDefaultConfig()
        {
            Add(new Job("CoreValidationDefault", Job.ShortRun)
                {
                    Run = {RunStrategy = RunStrategy.Throughput}
                }
                .With(Platform.X64)
                .With(Jit.RyuJit)
                .With(Runtime.Core)
                .WithGcServer(false));

            Add(CsvMeasurementsExporter.Default);
            Add(HtmlExporter.Default);
            Add(MarkdownExporter.Default);
            Add(MemoryDiagnoser.Default);
        }
    }
}