using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAndVSTO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            Microsoft.Office.Interop.Excel.Application  application = new Microsoft.Office.Interop.Excel.Application();

            FileInfo f = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles().FirstOrDefault(a => a.Extension == ".xlsx");

            if (f != null)
            {
                Workbook workbook = application.Workbooks.Open(f.FullName);
                workbook.SheetBeforeDoubleClick += Workbook_SheetBeforeDoubleClick;
                application.Visible = true;
            }
        }

        private void Workbook_SheetBeforeDoubleClick(object Sh, Range Target, ref bool Cancel)
        {
            foreach (Range r in Target.Cells)
            {
                string text = (string)r.Text;
                if (!String.IsNullOrEmpty(text))
                {
                    textBox1.Invoke(new System.Action(() => textBox1.AppendText(text)));
                }
            }
            
        }
    }
}
