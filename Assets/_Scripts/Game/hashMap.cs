using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hashMap<Key, Value>
{
    public class Entry
    {
        public Key key;
        public Value value;

        public Entry(Key key, Value value)
        {
            this.key = key;
            this.value = value;
        }
    }

    public List<Entry> entries = new List<Entry>();

    public hashMap()
    {
        entries = new List<Entry>();
    }

    public void Add(Key key, Value value)
    {
        entries.Add(new Entry(key, value));
    }

    public void Replace(Key key, Value value)
    {

        foreach (Entry e in entries)
        {
            if (e.key.Equals(key))
            {
                e.value = value;
                return;
            }
        }

        Entry entry = new Entry(key, value);

        entries.Add(entry);
    }

    public Value Get(Key key)
    {
        foreach (Entry e in entries)
        {
            if (e.key.Equals(key))
                return e.value;
        }

        return default(Value);
    }

    public Key GetKey(Value value)
    {
        foreach (Entry e in entries)
        {
            if (e.value.Equals(value))
                return e.key;
        }
        return default(Key);
    }

    public List<Key> KeySet()
    {
        List<Key> keySet = new List<Key>();

        foreach (Entry e in entries)
        {
            keySet.Add(e.key);
        }

        return keySet;
    }

    public List<Value> ValueSet()
    {
        List<Value> valueSet = new List<Value>();

        foreach (Entry e in entries)
        {
            valueSet.Add(e.value);
        }

        return valueSet;
    }

    public List<Entry> EntrySet()
    {
        return this.entries;
    }

    public int Size()
    {
        return this.entries.Count;
    }

    public bool IsEmpty()
    {
        if (this.entries.Count == 0 || this.entries.Equals(null))
            return true;

        return false;
    }

    public void Remove(Key key)
    {
        foreach (Entry e in entries)
        {
            if (e.key.Equals(key))
                entries.Remove(e);
        }
    }
}
