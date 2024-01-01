using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ArknightsRoguelikeRec.Config;

namespace ArknightsRoguelikeRec
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DefineConfig.InitConfig();

            //测试用
            string layerConfig = Newtonsoft.Json.JsonConvert.SerializeObject(DefineConfig.LayerConfigDict.AsList(), Newtonsoft.Json.Formatting.Indented);
            string nodeConfig = Newtonsoft.Json.JsonConvert.SerializeObject(DefineConfig.NodeConfigDict.AsList(), Newtonsoft.Json.Formatting.Indented);

            Console.WriteLine(layerConfig);
            Console.WriteLine(nodeConfig);
        }
    }
}
