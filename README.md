# MyBudget — Assignment 2 (Provided Pack)

This pack gives you the **contract** and the **UI**, not the solution. Your task
is to design and build the classes and records yourself, following the
"Build specification" section of the assignment brief, until the whole solution
compiles and every provided test passes.

> Important: **this solution does NOT compile as delivered.** The console UI and
> the unit tests reference types you have not created yet. Creating those types
> correctly is the assignment.

## What is provided

```
MyBudget.sln
├─ MyBudget.Core/
│   ├─ ExpenseCategory.cs        enum (provided)
│   ├─ InvalidExpenseException.cs (provided)
│   ├─ IReportable.cs            interface (provided)
│   ├─ IExpenseStore.cs          interface (provided)
│   ├─ IExpenseRepository.cs     interface (provided)
│   └─ IBudgetService.cs         interface + BudgetStatus enum (provided)
├─ MyBudget.App/
│   ├─ ConsoleApp.cs             the menu UI (provided)
│   └─ Program.cs                DI wiring — YOU complete the TODO
├─ MyBudget.Data/                (empty — you add the JSON store)
└─ MyBudget.Tests/               provided xUnit suite + fake (DO NOT MODIFY)
```

## What you build (see the brief for exact signatures)

- `MyBudget.Core/Expense.cs` — the `Expense` record hierarchy (base + two derived types)
- `MyBudget.Core/ExpenseFactory.cs` — validation + creation
- `MyBudget.Core/BudgetService.cs` — implements `IBudgetService`
- `MyBudget.Core/ExpenseRepository.cs` — implements `IExpenseRepository`
- `MyBudget.Data/JsonExpenseStore.cs` — implements `IExpenseStore`
- `MyBudget.App/Program.cs` — register your services with the DI container
- A test class of **your own** (additional to the provided tests)

## Workflow

1. Read the "Build specification" in the assignment brief.
2. Create the types above with the exact names, namespaces, and signatures given.
3. Build the solution; fix compile errors until it builds.
4. Run the tests (Test Explorer or `dotnet test`) and make them all green.
5. Add your own tests, run the app (`dotnet run --project MyBudget.App`), and
   commit to GitHub as you go.

## Rules

- Do not modify the provided interfaces, the UI, or the test files.
- Match the names/signatures in the brief exactly — the provided UI and tests
  depend on them.
- No mocking frameworks; use the hand-written `InMemoryExpenseStore` fake pattern.
