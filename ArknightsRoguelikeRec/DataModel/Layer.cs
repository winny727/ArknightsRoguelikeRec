using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.DataModel
{
    [Serializable]
    public class Layer
    {
        public string Name { get; set; }
        public List<List<Node>> Nodes { get; set;} = new List<List<Node>>();
        public List<Connection> Connections { get; set; } = new List<Connection>();
    }
}
