using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCLTemplateExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenCLPrograms oclp = new OpenCLPrograms();
            Values val = oclp.Sum();

            for(int i = 0; i < val.output.Length; i++)
            {
                textBox1.AppendText(string.Format(Environment.NewLine + "{0} + {1} = {2}", val.input1[i].ToString(), val.input2[i].ToString(), val.output[i].ToString()));
            }
        }
    }
}
