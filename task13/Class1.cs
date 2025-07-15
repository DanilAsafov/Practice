using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13;

public class Subject {
    public string Name {get; set;}
    public int Grade {get; set;}
}


public class Student {
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public DateTime BirthDate {get; set;}
        
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Subject> Grades {get; set;}
}


public static class StudentJsonSerializer {
    private static readonly JsonSerializerOptions _options = new() {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
        Converters = {new DateTimeConverter()}
    };

    public static string Serialize(Student student) 
        => JsonSerializer.Serialize(student, _options);

    public static Student Deserialize(string json) {
        var student = JsonSerializer.Deserialize<Student>(json, _options)
            ?? throw new InvalidOperationException("Ошибка: пустой объект");

        ValidateStudent(student);
        return student;
    }

    public static void SaveToFile(string json, string filePath) 
        => File.WriteAllText(filePath, json);

    public static string LoadFromFile(string filePath) 
        => File.ReadAllText(filePath);

    private static void ValidateStudent(Student student) {
        if (string.IsNullOrWhiteSpace(student.FirstName))
            throw new InvalidOperationException("Ошибка: требуется FirstName");
            
        if (string.IsNullOrWhiteSpace(student.LastName))
            throw new InvalidOperationException("Ошибка: требуется LastName");
    }

    private class DateTimeConverter : JsonConverter<DateTime> {
        private const string Format = "dd.MM.yyyy";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DateTime.ParseExact(reader.GetString()!, Format, null);

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) {
                writer.WriteStringValue(value.ToString(Format));
        }
    }
}
