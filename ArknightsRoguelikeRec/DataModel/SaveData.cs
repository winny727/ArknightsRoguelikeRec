using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.DataModel
{
    [Serializable]
    public class SaveData
    {
        public string UserName { get; set; }
        public string DataID { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public List<Layer> Layers { get; set; } = new List<Layer>();
    }
}
