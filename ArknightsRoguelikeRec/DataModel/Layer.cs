using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.DataModel
{
    [Serializable]
    public class Layer
    {
        public string Name { get; set; }
        public List<List<Node>> Nodes { get; set;} = new List<List<Node>>();
        public List<KeyValuePair<int, int>> Connections { get; set; } = new List<KeyValuePair<int, int>>();
    }
}
