namespace MyBudget.Core;

public enum BudgetStatus
{
    NotSet,
    OnTrack,
    AlmostOut,
    OverBudget,
}

/// <summary>Tracks a monthly limit and evaluates spending against it (Module 8 service).</summary>
public interface IBudgetService
{
    decimal MonthlyLimit { get; }
    void SetMonthlyLimit(decimal limit);
    decimal Remaining(decimal totalSpent);
    BudgetStatus Evaluate(decimal totalSpent);
}
