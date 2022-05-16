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
    public partial class FormEditarUsuario : Form
    {
        public FormEditarUsuario()
        {
            InitializeComponent();
            textBoxUsuario.Text = Program.Usuario;
            textBoxPassword.Text = Program.Password;
        }

        private static string ruta = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            SQLiteConnection conexion = new SQLiteConnection(ruta);

            try
            {
                conexion.Open();
                SQLiteCommand sqliteCommand = new SQLiteCommand("update Usuarios set User_Name = @Usuario, " +
                    "Password = @Password where User_Name = @Usuario_Viejo", conexion);
                sqliteCommand.CommandType = CommandType.Text;
                sqliteCommand.Parameters.AddWithValue("@Usuario", textBoxUsuario.Text);
                sqliteCommand.Parameters.AddWithValue("@Password", textBoxPassword.Text);
                sqliteCommand.Parameters.AddWithValue("@Usuario_Viejo", Program.Usuario);
                sqliteCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Text, ex.Message);
            }

            finally 
            {
                conexion.Close();
                conexion.Dispose();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
