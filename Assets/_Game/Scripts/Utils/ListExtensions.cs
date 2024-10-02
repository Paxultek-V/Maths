

using System.Collections.Generic;

public static class ListExtensions
{
    public static void MoveItemAtIndexToFront<T>(this List<T> list, int index)
    {
        T item = list[index];
        list.RemoveAt(index);
        list.Insert(0, item);
    }
    
    public static void MoveItemAtIndexToLast<T>(this List<T> list, int index)
    {
        T item = list[index];
        list.RemoveAt(index);
        list.Add(item);
    }
}