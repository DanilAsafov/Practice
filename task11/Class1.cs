using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;

namespace task11;

public interface ICalculator {
    int Add(int a, int b);
    int Minus(int a, int b);
    int Mul(int a, int b);
    int Div(int a, int b);
}

public class Program {
    public static ICalculator CreateCalculator() {
        string code = @"
        using task11;
        
        public class Calculator : ICalculator
        {
            public int Add(int a, int b) => a + b;
            public int Minus(int a, int b) => a - b;
            public int Mul(int a, int b) => a * b;
            public int Div(int a, int b) => a / b;
        }";

        var syntaxTree = CSharpSyntaxTree.ParseText(code);
        
        var references = new List<MetadataReference> {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location)
        };

        var compilation = CSharpCompilation.Create("DynamicCalculatorAssembly")
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(references)
            .AddSyntaxTrees(syntaxTree);

        using var ms = new MemoryStream();
        EmitResult result = compilation.Emit(ms);
        
        if (!result.Success) {
            throw new InvalidOperationException("Ошибка: " + 
                string.Join("\n", result.Diagnostics
                    .Where(d => d.Severity == DiagnosticSeverity.Error)
                    .Select(d => d.GetMessage())));
        }

        ms.Seek(0, SeekOrigin.Begin);
        Assembly assembly = Assembly.Load(ms.ToArray());
        Type calculatorType = assembly.GetType("Calculator");
        return (ICalculator)Activator.CreateInstance(calculatorType);
    }

    public static void Main() {
        ICalculator calculator = CreateCalculator();
        
        Console.WriteLine($"Add: {calculator.Add(5, 3)}");
        Console.WriteLine($"Minus: {calculator.Minus(5, 3)}");
        Console.WriteLine($"Mul: {calculator.Mul(5, 3)}");
        Console.WriteLine($"Div: {calculator.Div(6, 3)}");
    }
}
