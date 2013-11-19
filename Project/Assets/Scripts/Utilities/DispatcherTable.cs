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

public class DispatchTable<T, TResult> {
    private readonly Dictionary<T, Func<TResult>> table = new Dictionary<T, Func<TResult>>();

    public void AddAction(T key, Func<TResult> action) { table[key] = action; }

    public TResult Dispatch(T key) {
        Func<TResult> action;
        return table.TryGetValue(key, out action) ? table[key]() : default(TResult);
    }
}

public class DispatchTable<T, FArg, TResult> {
    private readonly Dictionary<T, Func<FArg, TResult>> table = new Dictionary<T, Func<FArg, TResult>>();

    public void AddAction(T key, Func<FArg, TResult> action) { table[key] = action; }

    public TResult Dispatch(T key, FArg arg) {
        Func<FArg, TResult> action;
        return table.TryGetValue(key, out action) ? table[key](arg) : default(TResult);
    }
}

public class DispatchTable<T, FArg1, FArg2, TResult> {
    public delegate TResult DispatchDel(FArg1 arg1, FArg2 arg2);

    private readonly Dictionary<T, DispatchDel> table = new Dictionary<T, DispatchDel>();

    public void AddAction(T key, DispatchDel action) { table[key] = action; }

    public TResult Dispatch(T key, FArg1 arg1, FArg2 arg2) {
        DispatchDel action;
        return table.TryGetValue(key, out action) ? table[key](arg1, arg2) : default(TResult);
    }
}

public class DispatchTable<T, FArg1, FArg2, FArg3, TResult> {
    public delegate TResult DispatchDel(FArg1 arg1, FArg2 arg2, FArg3 arg3);

    private readonly Dictionary<T, DispatchDel> table = new Dictionary<T, DispatchDel>();

    public void AddAction(T key, DispatchDel action) { table[key] = action; }

    public TResult Dispatch(T key, FArg1 arg1, FArg2 arg2, FArg3 arg3) {
        DispatchDel action;
        return table.TryGetValue(key, out action) ? table[key](arg1, arg2, arg3) : default(TResult);
    }
}