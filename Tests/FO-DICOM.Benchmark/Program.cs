﻿// Copyright (c) 2012-2025 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).
#nullable disable

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace FellowOakDicom.Benchmark
{
    static class Program
    {
        static void Main()
        {
            // Run all benchmarks in assembly
            BenchmarkRunner.Run(typeof(Program).Assembly,
 //           BenchmarkRunner.Run<ParseDatasetBenchmark>(
                ManualConfig.Create(DefaultConfig.Instance)
                .WithOptions(ConfigOptions.JoinSummary)
                .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                );
        }
    }
}
