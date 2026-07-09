using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBudget.Core;

public class ExpenseRepository : IExpenseRepository
{
    private readonly List<Expense> _expenses;
    private readonly IExpenseStore _store;

    public ExpenseRepository(IExpenseStore store)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _expenses = _store.Load().ToList();
    }

    public IReadOnlyList<Expense> GetAll()
    {
        return _expenses.OrderBy(e => e.Date).ToList().AsReadOnly();
    }

    public void Add(Expense expense)
    {
        if (expense == null)
        {
            throw new ArgumentNullException(nameof(expense), "Cannot add a null expense.");
        }
        _expenses.Add(expense);
    }

    public decimal Total()
    {
        return _expenses.Sum(e => e.MonthlyImpact);
    }

    public IReadOnlyDictionary<ExpenseCategory, decimal> TotalsByCategory()
    {
        var summary = Enum.GetValues<ExpenseCategory>()
                          .ToDictionary(cat => cat, _ => 0m);

        var groups = _expenses.GroupBy(e => e.Category);
        foreach (var group in groups)
        {
            summary[group.Key] = group.Sum(e => e.MonthlyImpact);
        }

        return summary.AsReadOnly();
    }

    public IReadOnlyList<Expense> InCategory(ExpenseCategory category)
    {
        return _expenses.Where(e => e.Category == category).OrderBy(e => e.Date).ToList().AsReadOnly();
    }

    public void Save()
    {
        _store.Save(_expenses);
    }
}