using System;
using System.Collections.Generic;

[Serializable]
public class Item
{
    public string Key { get; set; }
    public string Value { get; set; }

    public Item()
    {

    }

    public Item(string key, string value)
    {
        Key = key;
        Value = value;
    }
}
