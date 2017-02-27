using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Application = System.Windows.Forms.Application;

namespace ArchivplanExcelImporterWindowsFormsApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = textBox1.Text;
            if (!File.Exists(path))
            {
                MessageBox.Show("Datei nicht gefunden");
                return;
            }
            var app = new Microsoft.Office.Interop.Excel.Application();
            if (app == null)
            {
                Console.WriteLine(
                    "EXCEL could not be started. Check that your office installation and project references are correct.");
                return;
            }
            //app.Visible = true;

            //var wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            var wb = app.Workbooks.Open(path);
            var ws = (Worksheet)wb.Worksheets[1];
            if (ws == null)
            {
                Console.WriteLine(
                    "Worksheet could not be created. Check that your office installation and project references are correct.");
            }

            // Struktur
            //Col 1 = A1 = ID
            //Col 2 = B1 = DE_Nummer   
            //Col 3 = C1 = Parent 
            //Col 4 = D1 = DE_Begriff      
            //Col 5 = E1 = DE_Dossierbildung 
            //Col 6 = F1 = DE_Aufbewahrungsfrist  
            //Col 7 = G1 = DE_Untertitel(Stufe3)  
            //Col 8 = H1 = DE_Haupttitel(Stufe2)  
            //Col 9 = I1 = Hinweis_DE 
            //Col10 = J1 = 
            //Col11 = K1 = 
            //Col12 = L1 = FR Nummer 
            //Col13 = M1 = FR Begriff 
            //Col14 = N1 = FR_Dossierbildung  
            //Col15 = O1 = FR_Aufbewahrungsfrist 
            //Col16 = P1 = FR_Untertitel (Stufe3)
            //Col17 = Q1 = FR_Haupttitel(Stufe2)  
            //Col18 = R1 = Hinweis_FR

            //A2           1 ...
            //A3           2 ...
            //...
            //A1361     1360 ...
            //A1362  

            var dicNr2Id = new Dictionary<string, int>();
            var list = new List<ArchivplanRow>();
            var i = 2;
            while (true)
            {
                var aRange = ws.Cells[i, 1];
                if (aRange == null)
                {
                    Console.WriteLine(
                        "Could not get a range. Check to be sure you have the correct versions of the office DLLs.");
                }
                if (aRange.Value2 == null)
                {
                    break;
                }
                var row = new ArchivplanRow();
                row.ID = int.Parse(ws.CellText(i, 1));
                row.DE_Nummer = ws.CellText(i, 2);
                row.Parent = ws.CellText(i, 3);
                row.DE_Begriff = ws.CellText(i, 4);
                row.DE_Dossierbildung = ws.CellText(i, 5);
                row.DE_Aufbewahrungsfrist = ws.CellText(i, 6);
                row.DE_UntertitelStufe3 = ws.CellText(i, 7);
                row.DE_HaupttitelStufe2 = ws.CellText(i, 8);
                row.Hinweis_DE = ws.CellText(i, 9);
                row.FR_Nummer = ws.CellText(i, 12);
                row.FR_Begriff = ws.CellText(i, 13);
                row.FR_Dossierbildung = ws.CellText(i, 14);
                row.FR_Aufbewahrungsfrist = ws.CellText(i, 15);
                row.FR_UntertitelStufe3 = ws.CellText(i, 16);
                row.FR_HaupttitelStufe2 = ws.CellText(i, 17);
                row.Hinweis_FR = ws.CellText(i, 18);
                dicNr2Id.Add(row.DE_Nummer, row.ID);
                list.Add(row);
                i++;
                if (i%100 == 0)
                {
                    label1.Text = i.ToString();
                    Application.DoEvents();
                }
            }
            //// Fill the cells in the C1 to C7 range of the worksheet with the number 6.
            //var args = new Object[1];
            //args[0] = 6;
            //aRange.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, aRange, args);
            ReleaseCom(app);

            //// Change the cells in the C1 to C7 range of the worksheet to the number 8.
            //aRange.Value2 = 8;
            var entities = new ArchivplanEntities();
            foreach (var row in list)
            {
                var item = new tbArchivplan();
                item.AufbewahrungsfristDE = row.DE_Aufbewahrungsfrist;
                item.AufbewahrungsfristFR = row.FR_Aufbewahrungsfrist;
                item.BezeichnungDE = row.DE_Begriff;
                item.BezeichnungFR = row.FR_Begriff;
                item.DossierbildungDE = row.DE_Dossierbildung;
                item.DossierbildungFR = row.FR_Dossierbildung;
                item.HaupttitelDE = row.DE_HaupttitelStufe2;
                item.HaupttitelFR = row.FR_HaupttitelStufe2;
                item.HinweisDE = row.Hinweis_DE;
                item.HinweisFR = row.Hinweis_FR;
                item.ParentId = string.IsNullOrEmpty(row.Parent) ? 0 : (int)dicNr2Id[row.Parent];
                item.RegPlanId = row.ID;
                item.RegPlanNr = row.DE_Nummer;
                item.UntertitelDE = row.DE_UntertitelStufe3;
                item.UntertitelFR = row.FR_UntertitelStufe3;
                entities.tbArchivplans.Add(item);
            }
            entities.SaveChanges();
        }

        private void ReleaseCom(Microsoft.Office.Interop.Excel.Application app)
        {
            if (app.Workbooks != null)
            {
                foreach (Microsoft.Office.Interop.Excel.Workbook wb in app.Workbooks)
                {
                    foreach (Microsoft.Office.Interop.Excel.Worksheet ws in wb.Worksheets)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
                    }
                    wb.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                }
                app.Workbooks.Close();
            }
            app.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            app = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ofd.Filter = "Excel Dateien|*.xls";
            var dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }
    }

    public static class Extensions
    {
        public static string CellText(this Worksheet ws, int row, int col)
        {
            var rg = ws.Cells[row, col];

            if (rg == null || rg.Value2 == null)
            {
                return string.Empty;
            }
            return rg.Value2.ToString();
        }
    }
}

