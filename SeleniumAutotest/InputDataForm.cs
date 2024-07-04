using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeleniumAutotest
{
    public partial class InputDataForm : Form
    {
        public string Result {  get; set; }

        public InputDataForm(string parameterName)
        {
            InitializeComponent();
            Label1.Text = "Введите значение для параметра " + parameterName;
        }

        private void BuOk_Click(object sender, EventArgs e)
        {
            Result = TeValue.Text;
        }
    }
}
