using System;
using System.Collections.Generic;

class Nullerator<T> : IEnumerator<T>
    where T : class
{
    int i, next;
    List<T> list;

    public Nullerator(List<T> list)
    {
        i = -1;
        next = -1;
        this.list = list;
    }

    public T Current
    {
        get { return list[i]; }
    }

    object System.Collections.IEnumerator.Current
    {
        get { return Current; }
    }

    public bool MoveNext()
    {
        i++;
        FindNextNonNull();
        if (i >= list.Count) return false;
        else if (list[i] == null)
        {
            if (next < list.Count)
            {
                list[i] = list[next];
                list[next] = null;
                return true;
            }
            else
            {
                for (int j = list.Count - 1; j >= i; j--)
                    list.RemoveAt(j);
                return false;
            }
        }
        else return true;
    }

    private void FindNextNonNull()
    {
        if (next >= list.Count) return;
        do
        {
            next++;
        } while (next < list.Count && list[next] == null);
    }

    public void Nullify()
    {
        list[i] = null;
        i--;
    }

    public void Reset()
    {
        i = -1;
        next = -1;
    }

    public void Dispose()
    { }
}