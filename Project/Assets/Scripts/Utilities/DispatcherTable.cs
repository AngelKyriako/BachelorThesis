using System;
using System.Collections.Generic;

public class DispatchTable<T> {
    private readonly Dictionary<T, Action> table = new Dictionary<T, Action>();

    public void AddAction(T key, Action action) { table[key] = action; }

    public void Dispatch(T key) {
        Action action;
        if (table.TryGetValue(key, out action))
            table[key]();
        else
            throw new InvalidOperationException("Action not supported: " + key.ToString());
    }
}
