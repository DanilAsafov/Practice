using System;
using Xunit;
using System.Linq;
using ScottPlot;
using System.Diagnostics;
using System.Threading;
using task14;

namespace task14tests;

public class DefiniteIntegralTests {
    [Fact]
    public void Test_LinearFunction() {
        var X = (double x) => x;

        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2), 1e-4);
        Assert.Equal(12.5, DefiniteIntegral.Solve(0, 5, X, 1e-6, 8), 1e-5);
    }

    [Fact]
    public void Test_SinFunction() {
        var SIN = (double x) => Math.Sin(x);

        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8), 1e-4);
        Assert.Equal(2, DefiniteIntegral.Solve(0, Math.PI, SIN, 1e-5, 4), 1e-4);
    }

    [Fact]
    public void Test_ConstantFunction() {
        var TWO = (double x) => 2.0;
        var FIVE = (double x) => 5.0;

        Assert.Equal(10, DefiniteIntegral.Solve(0, 5, TWO, 1e-6, 8), 1e-4);
        Assert.Equal(15, DefiniteIntegral.Solve(0, 3, FIVE, 1e-6, 4), 1e-4);
    }

    [Fact]
    public void Test_QuadraticFunction() {
        var SQUARE = (double x) => x * x;
        
        Assert.Equal(9, DefiniteIntegral.Solve(0, 3, SQUARE, 1e-5, 4), 1e-4);
        Assert.Equal(2.0/3, DefiniteIntegral.Solve(-1, 1, SQUARE, 1e-5, 4), 1e-4);
    }

    [Fact]
    public void Test_CubicFunction() {
        var CUBE = (double x) => x * x * x;

        Assert.Equal(4, DefiniteIntegral.Solve(0, 2, CUBE, 1e-5, 4), 1e-4);
        Assert.Equal(0.25, DefiniteIntegral.Solve(0, 1, CUBE, 1e-5, 4), 1e-4);
    }

    [Fact]
    public void Test_ExponentialFunction() {
        var EXP = (double x) => Math.Exp(x);

        Assert.Equal(Math.E - 1, DefiniteIntegral.Solve(0, 1, EXP, 1e-6, 8), 1e-5);
        Assert.Equal(1 - Math.Exp(-1), DefiniteIntegral.Solve(-1, 0, EXP, 1e-6, 8), 1e-5);
    }
}

public class IntegrationExperiments {
    [Fact]
    public void RunExperiments() {
        const double a = -100;
        const double b = 100;
        Func<double, double> sinFunction = Math.Sin;
        const double exactValue = 0.0;
        double[] steps = [0.1, 0.01, 0.001, 0.0001, 0.00001, 0.000001];
        double selectedStep = 0.0;

        selectedStep = steps.FirstOrDefault(step => 
            Math.Abs(DefiniteIntegral.SingleThreadIntegral(a, b, sinFunction, step) - exactValue) <= 1e-4,
            steps.Last()
        );

        Stopwatch sw = new();
        sw.Start();
        DefiniteIntegral.SingleThreadIntegral(a, b, sinFunction, selectedStep);
        sw.Stop();
        double singleThreadTime = sw.Elapsed.TotalMilliseconds;

        int minThreads = 1;
        int maxThreads = Environment.ProcessorCount * 4;
        int repeats = 5;
        double bestTime = double.MaxValue;
        int optimalThreads = 1;
        
        var threadResults = Enumerable.Range(minThreads, maxThreads - minThreads + 1)
            .Select(numThreads => {
                double totalTime = Enumerable.Range(0, repeats)
                    .Select(_ => {
                        sw.Restart();
                        DefiniteIntegral.SolveOptimized(a, b, sinFunction, selectedStep, numThreads);
                        sw.Stop();
                        return sw.Elapsed.TotalMilliseconds;
                    })
                    .Sum();
                    
                double avgTime = totalTime / repeats;
                return (numThreads, avgTime);
            })
            .ToList();

        var bestResult = threadResults.OrderBy(r => r.avgTime).First();
        optimalThreads = bestResult.numThreads;
        bestTime = bestResult.avgTime;
        
        var times = threadResults.Select(r => r.avgTime).ToList();
        var threadCounts = threadResults.Select(r => r.numThreads).ToList();

        double speedDifference = (singleThreadTime - bestTime) / singleThreadTime * 100;
        bool needOptimization = speedDifference < 15.0;

        if (needOptimization) {
            double baseTime = Enumerable.Range(0, repeats)
                .Select(_ => {
                    sw.Restart();
                    DefiniteIntegral.Solve(a, b, sinFunction, selectedStep, optimalThreads);
                    sw.Stop();
                    return sw.Elapsed.TotalMilliseconds;
                })
                .Average();
            
            if (baseTime < bestTime) {
                bestTime = baseTime;
                speedDifference = (singleThreadTime - bestTime) / singleThreadTime * 100;
            }
        }

        string resultText = $"Выбранный шаг: {selectedStep}\n" +
            $"Оптимальное количество потоков: {optimalThreads}\n" +
            $"Время однопоточного выполнения: {singleThreadTime} мс\n" +
            $"Время многопоточного выполнения: {bestTime} мс\n" +
            $"Процент уменьшения времени выполнения: {speedDifference:F2}%";
        
        string baseDir = AppContext.BaseDirectory;
        string projectRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
        string resultsDir = Path.Combine(projectRoot, "Results");
        
        Directory.CreateDirectory(resultsDir);

        string resultFilePath = Path.Combine(resultsDir, "result.txt");
        File.WriteAllText(resultFilePath, resultText);

        var plt = new Plot();
        var scatter = plt.Add.Scatter(
            threadCounts.Select(x => (double)x).ToArray(),
            times.ToArray()
        );
        scatter.Label = "Время выполнения";
        plt.Title("Зависимость времени выполнения от количества потоков");
        plt.XLabel("Количество потоков");
        plt.YLabel("Время выполнения (мс)");
        plt.ShowLegend();
        plt.Grid.IsVisible = true;
        
        string plotFilePath = Path.Combine(resultsDir, "performance.png");
        plt.SavePng(plotFilePath, 600, 400);
    }
}
