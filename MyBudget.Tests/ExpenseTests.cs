using MyBudget.Core;
using Xunit;

namespace MyBudget.Tests;

public class ExpenseTests
{
    [Fact]
    public void OneTimeExpense_MonthlyImpactEqualsAmount()
    {
        var o = new OneTimeExpense(Guid.NewGuid(), "Shoes", 80m, ExpenseCategory.Other, default);
        Assert.Equal(80m, o.MonthlyImpact);
    }

    [Fact]
    public void RecurringExpense_MonthlyImpactMultipliesByFrequency()
    {
        var r = new RecurringExpense(Guid.NewGuid(), "Transit", 50m, ExpenseCategory.Transport, default, 3);
        Assert.Equal(150m, r.MonthlyImpact);
    }

    [Fact]
    public void Records_UseValueEquality()
    {
        var id = Guid.NewGuid();
        var a = new OneTimeExpense(id, "X", 10m, ExpenseCategory.Food, default);
        var b = new OneTimeExpense(id, "X", 10m, ExpenseCategory.Food, default);
        Assert.Equal(a, b);
    }

    [Fact]
    public void RecurringExpense_ReportLineShowsFrequency()
    {
        var r = new RecurringExpense(Guid.NewGuid(), "Gym", 25m, ExpenseCategory.Other, default, 2);
        Assert.Contains("x2/month", r.ToReportLine());
    }

    [Fact]
    public void Expense_IsReportablePolymorphically()
    {
        IReportable e = new OneTimeExpense(Guid.NewGuid(), "Book", 15m, ExpenseCategory.Other, default);
        Assert.False(string.IsNullOrWhiteSpace(e.ToReportLine()));
    }
}
