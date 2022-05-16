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
    public partial class FormRegistrar : Form
    {
        public FormRegistrar()
        {
            InitializeComponent();
        }

        private static string stringConnection = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        private void buttonRegistrar_Click(object sender, EventArgs e)
        {
            if (textBoxUsuario.Text.Trim() == "" || textBoxPassword.Text.Trim() == "")
            {
                MessageBox.Show("Inserto campos vacios");
            }

            else
            {
                SQLiteConnection conexion = new SQLiteConnection(stringConnection);
                try
                {
                        conexion.Open();
                        SQLiteCommand sqliteCommand = new SQLiteCommand("INSERT INTO Usuarios(User_Name, Password) " +
                            "VALUES (@Usuario,@Password)", conexion);
                        sqliteCommand.Parameters.AddWithValue("@Usuario", textBoxUsuario.Text);
                        sqliteCommand.Parameters.AddWithValue("@Password", textBoxPassword.Text);
                        sqliteCommand.CommandType = CommandType.Text;

                    if(sqliteCommand.ExecuteNonQuery() > 0) 
                    {
                        conexion.Close();
                        Program.Opciones = 2;
                        Program.Usuario = textBoxUsuario.Text;
                        Program.Password = textBoxPassword.Text;
                        this.Close();
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Program.Ciclo = false;
        }
    }
}

