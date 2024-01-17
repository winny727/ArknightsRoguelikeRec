using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ArknightsRoguelikeRec
{
    public partial class InputForm : Form
    {
        public string Content {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        public InputForm()
        {
            InitializeComponent();
        }
    }
}
