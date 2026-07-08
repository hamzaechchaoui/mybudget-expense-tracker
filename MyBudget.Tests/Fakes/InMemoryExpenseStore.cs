using MyBudget.Core;

namespace MyBudget.Tests.Fakes;

/// <summary>
/// A hand-written test double for <see cref="IExpenseStore"/>. We write our own
/// fake rather than use a mocking framework, which is deferred to the advanced
/// course (Module 9 note).
/// </summary>
public sealed class InMemoryExpenseStore : IExpenseStore
{
    private List<Expense> _saved;

    public int SaveCount { get; private set; }

    public InMemoryExpenseStore(params Expense[] seed) => _saved = seed.ToList();

    public IReadOnlyList<Expense> Load() => _saved;

    public void Save(IEnumerable<Expense> expenses)
    {
        _saved = expenses.ToList();
        SaveCount++;
    }
}
