using System;
using Xunit;
using task13;

namespace task13tests;

public class StudentJsonSerializerTests {
    [Fact]
    public void Serialize_ValidStudent_CorrectJson() {
        var student = new Student {
            FirstName = "Danil",
            LastName = "Asafov",
            BirthDate = new DateTime(2025, 7, 11),
            Grades = new List<Subject> {
                new Subject {Name = "Math", Grade = 5}
            }
        };

        var json = StudentJsonSerializer.Serialize(student);

        Assert.Contains("\"FirstName\": \"Danil\"", json);
        Assert.Contains("\"LastName\": \"Asafov\"", json);
        Assert.Contains("\"BirthDate\": \"11.07.2025\"", json);
        Assert.Contains("\"Grades\":", json);
        Assert.Contains("\"Name\": \"Math\"", json);
        Assert.Contains("\"Grade\": 5", json);
    }

    [Fact]
    public void Serialize_NullGrades_IgnoresInJson() {
        var student = new Student {
            FirstName = "Danil",
            LastName = "Asafov",
            BirthDate = new DateTime(2025, 7, 11),
            Grades = null
        };

        var json = StudentJsonSerializer.Serialize(student);

        Assert.DoesNotContain("\"Grades\":", json);
    }

    [Fact]
    public void Deserialize_ValidJson_CorrectObject() {
        var json = @"{
            ""FirstName"": ""Danil"",
            ""LastName"": ""Asafov"",
            ""BirthDate"": ""11.07.2025"",
            ""Grades"": [
                { ""Name"": ""Math"", ""Grade"": 5 }
            ]
        }";

        var student = StudentJsonSerializer.Deserialize(json);

        Assert.Equal("Danil", student.FirstName);
        Assert.Equal("Asafov", student.LastName);
        Assert.Equal(new DateTime(2025, 7, 11), student.BirthDate);
        Assert.Single(student.Grades);
        Assert.Equal("Math", student.Grades[0].Name);
        Assert.Equal(5, student.Grades[0].Grade);
    }

    [Fact]
    public void Deserialize_MissingRequiredField_ThrowsException() {
        var json = @"{
            ""LastName"": ""Asafov"",
            ""BirthDate"": ""11.07.2025""
        }";

        var ex = Assert.Throws<InvalidOperationException>(() => StudentJsonSerializer.Deserialize(json));
        Assert.Contains("FirstName", ex.Message);
    }

    [Fact]
    public void FileOperations_SaveAndLoad_CorrectData() {
        var student = new Student {
            FirstName = "TestFile",
            LastName = "User",
            BirthDate = new DateTime(2025, 7, 11)
        };
        
        var path = Path.GetTempFileName();

        try {
            var json = StudentJsonSerializer.Serialize(student);
            StudentJsonSerializer.SaveToFile(json, path);
            var loadedJson = StudentJsonSerializer.LoadFromFile(path);
            var loadedStudent = StudentJsonSerializer.Deserialize(loadedJson);

            Assert.Equal(student.FirstName, loadedStudent.FirstName);
            Assert.Equal(student.LastName, loadedStudent.LastName);
            Assert.Equal(student.BirthDate, loadedStudent.BirthDate);
        } finally {
            File.Delete(path);
        }
    }

    [Fact]
    public void Deserialize_InvalidDate_ThrowsException() {
        var json = @"{
            ""FirstName"": ""Test"",
            ""LastName"": ""User"",
            ""BirthDate"": ""INVALID_DATE""
        }";

        Assert.Throws<FormatException>(() => StudentJsonSerializer.Deserialize(json));
    }
}
