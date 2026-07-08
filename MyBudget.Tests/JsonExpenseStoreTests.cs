using MyBudget.Core;
using MyBudget.Data;
using Xunit;

namespace MyBudget.Tests;

public class JsonExpenseStoreTests
{
    [Fact]
    public void Load_ReturnsEmpty_WhenFileMissing()
    {
        string path = Path.Combine(Path.GetTempPath(), $"missing_{Guid.NewGuid():N}.json");
        var store = new JsonExpenseStore(path);
        Assert.Empty(store.Load());
    }

    [Fact]
    public void SaveThenLoad_RoundTripsDerivedTypes()
    {
        string path = Path.Combine(Path.GetTempPath(), $"mybudget_{Guid.NewGuid():N}.json");
        try
        {
            var store = new JsonExpenseStore(path);
            Expense one = new OneTimeExpense(Guid.NewGuid(), "Coffee", 4.5m, ExpenseCategory.Food, new DateOnly(2026, 6, 1));
            Expense rec = new RecurringExpense(Guid.NewGuid(), "Bus", 2m, ExpenseCategory.Transport, new DateOnly(2026, 6, 2), 20);

            store.Save([one, rec]);
            IReadOnlyList<Expense> loaded = store.Load();

            Assert.Equal(2, loaded.Count);
            Assert.Contains(loaded, e => e is OneTimeExpense && e.Description == "Coffee");
            Assert.Contains(loaded, e => e is RecurringExpense r && r.TimesPerMonth == 20);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
