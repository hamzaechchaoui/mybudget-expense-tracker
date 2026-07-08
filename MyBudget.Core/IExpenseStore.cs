namespace MyBudget.Core;

/// <summary>
/// Persistence boundary for expenses (Module 8: Dependency Inversion).
/// The repository depends on this abstraction, not on a concrete file/JSON class.
/// </summary>
public interface IExpenseStore
{
    IReadOnlyList<Expense> Load();
    void Save(IEnumerable<Expense> expenses);
}
