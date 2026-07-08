using MyBudget.Core;
using MyBudget.Tests.Fakes;
using Xunit;

namespace MyBudget.Tests;

public class ExpenseRepositoryTests
{
    private static OneTimeExpense One(decimal amount, ExpenseCategory category) =>
        new(Guid.NewGuid(), "x", amount, category, default);

    [Fact]
    public void Add_ThenTotal_SumsMonthlyImpact()
    {
        var repo = new ExpenseRepository(new InMemoryExpenseStore());
        repo.Add(One(10m, ExpenseCategory.Food));
        repo.Add(One(20m, ExpenseCategory.Food));
        Assert.Equal(30m, repo.Total());
    }

    [Fact]
    public void Constructor_LoadsExistingExpensesFromStore()
    {
        var store = new InMemoryExpenseStore(
            One(5m, ExpenseCategory.Food),
            One(15m, ExpenseCategory.Transport));

        var repo = new ExpenseRepository(store);

        Assert.Equal(2, repo.GetAll().Count);
        Assert.Equal(20m, repo.Total());
    }

    [Fact]
    public void TotalsByCategory_GroupsAndSums()
    {
        var repo = new ExpenseRepository(new InMemoryExpenseStore());
        repo.Add(One(10m, ExpenseCategory.Food));
        repo.Add(One(5m, ExpenseCategory.Food));
        repo.Add(One(8m, ExpenseCategory.Transport));

        var totals = repo.TotalsByCategory();

        Assert.Equal(15m, totals[ExpenseCategory.Food]);
        Assert.Equal(8m, totals[ExpenseCategory.Transport]);
    }

    [Fact]
    public void InCategory_FiltersByCategory()
    {
        var repo = new ExpenseRepository(new InMemoryExpenseStore());
        repo.Add(One(10m, ExpenseCategory.Food));
        repo.Add(One(8m, ExpenseCategory.Transport));

        Assert.Single(repo.InCategory(ExpenseCategory.Food));
    }

    [Fact]
    public void Total_CountsRecurringMonthlyImpact()
    {
        var repo = new ExpenseRepository(new InMemoryExpenseStore());
        repo.Add(new RecurringExpense(Guid.NewGuid(), "bus", 10m, ExpenseCategory.Transport, default, 5));
        Assert.Equal(50m, repo.Total());
    }

    [Fact]
    public void Save_DelegatesToStore()
    {
        var store = new InMemoryExpenseStore();
        var repo = new ExpenseRepository(store);
        repo.Add(One(10m, ExpenseCategory.Food));

        repo.Save();

        Assert.Equal(1, store.SaveCount);
        Assert.Single(store.Load());
    }
}
