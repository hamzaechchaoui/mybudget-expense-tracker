namespace MyBudget.Core;

/// <summary>Anything that can render itself as a single report line (Module 7).</summary>
public interface IReportable
{
    string ToReportLine();
}
