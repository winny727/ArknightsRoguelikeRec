using System;
using System.Collections.Generic;
using ArknightsRoguelikeRec.Config;

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
                Console.WriteLine(ex.Message);
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
                    list.Add(default);
                    continue;
                }

                T value = values[i].ConvertTo<T>(default);
                list.Add(value);
            }

            return list;
        }

        public static LayerConfig GetLayerConfigByName(string layerName)
        {
            var layerList = DefineConfig.LayerConfigDict.AsList();
            for (int i = 0; i < layerList.Count; i++)
            {
                if (layerList[i].Name == layerName)
                {
                    return layerList[i];
                }
            }
            return null;
        }
    }
}
