using System;
using System.Reflection;

namespace task07;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DisplayNameAttribute : Attribute {
    public string DisplayName {get;}
    public DisplayNameAttribute(string displayName) => DisplayName = displayName;
}


[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute : Attribute {
    public int Major {get;}
    public int Minor {get;}

    public VersionAttribute(int major, int minor) {
        Major = major;
        Minor = minor;
    }
}


[DisplayName("Пример класса")]
[Version(1, 0)]
public class SampleClass {
    [DisplayName("Тестовый метод")]
    public void TestMethod() { }

    [DisplayName("Числовое свойство")]
    public int Number {get; set;}
}


public static class ReflectionHelper {
    public static void PrintTypeInfo(Type type) {
        var displayNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
        if (displayNameAttr != null)
            Console.WriteLine($"Отображаемое имя класса: {displayNameAttr.DisplayName}");
        
        var versionAttr = type.GetCustomAttribute<VersionAttribute>();
        if (versionAttr != null)
            Console.WriteLine($"Версия класса: {versionAttr.Major}.{versionAttr.Minor}");

        var methods = type.GetMethods().Where(m => m.GetCustomAttribute<DisplayNameAttribute>() != null);
        foreach (var method in methods) {
            var methodDisplayNameAttr = method.GetCustomAttribute<DisplayNameAttribute>();
            Console.WriteLine($"Метод: {methodDisplayNameAttr.DisplayName}");
        }

        var properties = type.GetProperties().Where(p => p.GetCustomAttribute<DisplayNameAttribute>() != null);
        foreach (var property in properties) {
            var propertyDisplayNameAttr = property.GetCustomAttribute<DisplayNameAttribute>();
            Console.WriteLine($"Свойство: {propertyDisplayNameAttr.DisplayName}");
        }
    }
}
