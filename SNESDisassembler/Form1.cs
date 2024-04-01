using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SNESDisassembler
{
    public partial class Form1 : Form
    {
        public StringBuilder diss;

        public Form1( List<List<string>> diss )
        {
            InitializeComponent();

            SourceData.RowHeadersVisible = false;
            SourceData.GridColor = Color.White;
            
            SourceData.CellBorderStyle = DataGridViewCellBorderStyle.None;
            SourceData.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            SourceData.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            DataTable table = new DataTable();
            table.Columns.Add("Address");
            table.Columns.Add("Hex");
            table.Columns.Add("opcodes");
            table.Columns.Add("comment");


            List<string> line;
            for (int i = 0; i < 27000; i++)
            {
                line = diss[i];
                DataRow dr = table.NewRow();
                dr["Address"] = line[0];
                dr["Hex"] = line[1];
                dr["opcodes"] = diss[i][2] + "    " + diss[i][3];
                dr["comment"] = "";
                table.Rows.Add(dr);
            }

            SourceData.DataSource = table;
        }
    }
}