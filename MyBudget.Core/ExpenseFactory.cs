using System;

namespace MyBudget.Core;

public static class ExpenseFactory
{
    private const decimal MaxAmount = 1_000_000m;

   
    public static decimal ValidateAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidExpenseException("Amount must be greater than zero.");
        }
        if (amount > MaxAmount)
        {
            throw new InvalidExpenseException("Amount cannot exceed the realistic limit of 1,000,000.");
        }

        return decimal.Round(amount, 2);
    }

  
    public static OneTimeExpense CreateOneTime(string description, decimal amount, ExpenseCategory category, DateOnly date)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidExpenseException("Description cannot be blank.");
        }

        decimal validatedAmount = ValidateAmount(amount);
        return new OneTimeExpense(Guid.NewGuid(), description.Trim(), validatedAmount, category, date);
    }


    public static RecurringExpense CreateRecurring(string description, decimal amount, ExpenseCategory category, DateOnly date, int timesPerMonth)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidExpenseException("Description cannot be blank.");
        }
        if (timesPerMonth < 1)
        {
            throw new InvalidExpenseException("Times per month must be at least 1.");
        }

        decimal validatedAmount = ValidateAmount(amount);
        return new RecurringExpense(Guid.NewGuid(), description.Trim(), validatedAmount, category, date, timesPerMonth);
    }
}