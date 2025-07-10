using System;
using Xunit;
using task11;

namespace task11tests;

public class CalculatorTests {
    private readonly ICalculator _calculator;

    public CalculatorTests() {
        _calculator = Program.CreateCalculator();
    }

    [Fact]
    public void Add_ShouldReturnSum() {
        Assert.Equal(5, _calculator.Add(2, 3));
        Assert.Equal(-1, _calculator.Add(2, -3));
    }

    [Fact]
    public void Minus_ShouldReturnDifference() {
        Assert.Equal(2, _calculator.Minus(5, 3));
        Assert.Equal(-8, _calculator.Minus(-5, 3));
    }

    [Fact]
    public void Mul_ShouldReturnProduct() {
        Assert.Equal(6, _calculator.Mul(2, 3));
        Assert.Equal(-6, _calculator.Mul(2, -3));
    }

    [Fact]
    public void Div_ShouldReturnQuotient() {
        Assert.Equal(2, _calculator.Div(6, 3));
        Assert.Equal(-2, _calculator.Div(6, -3));
    }

    [Fact]
    public void Div_ShouldThrowWhenDividingByZero() {
        Assert.Throws<DivideByZeroException>(() => {_calculator.Div(5, 0); });
    }
}
