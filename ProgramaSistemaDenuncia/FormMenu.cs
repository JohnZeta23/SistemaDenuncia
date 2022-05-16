using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramaSistemaDenuncia
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
            FillComboBox();
        }

        private static string stringConnection = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;
        string Fecha = "";

        private void timerHora_Tick(object sender, EventArgs e)
        {
                labelHora.Text = DateTime.Now.ToLongDateString();       
        }

        private void buttonAgregarDenuncia_Click(object sender, EventArgs e)
        {
            if (textBoxCedulaDenunciado.Text.Trim() == "" || textBoxCedulaDenunciante.Text.Trim() == ""
                || textBoxDescripcionDenuncia.Text.Trim() == "" || textBoxNombreDenunciado.Text.Trim() == ""
                || textBoxNombreDenunciante.Text.Trim() == "" || textBoxTituloDenuncia.Text.Trim() == ""
                || comboBoxTribunal.Text.Trim() == "")
            {
                MessageBox.Show("Inserto campos vacios.");
            }

            else
            {
                SQLiteConnection conexion = new SQLiteConnection(stringConnection);
                try
                {   
                        conexion.Open();

                        SQLiteCommand sqliteCommand = new SQLiteCommand("INSERT INTO Denuncia (Titulo_Denuncia," +
                            " Descripcion, Nombre_Denunciante, Nombre_Denunciado, Cedula_Denunciante, " +
                            " Cedula_Denunciado, Fecha_Hecho, Tribunal_Asignado) " +
                            "VALUES (@Titulo_Denuncia, @Descripcion, @Nombre_Denunciante, @Nombre_Denunciado, " +
                            "@Cedula_Denunciante, @Cedula_Denunciado, @Fecha_Hecho, @Tribunal_Asignado)", conexion);
                        sqliteCommand.Parameters.AddWithValue("@Titulo_Denuncia", textBoxTituloDenuncia.Text);
                        sqliteCommand.Parameters.AddWithValue("@Descripcion", textBoxDescripcionDenuncia.Text);
                        sqliteCommand.Parameters.AddWithValue("@Nombre_Denunciante",textBoxNombreDenunciante.Text);
                        sqliteCommand.Parameters.AddWithValue("@Nombre_Denunciado",textBoxNombreDenunciado.Text);
                        sqliteCommand.Parameters.AddWithValue("@Cedula_Denunciante",textBoxCedulaDenunciante.Text);
                        sqliteCommand.Parameters.AddWithValue("@Cedula_Denunciado",textBoxCedulaDenunciado.Text);
                        sqliteCommand.Parameters.AddWithValue("@Fecha_Hecho",labelHora.Text);
                        sqliteCommand.Parameters.AddWithValue("@Tribunal_Asignado",comboBoxTribunal.Text);

                        sqliteCommand.CommandType = CommandType.Text;
                        Fecha = labelHora.Text;

                        if (sqliteCommand.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Denuncia agregada exitosamente");
                        }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                finally
                {
                    conexion.Close();
                }
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
        private void buttonEditarTribunales_Click(object sender, EventArgs e)
        {
            Form form = new FormTribunales();
            this.Hide();
            form.ShowDialog();

            if(form.DialogResult == DialogResult.OK) 
            {
                this.Show();
                FillComboBox();
            }
        }

        private void buttonEditarDenuncia_Click(object sender, EventArgs e)
        {
            SQLiteConnection conexion = new SQLiteConnection(stringConnection);

            try
            {
                conexion.Open();

                SQLiteCommand sqliteCommand = new SQLiteCommand("update Denuncia " +
                    "set Titulo_Denuncia = @Titulo_Denuncia, Descripcion = @Descripcion, " +
                    "Descripcion = @Descripcion, Nombre_Denunciante = @Nombre_Denunciante, " +
                    "Nombre_Denunciado = @Nombre_Denunciado, Cedula_Denunciante = @Cedula_Denunciante, " +
                    "Cedula_Denunciado = @Cedula_Denunciado, Tribunal_Asignado = @Tribunal_Asignado " +
                    "where Titulo_Denuncia = @TituloDenuncia", conexion);

                sqliteCommand.Parameters.AddWithValue("@Titulo_Denuncia", textBoxTituloDenuncia.Text);
                sqliteCommand.Parameters.AddWithValue("@Descripcion", textBoxDescripcionDenuncia.Text);
                sqliteCommand.Parameters.AddWithValue("@Nombre_Denunciante", textBoxNombreDenunciante.Text);
                sqliteCommand.Parameters.AddWithValue("@Nombre_Denunciado", textBoxNombreDenunciado.Text);
                sqliteCommand.Parameters.AddWithValue("@Cedula_Denunciante", textBoxCedulaDenunciante.Text);
                sqliteCommand.Parameters.AddWithValue("@Cedula_Denunciado", textBoxCedulaDenunciado.Text);
                sqliteCommand.Parameters.AddWithValue("@Tribunal_Asignado", comboBoxTribunal.Text);
                sqliteCommand.Parameters.AddWithValue("@TituloDenuncia", textBoxTituloDenuncia.Text);
                sqliteCommand.CommandType = CommandType.Text;

                Fecha = labelHora.Text;
                if (sqliteCommand.ExecuteNonQuery() >0) 
                { 
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Text, ex.Message);
            }
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            SQLiteConnection conexion = new SQLiteConnection(stringConnection);

            try
            {
                conexion.Open();
                SQLiteCommand sqliteCommand = new SQLiteCommand("select * from Denuncia " +
                    "where Titulo_Denuncia = @Titulo_Denuncia", conexion);
                SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(sqliteCommand);
                sqliteCommand.CommandType = CommandType.Text;
                sqliteCommand.Parameters.AddWithValue("@Titulo_Denuncia", textBoxBuscar.Text);

                DataTable dataTable = new DataTable();
                sQLiteDataAdapter.Fill(dataTable);

                textBoxTituloDenuncia.Text = dataTable.Rows[0][1].ToString();
                textBoxDescripcionDenuncia.Text = dataTable.Rows[0][2].ToString();
                textBoxNombreDenunciante.Text = dataTable.Rows[0][3].ToString();
                textBoxNombreDenunciado.Text = dataTable.Rows[0][4].ToString();
                textBoxCedulaDenunciante.Text = dataTable.Rows[0][5].ToString();
                textBoxCedulaDenunciado.Text = dataTable.Rows[0][6].ToString();
                Fecha = dataTable.Rows[0][7].ToString();
                comboBoxTribunal.Text = String.Empty;
                comboBoxTribunal.SelectedText = dataTable.Rows[0][8].ToString();
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

        private void buttonExportar_Click(object sender, EventArgs e)
        {
            ExportarHTML(textBoxTituloDenuncia.Text, textBoxDescripcionDenuncia.Text,
                textBoxNombreDenunciante.Text, textBoxNombreDenunciado.Text, textBoxCedulaDenunciante.Text,
                textBoxCedulaDenunciado.Text, Fecha, comboBoxTribunal.Text);
        }

        public static void ExportarHTML(string Titulo, string descripcion, string denunciante,
         string denunciado, string denunciantecedula, string denunciadocedula, string fecha, string tribunal)
        {

            var plantilla = "";

            StreamReader sr = new StreamReader("Denuncia.html");
            plantilla = sr.ReadToEnd();
            sr.Close();

            plantilla = plantilla.Replace("#titulo#", Titulo);
            plantilla = plantilla.Replace("#descripcion#", descripcion);
            plantilla = plantilla.Replace("#denunciante#", denunciante);
            plantilla = plantilla.Replace("#denunciado#", denunciado);
            plantilla = plantilla.Replace("#cedula denunciante#", denunciantecedula);
            plantilla = plantilla.Replace("#cedula denunciado#", denunciadocedula);
            plantilla = plantilla.Replace("#fecha#",fecha);
            plantilla = plantilla.Replace("#tribunal asignado#", tribunal);

            System.IO.File.WriteAllText("DenunciaExportar.html", plantilla);

            var uri = "DenunciaExportar.html";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);

            MessageBox.Show("Denuncia exportada con exito.");
        }

        private void toolStripStatuslinkLabelEditar_Click(object sender, EventArgs e)
        {
            Form form = new FormEditarUsuario();
            this.Hide();
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
            {
                this.Show();
            }
        }

        private void toolStripStatusLabelCerrarSesion_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripStatusLabelAcercaDe_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = new DialogResult();
            Form form = new FormAcercaDe();
            this.Hide();
            form.ShowDialog();
            dialogResult = form.DialogResult;

            if (dialogResult == DialogResult.OK)
            {
                this.Show();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Program.Ciclo = false;
        }

        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            textBoxTituloDenuncia.Text = String.Empty;
            textBoxDescripcionDenuncia.Text = String.Empty;
            textBoxCedulaDenunciado.Text = String.Empty;
            textBoxCedulaDenunciante.Text = String.Empty;
            textBoxNombreDenunciado.Text = String.Empty;
            textBoxNombreDenunciante.Text = String.Empty;
            comboBoxTribunal.SelectedIndex = 0;

        }
    }
}
