using MyBudget.Core;

namespace MyBudget.App;

/// <summary>
/// The interactive console UI. Depends only on the service abstractions, which
/// are supplied by the DI container (constructor injection via a primary constructor).
/// </summary>
public sealed class ConsoleApp(IExpenseRepository repository, IBudgetService budget)
{
    public void Run()
    {
        Console.WriteLine("""
            ============================================================
              MyBudget — Assignment 2
            ============================================================
            """);

        bool running = true;
        while (running)
        {
            Console.WriteLine("""

                1) Add expense   2) List   3) Report   4) Set budget   5) Save & exit
                """);
            switch (Console.ReadLine()?.Trim())
            {
                case "1": AddExpense(); break;
                case "2": ListExpenses(); break;
                case "3": ShowReport(); break;
                case "4": SetBudget(); break;
                case "5": repository.Save(); running = false; Console.WriteLine("Saved. Goodbye!"); break;
                default: Console.WriteLine("Unknown option."); break;
            }
        }
    }

    private void AddExpense()
    {
        try
        {
            string description = ReadNonEmpty("Description : ");
            decimal amount = ReadDecimal("Amount      : ");
            ExpenseCategory category = ReadCategory();
            DateOnly date = ReadDate();

            Console.Write("Recurring? (y/N): ");
            bool recurring = (Console.ReadLine()?.Trim().ToLowerInvariant()) is "y" or "yes";

            Expense expense = recurring
                ? ExpenseFactory.CreateRecurring(description, amount, category, date, ReadInt("Times per month: "))
                : ExpenseFactory.CreateOneTime(description, amount, category, date);

            repository.Add(expense);
            Console.WriteLine($"Added: {expense.ToReportLine()}");
        }
        catch (InvalidExpenseException ex)
        {
            Console.WriteLine($"Rejected: {ex.Message}");
        }
    }

    private void ListExpenses()
    {
        IReadOnlyList<Expense> all = repository.GetAll();
        if (all.Count == 0)
        {
            Console.WriteLine("No expenses yet.");
            return;
        }

        foreach (Expense e in all)
            Console.WriteLine("  " + e.ToReportLine());
    }

    private void ShowReport()
    {
        decimal total = repository.Total();
        Console.WriteLine($"\nMonthly impact total: ${total:0.00}");

        Console.WriteLine("By category:");
        foreach ((ExpenseCategory category, decimal amount) in repository.TotalsByCategory())
            Console.WriteLine($"  {category,-13} ${amount:0.00}");

        BudgetStatus status = budget.Evaluate(total);
        if (status == BudgetStatus.NotSet)
        {
            Console.WriteLine("Budget: not set (use option 4).");
            return;
        }

        string label = status switch
        {
            BudgetStatus.OverBudget => "OVER BUDGET",
            BudgetStatus.AlmostOut => "Almost out",
            _ => "On track",
        };
        Console.WriteLine($"Budget: ${budget.Remaining(total):0.00} remaining of ${budget.MonthlyLimit:0.00} -> {label}");
    }

    private void SetBudget()
    {
        try
        {
            budget.SetMonthlyLimit(ReadDecimal("Monthly budget: "));
            Console.WriteLine($"Budget set to ${budget.MonthlyLimit:0.00}.");
        }
        catch (InvalidExpenseException ex)
        {
            Console.WriteLine($"Rejected: {ex.Message}");
        }
    }

    // ----- input helpers -------------------------------------------------

    private static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();
            Console.WriteLine("  Cannot be empty.");
        }
    }

    private static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out decimal value))
                return value;
            Console.WriteLine("  Enter a number, e.g. 12.50.");
        }
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value))
                return value;
            Console.WriteLine("  Enter a whole number.");
        }
    }

    private static ExpenseCategory ReadCategory()
    {
        while (true)
        {
            Console.Write("Category    : [Food/Transport/Utilities/Entertainment/Other] ");
            if (Enum.TryParse(Console.ReadLine()?.Trim(), ignoreCase: true, out ExpenseCategory category)
                && Enum.IsDefined(category))
                return category;
            Console.WriteLine("  Unknown category.");
        }
    }

    private static DateOnly ReadDate()
    {
        while (true)
        {
            Console.Write("Date (yyyy-mm-dd, blank = today): ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return DateOnly.FromDateTime(DateTime.Today);
            if (DateOnly.TryParse(input, out DateOnly date))
                return date;
            Console.WriteLine("  Enter a valid date (yyyy-mm-dd).");
        }
    }
}
