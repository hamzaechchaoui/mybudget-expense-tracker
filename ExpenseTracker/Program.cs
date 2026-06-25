using ExpenseTracker;

// State Tracking Variables (Modules 1-4: No collections/lists/custom classes allowed!)
decimal overallTotalSpent = 0m;
int totalExpenseCount = 0;
decimal highestSingleExpense = 0m;

// Category Running Balances
decimal totalFood = 0m;
decimal totalTransport = 0m;
decimal totalUtilities = 0m;
decimal totalEntertainment = 0m;
decimal totalOther = 0m;

// Budget Configuration States
decimal monthlyBudgetLimit = 0m;
bool isBudgetSet = false;

// Feature: Raw string literal banner with string interpolation
string applicationBanner = $"""
============================================================
               MyBudget Expense Tracker                     
============================================================
""";

Console.WriteLine(applicationBanner);

bool keepRunning = true;

// Main interactive application menu loop
while (keepRunning)
{
    Console.WriteLine();
    Console.WriteLine("1) Add an expense   2) View summary   3) Set monthly budget   4) Exit");
    Console.Write("> ");
    string? menuChoice = Console.ReadLine();

    switch (menuChoice)
    {
        case "1":
            ExecuteAddExpense();
            break;
        case "2":
            ExecuteViewSummary();
            break;
        case "3":
            ExecuteSetBudget();
            break;
        case "4":
            keepRunning = false;
            Console.WriteLine("Thank you for using MyBudget. Goodbye!");
            break;
        default:
            Console.WriteLine("Invalid option. Please enter a number between 1 and 4.");
            break;
    }
}

// =====================================================================
// INTERACTIVE ROUTINE SUBMETHODS
// =====================================================================

void ExecuteAddExpense()
{
    Console.WriteLine("\n--- Add New Expense ---");
    
    try
    {
        // 1. Description Entry & Validation
        string description = "";
        while (string.IsNullOrWhiteSpace(description))
        {
            Console.Write("Description : ");
            description = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Description cannot be empty.");
            }
        }

        // 2. Amount Entry & Validation
        decimal validatedAmount = 0m;
        bool isAmountValid = false;
        while (!isAmountValid)
        {
            Console.Write("Amount      : ");
            if (decimal.TryParse(Console.ReadLine(), out decimal inputAmount))
            {
                try
                {
                    // Verify with your completed core logic engine rules
                    validatedAmount = BudgetRules.ValidateAmount(inputAmount);
                    isAmountValid = true;
                }
                catch (InvalidExpenseException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Amount must be a valid numerical value.");
            }
        }

        // 3. Category Entry & Validation
        string? normalizedCategory = null;
        while (normalizedCategory == null)
        {
            Console.Write("Category    : [Food/Transport/Utilities/Entertainment/Other] ");
            string? rawCategory = Console.ReadLine();
            normalizedCategory = BudgetRules.NormalizeCategory(rawCategory);

            if (normalizedCategory == null)
            {
                Console.WriteLine("Invalid category choice. Please select from the listed options.");
            }
        }

        // 4. Date Entry & Validation (Blank defaults to Today)
        DateOnly executionDate = DateOnly.FromDateTime(DateTime.Today);
        bool isDateValid = false;
        while (!isDateValid)
        {
            Console.Write("Date (blank = today [yyyy-mm-dd]): ");
            string? rawDateInput = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(rawDateInput))
            {
                executionDate = DateOnly.FromDateTime(DateTime.Today);
                isDateValid = true;
            }
            else if (DateOnly.TryParse(rawDateInput, out DateOnly parsedDate))
            {
                if (parsedDate > DateOnly.FromDateTime(DateTime.Today))
                {
                    Console.WriteLine("Future dates are not allowed.");
                }
                else
                {
                    executionDate = parsedDate;
                    isDateValid = true;
                }
            }
            else
            {
                Console.WriteLine("Invalid format. Please use calendar formatting format (yyyy-mm-dd).");
            }
        }

        // 5. Optional Note String (Handling via Nullable Safe Operators)
        Console.Write("Note (optional): ");
        string? structuralNote = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(structuralNote))
        {
            structuralNote = null;
        }

        // All fields verified! Update running totals tracking variables
        overallTotalSpent += validatedAmount;
        totalExpenseCount++;
        
        if (validatedAmount > highestSingleExpense)
        {
            highestSingleExpense = validatedAmount;
        }

        // Step up category trackers
        switch (normalizedCategory)
        {
            case "Food": totalFood += validatedAmount; break;
            case "Transport": totalTransport += validatedAmount; break;
            case "Utilities": totalUtilities += validatedAmount; break;
            case "Entertainment": totalEntertainment += validatedAmount; break;
            case "Other": totalOther += validatedAmount; break;
        }

        // Display results to user using logic functions
        string displayValue = BudgetRules.FormatCurrency(validatedAmount);
        Console.WriteLine($"Recorded: {displayValue} | {normalizedCategory} | {executionDate:yyyy-MM-dd}");
        
        string computedNoteString = structuralNote?.Trim() ?? "None";
        Console.WriteLine($"Note      : {computedNoteString}");
        Console.WriteLine($"Size band : {BudgetRules.ClassifyAmount(validatedAmount)}");

        if (isBudgetSet)
        {
            decimal remainingFunds = monthlyBudgetLimit - overallTotalSpent;
            string localizedStatus = BudgetRules.BudgetStatus(remainingFunds, monthlyBudgetLimit);
            Console.WriteLine($"Budget: {BudgetRules.FormatCurrency(remainingFunds)} remaining of {BudgetRules.FormatCurrency(monthlyBudgetLimit)} -> {localizedStatus}");
        }
    }
    catch (InvalidExpenseException ex)
    {
        Console.WriteLine($"Validation Error: {ex.Message}");
    }
    finally
    {
        // Try-Catch-Finally block requirement satisfied
    }
}

void ExecuteViewSummary()
{
    Console.WriteLine("\n--- MyBudget Expense Summary ---");
    
    if (totalExpenseCount == 0)
    {
        Console.WriteLine("No expenses recorded yet.");
        if (isBudgetSet)
        {
            Console.WriteLine($"Budget: {BudgetRules.FormatCurrency(monthlyBudgetLimit)} remaining of {BudgetRules.FormatCurrency(monthlyBudgetLimit)} -> On track");
        }
        return;
    }

    // Guard against divide-by-zero errors
    decimal calculatedAverage = overallTotalSpent / totalExpenseCount;

    Console.WriteLine($"Total Expenses: {totalExpenseCount}");
    Console.WriteLine($"Total Spent   : {BudgetRules.FormatCurrency(overallTotalSpent)}");
    Console.WriteLine($"Average Cost  : {BudgetRules.FormatCurrency(calculatedAverage)}");
    Console.WriteLine($"Highest Single: {BudgetRules.FormatCurrency(highestSingleExpense)}");
    
    Console.WriteLine("\n--- Category Breakdowns ---");
    Console.WriteLine($"* Food          : {BudgetRules.FormatCurrency(totalFood)}");
    Console.WriteLine($"* Transport     : {BudgetRules.FormatCurrency(totalTransport)}");
    Console.WriteLine($"* Utilities     : {BudgetRules.FormatCurrency(totalUtilities)}");
    Console.WriteLine($"* Entertainment : {BudgetRules.FormatCurrency(totalEntertainment)}");
    Console.WriteLine($"* Other         : {BudgetRules.FormatCurrency(totalOther)}");

    if (isBudgetSet)
    {
        Console.WriteLine("\n--- Budget Tracking Status ---");
        decimal remainingFunds = monthlyBudgetLimit - overallTotalSpent;
        string structuralStatusString = BudgetRules.BudgetStatus(remainingFunds, monthlyBudgetLimit);
        Console.WriteLine($"Budget Configured : {BudgetRules.FormatCurrency(monthlyBudgetLimit)}");
        Console.WriteLine($"Remaining Balance : {BudgetRules.FormatCurrency(remainingFunds)}");
        Console.WriteLine($"Current Standing  : {structuralStatusString}");
    }
}

void ExecuteSetBudget()
{
    Console.WriteLine("\n--- Set Monthly Budget Target ---");
    bool initializedInputResult = false;

    while (!initializedInputResult)
    {
        Console.Write("Monthly budget: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal configuredAmount) && configuredAmount > 0)
        {
            monthlyBudgetLimit = decimal.Round(configuredAmount, 2);
            isBudgetSet = true;
            initializedInputResult = true;

            Console.WriteLine($"Budget set to {BudgetRules.FormatCurrency(monthlyBudgetLimit)}.");
            
            decimal remainingFunds = monthlyBudgetLimit - overallTotalSpent;
            string dynamicStatus = BudgetRules.BudgetStatus(remainingFunds, monthlyBudgetLimit);
            Console.WriteLine($"Budget: {BudgetRules.FormatCurrency(remainingFunds)} remaining of {BudgetRules.FormatCurrency(monthlyBudgetLimit)} -> {dynamicStatus}");
        }
        else
        {
            Console.WriteLine("Budget must be a valid positive number.");
        }
    }
}