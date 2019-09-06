using Excel= Microsoft.Office.Interop.Excel;
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
        string wcName = AppDomain.CurrentDomain.BaseDirectory + "words.txt";
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
                Excel.Workbook workbook = application.Workbooks.Open(f.FullName);
                workbook.SheetBeforeDoubleClick += Workbook_SheetBeforeDoubleClick;
                application.Visible = true;
            }
        }

        private void Workbook_SheetBeforeDoubleClick(object Sh, Excel.Range Target, ref bool Cancel)
        {
            Cancel = true;
            string pickValue = YDDicHelper.Create().TransText(Target.Value);
            Target.Next.Value= pickValue;
            //Excel.Range dcolumn = Target.Range["D1"];
            //dcolumn.Value2 = "中文解释";
            //Target.EntireColumn.Insert(Excel.XlInsertShiftDirection.xlShiftToRight,
            //        Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
            // this.Invoke((EventHandler)delegate { System.IO.File.AppendAllText(wcName, pickValue + Environment.NewLine); });
        }
    }
}
