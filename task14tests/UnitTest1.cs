using System;
using Xunit;
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
