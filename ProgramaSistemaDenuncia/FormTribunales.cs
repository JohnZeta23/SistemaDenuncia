using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramaSistemaDenuncia
{
    public partial class FormTribunales : Form
    {
        public FormTribunales()
        {
            InitializeComponent();
            FillComboBox();
        }

        private static string stringConnection = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        private void ButtonAceptar_Click(object sender, EventArgs e)
        {   
            SQLiteConnection conexion = new SQLiteConnection(stringConnection);

            try
            {
                conexion.Open();
                SQLiteCommand sqliteCommand = new SQLiteCommand("update Tribunales set Tribunal = @Tribunal " +
                    "where ID_Tribunal = @ID_Tribunal", conexion);
                sqliteCommand.Parameters.AddWithValue("@Tribunal", textBoxTribunal.Text);
                sqliteCommand.Parameters.AddWithValue("@ID_Tribunal", comboBoxTribunal.SelectedIndex);
                sqliteCommand.CommandType = CommandType.Text;
                
                if (sqliteCommand.ExecuteNonQuery() > 0)
                {
                    conexion.Close();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Text, ex.Message);
            }
        }


        private void FillComboBox()
        {
            SQLiteConnection conexion = new SQLiteConnection(stringConnection);

            try
            {
                conexion.Open();
                SQLiteCommand sqliteCommand = new SQLiteCommand("select * from Tribunales", conexion);
                SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(sqliteCommand);
                sqliteCommand.CommandType = CommandType.Text;

                DataTable dataTable = new DataTable();

                sQLiteDataAdapter.Fill(dataTable);

                DataRow DataRow = dataTable.NewRow();
                DataRow["Tribunal"] = "Selecciona un Tribunal";
                dataTable.Rows.InsertAt(DataRow, 0);

                comboBoxTribunal.ValueMember = "ID_Tribunal";
                comboBoxTribunal.DisplayMember = "Tribunal";
                comboBoxTribunal.DataSource = dataTable;

            }

            catch (Exception ex)
            {
                MessageBox.Show(Text, ex.Message);
            }

            finally
            {
                conexion.Close();
            }
        }

        private void comboBoxTribunal_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxTribunal.Text = comboBoxTribunal.Text;
        }
    }
}
