using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.DataModel
{
    [Serializable]
    public class Layer
    {
        public string Name { get; set; }
        public string CustomName { get; set; }
        public string Type { get; set; }
        public bool IsComplete { get; set; }
        public string Comment { get; set; }
        public List<List<Node>> Nodes { get; set; } = new List<List<Node>>();
        public List<Connection> Connections { get; set; } = new List<Connection>();
        public List<int> Routes { get; set; } = new List<int>();
    }
}
