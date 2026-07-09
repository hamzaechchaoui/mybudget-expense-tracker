using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MyBudget.Core;

namespace MyBudget.Data;

public class JsonExpenseStore : IExpenseStore
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonExpenseStore(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("File path cannot be empty.", nameof(path));
        }
        _filePath = path;

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }

    public IReadOnlyList<Expense> Load()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Expense>().AsReadOnly();
        }

        string jsonContent = File.ReadAllText(_filePath);
        if (string.IsNullOrWhiteSpace(jsonContent))
        {
            return new List<Expense>().AsReadOnly();
        }

        try
        {
            var list = JsonSerializer.Deserialize<List<Expense>>(jsonContent, _jsonOptions);
            return list?.AsReadOnly() ?? new List<Expense>().AsReadOnly();
        }
        catch (JsonException)
        {
            return new List<Expense>().AsReadOnly();
        }
    }

    public void Save(IEnumerable<Expense> expenses)
    {
        ArgumentNullException.ThrowIfNull(expenses);

        string jsonString = JsonSerializer.Serialize(expenses, _jsonOptions);
        File.WriteAllText(_filePath, jsonString);
    }
}