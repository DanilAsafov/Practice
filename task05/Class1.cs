using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace task05;

public class ClassAnalyzer {
    private readonly Type _type;

    public ClassAnalyzer(Type type) => _type = type;

    public IEnumerable<string> GetPublicMethods() {
        return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Select(m => m.Name).Distinct();;
    }

    public IEnumerable<string> GetMethodParams(string methodName) {
        return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
            .Where(m => m.Name == methodName)
            .Select(m => {var parameters = m.GetParameters();
                var paramString = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                return (paramString == "") ? $": {m.ReturnType.Name}" : $"{paramString} : {m.ReturnType.Name}";
            });
    }

    public IEnumerable<string> GetAllFields() {
        return _type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Select(f => f.Name);
    }

    public IEnumerable<string> GetProperties() {
        return _type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Select(p => p.Name);
    }

    public bool HasAttribute<T>() where T : Attribute {
        return _type.GetCustomAttributes(typeof(T), false).Length != 0;
    }
}
