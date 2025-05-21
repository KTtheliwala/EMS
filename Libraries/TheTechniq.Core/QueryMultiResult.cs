using System.Collections.Generic;

namespace TheTecniQ.Core;

/// <summary>
/// Represents the base class for Query multi result
/// </summary>
public abstract partial class QueryMultiResult<T1, T2>
{
    public List<T1> Results { get; set; }
    public T2 TotalCount { get; set; }
}