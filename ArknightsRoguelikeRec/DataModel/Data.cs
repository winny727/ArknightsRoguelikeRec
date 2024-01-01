using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.DataModel
{
    [Serializable]
    public class Data
    {
        public string UserName { get; set; }
        public int DataID { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public List<Layer> Layers { get; set; } = new List<Layer>();
    }
}
