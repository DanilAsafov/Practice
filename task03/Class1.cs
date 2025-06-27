using System;
using System.Collections;

namespace task03;

public class CustomCollection<T> : IEnumerable<T> {
    private readonly List<T> _items = new();

    public void Add(T item) => _items.Add(item);
    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<T> GetReverseEnumerator() {
        foreach (var item in Enumerable.Range(0, _items.Count))
            yield return _items[_items.Count - 1 - item];
    }

    public static IEnumerable<int> GenerateSequence(int start, int count) {
        foreach (var i in Enumerable.Range(0, count))
            yield return start + i;
    }

    public IEnumerable<T> FilterAndSort(Func<T, bool> predicate, Func<T, IComparable> keySelector) {
        return _items.Where(predicate).OrderBy(keySelector);
    }
}
