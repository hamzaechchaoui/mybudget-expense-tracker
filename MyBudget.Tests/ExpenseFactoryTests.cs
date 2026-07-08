using MyBudget.Core;
using Xunit;

namespace MyBudget.Tests;

public class ExpenseFactoryTests
{
    [Fact]
    public void CreateOneTime_SetsPropertiesAndId()
    {
        // Arrange / Act
        var e = ExpenseFactory.CreateOneTime("Coffee", 4.50m, ExpenseCategory.Food, new DateOnly(2026, 6, 1));

        // Assert
        Assert.Equal("Coffee", e.Description);
        Assert.Equal(4.50m, e.Amount);
        Assert.Equal(ExpenseCategory.Food, e.Category);
        Assert.Equal(4.50m, e.MonthlyImpact);
        Assert.NotEqual(Guid.Empty, e.Id);
    }

    [Fact]
    public void CreateOneTime_TrimsDescription()
    {
        var e = ExpenseFactory.CreateOneTime("  Lunch  ", 9m, ExpenseCategory.Food, default);
        Assert.Equal("Lunch", e.Description);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(2000000)]
    public void ValidateAmount_RejectsInvalid(decimal amount)
    {
        Assert.Throws<InvalidExpenseException>(() => ExpenseFactory.ValidateAmount(amount));
    }

    [Fact]
    public void ValidateAmount_RoundsToTwoDecimalPlaces()
    {
        Assert.Equal(11.00m, ExpenseFactory.ValidateAmount(10.999m));
    }

    [Fact]
    public void CreateOneTime_RejectsBlankDescription()
    {
        Assert.Throws<InvalidExpenseException>(
            () => ExpenseFactory.CreateOneTime("   ", 5m, ExpenseCategory.Other, default));
    }

    [Fact]
    public void CreateRecurring_RejectsFrequencyBelowOne()
    {
        Assert.Throws<InvalidExpenseException>(
            () => ExpenseFactory.CreateRecurring("Rent", 100m, ExpenseCategory.Utilities, default, 0));
    }

    [Fact]
    public void CreateRecurring_SetsFrequency()
    {
        var e = ExpenseFactory.CreateRecurring("Bus", 2.50m, ExpenseCategory.Transport, default, 20);
        Assert.Equal(20, e.TimesPerMonth);
        Assert.Equal(50.00m, e.MonthlyImpact);
    }
}
