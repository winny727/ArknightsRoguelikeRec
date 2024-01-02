using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.DataModel
{
    [Serializable]
    public class Node
    {
        public string Type { get; set; }
        public string SubType { get; set; }
        public int ExtraLayer { get; set; }
    }
}
