using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ArknightsRoguelikeRec
{
    public partial class InputForm : Form
    {
        public string Title
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

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
