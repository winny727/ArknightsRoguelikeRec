using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.DataModel
{
    [Serializable]
    public class NodeData
    {
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Comment { get; set; }
    }

    [Serializable]
    public class Node
    {
        public NodeData Data { get; set; } = new NodeData();

        [JsonIgnore]
        public int RefreshIndex { get; set; } = -1;
        public List<NodeData> RefreshNodes { get; set; } = new List<NodeData>();
    }
}
