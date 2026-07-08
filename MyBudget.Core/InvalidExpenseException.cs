namespace MyBudget.Core;

/// <summary>Thrown when expense or budget data fails validation (Module 4 carried forward).</summary>
public sealed class InvalidExpenseException(string message) : Exception(message);
