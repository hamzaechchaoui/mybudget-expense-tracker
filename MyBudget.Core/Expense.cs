using System;
using System.Text.Json.Serialization;

namespace MyBudget.Core;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(OneTimeExpense), "onetime")]
[JsonDerivedType(typeof(RecurringExpense), "recurring")]
public abstract record Expense(
    Guid Id, 
    string Description, 
    decimal Amount, 
    ExpenseCategory Category, 
    DateOnly Date
) : IReportable
{
    public abstract decimal MonthlyImpact { get; }

    public virtual string ToReportLine()
    {
        return $"{Date:yyyy-MM-dd} | {Category,-13} | {Description,-20} | {Amount,8:C}";
    }
}

public record OneTimeExpense(
    Guid Id, 
    string Description, 
    decimal Amount, 
    ExpenseCategory Category, 
    DateOnly Date
) : Expense(Id, Description, Amount, Category, Date)
{
    public override decimal MonthlyImpact => Amount;
}

public record RecurringExpense(
    Guid Id, 
    string Description, 
    decimal Amount, 
    ExpenseCategory Category, 
    DateOnly Date, 
    int TimesPerMonth
) : Expense(Id, Description, Amount, Category, Date)
{
    public override decimal MonthlyImpact => Amount * TimesPerMonth;

    public override string ToReportLine()
    {
        return $"{base.ToReportLine()} (x{TimesPerMonth}/month)";
    }
}