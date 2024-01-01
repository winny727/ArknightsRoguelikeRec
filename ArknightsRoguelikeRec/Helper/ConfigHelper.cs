using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.Helper
{
    public static class ConfigHelper
    {
        public static TableReader LoadConfig(string path)
        {
            try
            {
                TableReader tableReader = new TableReader(path);
                return tableReader;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public static List<T> ParseList<T>(string text, char split = '|')
        {
            List<T> list = new List<T>();
            if (string.IsNullOrEmpty(text))
            {
                return list;
            }

            string[] values = text.Split(split);
            for (int i = 0; i < values.Length; i++)
            {
                if (string.IsNullOrEmpty(values[i]))
                {
                    continue;
                }

                T value = values[i].ConvertTo<T>(default);
                list.Add(value);
            }

            return list;
        }
    }
}
