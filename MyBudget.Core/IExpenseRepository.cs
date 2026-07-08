namespace MyBudget.Core;

/// <summary>In-memory collection of expenses with aggregate queries (Module 6).</summary>
public interface IExpenseRepository
{
    IReadOnlyList<Expense> GetAll();
    void Add(Expense expense);
    decimal Total();
    IReadOnlyDictionary<ExpenseCategory, decimal> TotalsByCategory();
    IReadOnlyList<Expense> InCategory(ExpenseCategory category);
    void Save();
}
