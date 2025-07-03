using Xunit;
using System.Collections.Generic;
using task05;

namespace task05tests;

public class TestClass {
    public int PublicField;
    private string _privateField;
    public int Property {get; set;}

    public void Method() { }

    public int MethodWithParams(int a, string b) => 0;

    public static void StaticMethod() {}
}

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests {
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods() {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields() {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetProperties_ReturnsPropertyNames() {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties().ToList();

        Assert.Contains("Property", properties);
    }

    [Fact]
    public void GetMethodParams_FormatsParametersCorrectly() {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parameters = analyzer.GetMethodParams("MethodWithParams").ToList();

        Assert.Equal("Int32 a, String b : Int32", parameters.First());
    }

    [Fact]
    public void GetMethodParams_ForMethodWithoutParams() {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parameters = analyzer.GetMethodParams("Method").ToList();

        Assert.Equal(": Void", parameters.First());
    }


    [Fact]
    public void HasAttribute_WhenAttributePresent_ReturnsTrue() {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));

        Assert.True(analyzer.HasAttribute<SerializableAttribute>());
    }

    [Fact]
    public void HasAttribute_WhenAttributeAbsent_ReturnsFalse() {
        var analyzer = new ClassAnalyzer(typeof(TestClass));

        Assert.False(analyzer.HasAttribute<SerializableAttribute>());
    }
}
