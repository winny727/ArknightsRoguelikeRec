using System;
using System.Collections.Generic;

[Serializable]
public class MenuItem
{
    public string Key { get; set; }
    public string Value { get; set; }

    public MenuItem()
    {

    }

    public MenuItem(string key, string value)
    {
        Key = key;
        Value = value;
    }
}
