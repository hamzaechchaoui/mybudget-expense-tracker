using System;

namespace MyBudget.Core;

public class BudgetService : IBudgetService
{
    public decimal MonthlyLimit { get; private set; }

    public void SetMonthlyLimit(decimal limit)
    {
        if (limit <= 0)
        {
            throw new InvalidExpenseException("Monthly budget limit must be greater than zero.");
        }
        MonthlyLimit = decimal.Round(limit, 2);
    }

 
    public decimal Remaining(decimal totalSpent)
    {
        return MonthlyLimit - totalSpent;
    }

 
    public BudgetStatus Evaluate(decimal totalSpent)
    {
        if (MonthlyLimit == 0)
        {
            return BudgetStatus.NotSet;
        }

        decimal remainingFunds = Remaining(totalSpent);

        if (remainingFunds < 0)
        {
            return BudgetStatus.OverBudget;
        }

        if (remainingFunds < (MonthlyLimit * 0.10m))
        {
            return BudgetStatus.AlmostOut;
        }

        return BudgetStatus.OnTrack;
    }
}