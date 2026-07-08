using MyBudget.Core;
using Xunit;

namespace MyBudget.Tests;

public class BudgetServiceTests
{
    [Fact]
    public void Evaluate_ReturnsNotSet_WhenNoLimit()
    {
        var svc = new BudgetService();
        Assert.Equal(BudgetStatus.NotSet, svc.Evaluate(100m));
    }

    [Theory]
    [InlineData(600, BudgetStatus.OverBudget)]   // remaining -100
    [InlineData(460, BudgetStatus.AlmostOut)]    // remaining 40  (< 10% of 500)
    [InlineData(450, BudgetStatus.OnTrack)]      // remaining 50
    [InlineData(0, BudgetStatus.OnTrack)]
    public void Evaluate_ClassifiesSpending(decimal spent, BudgetStatus expected)
    {
        var svc = new BudgetService();
        svc.SetMonthlyLimit(500m);
        Assert.Equal(expected, svc.Evaluate(spent));
    }

    [Fact]
    public void SetMonthlyLimit_RejectsNonPositive()
    {
        var svc = new BudgetService();
        Assert.Throws<InvalidExpenseException>(() => svc.SetMonthlyLimit(0m));
    }

    [Fact]
    public void Remaining_IsLimitMinusSpent()
    {
        var svc = new BudgetService();
        svc.SetMonthlyLimit(500m);
        Assert.Equal(200m, svc.Remaining(300m));
    }
}
