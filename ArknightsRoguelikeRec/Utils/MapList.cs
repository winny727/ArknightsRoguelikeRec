using System;
using System.Collections.Generic;

public class MapList<TKey, TValue>
{
    private Dictionary<TKey, TValue> m_map = new Dictionary<TKey, TValue>();
    private List<TValue> m_list = new List<TValue>();
    private List<TKey> m_keyList = new List<TKey>();


    public List<TValue> AsList()
    {
        return m_list;
    }

    public Dictionary<TKey, TValue> AsDictionary()
    {
        return m_map;
    }

    public List<TKey> Keys { get { return m_keyList; } }

    public TValue[] ToArray()
    {
        return m_list.ToArray();
    }

    public TValue this[TKey indexKey]
    {
        get
        {
            TValue value;
            m_map.TryGetValue(indexKey, out value);
            return value;
        }
        set
        {
            if (m_map.ContainsKey(indexKey))
            {
                TValue v = m_map[indexKey];
                m_map[indexKey] = value;
                int keyIndex = m_keyList.IndexOf(indexKey);
                if (keyIndex != -1)
                {
                    m_list[keyIndex] = value;
                }
            }
            else
            {
                m_map.Add(indexKey, value);
                m_keyList.Add(indexKey);
                m_list.Add(value);
            }

        }
    }

    public bool Add(TKey key, TValue value)
    {
        if (m_map.ContainsKey(key))
        {
            return false;
        }
        m_map.Add(key, value);
        m_keyList.Add(key);
        m_list.Add(value);
        return true;
    }

    public bool Remove(TKey key)
    {
        if (m_map.ContainsKey(key))
        {
            TValue v = m_map[key];
            int keyIndex = m_keyList.IndexOf(key);
            if (keyIndex != -1)
            {
                m_keyList.RemoveAt(keyIndex);
                m_list.RemoveAt(keyIndex);
            }
            return m_map.Remove(key);
        }
        return false;
    }


    public void Clear()
    {
        m_map.Clear();
        m_keyList.Clear();
        m_list.Clear();
    }

    public int Count
    {
        get { return m_keyList.Count; }
    }

    public bool ContainsKey(TKey key)
    {
        return m_map.ContainsKey(key);
    }


}
