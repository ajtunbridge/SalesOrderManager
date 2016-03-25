using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SalesOrderManager.Properties;

namespace SalesOrderManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            Icon = Resources.ApplicationIcon;

            InitializeComponent();
        }
    }
}
