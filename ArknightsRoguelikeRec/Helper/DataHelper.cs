using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.Config;

namespace ArknightsRoguelikeRec.Helper
{
    public static class DataHelper
    {
        public static SaveData CreateData(string userName, string dataID)
        {
            SaveData saveData = new SaveData()
            {
                UserName = userName,
                DataID = dataID,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Version = GlobalDefine.VERSION,
            };

            //预设，自动添加一层到五层
            var layerList = DefineConfig.LayerConfigDict.AsList();
            for (int i = 0; i < GlobalDefine.PRESET_LAYER_COUNT; i++)
            {
                DataAPI.AddLayer(saveData, layerList[i].Name);
            }

            return saveData;
        }

        public static SaveData LoadData(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                string fileText = File.ReadAllText(path);
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(fileText);
                return saveData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public static string SaveData(SaveData saveData, string path, Formatting formatting = Formatting.Indented)
        {
            if (saveData == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            saveData.UpdateTime = DateTime.Now;
            saveData.Version = GlobalDefine.VERSION;
            string fileText = JsonConvert.SerializeObject(saveData, formatting);
            string fileName = $"{saveData.UserName}_data{saveData.DataID}_{saveData.CreateTime:yyyy-MM-dd_HH-mm-ss}.json";
            string fullPath = Path.Combine(path, fileName);
            File.WriteAllText(fullPath, fileText);
            return fullPath;
        }
    }
}
