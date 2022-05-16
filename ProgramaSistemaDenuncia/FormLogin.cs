using Microsoft.Data.Sqlite;
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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void linkLabelRegistrar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.Opciones = 1;
            this.Close();
        }

        private static string stringConnection = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        private void buttonLogin_Click(object sender, EventArgs e)
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
                    SQLiteCommand sqliteCommand = new SQLiteCommand("select * from Usuarios " +
                        "Where User_Name = @Usuario and Password = @Password", conexion);
                    sqliteCommand.Parameters.AddWithValue("@Usuario", textBoxUsuario.Text);
                    sqliteCommand.Parameters.AddWithValue("@Password", textBoxPassword.Text);
                    sqliteCommand.CommandType = CommandType.Text;
                    
                    SQLiteDataReader sQLiteDataReader = sqliteCommand.ExecuteReader();
                    sQLiteDataReader.Read();

                    if (sQLiteDataReader.HasRows == true)
                    {
                        Program.Opciones = 2;
                        Program.Usuario = textBoxUsuario.Text;
                        Program.Password = textBoxPassword.Text;
                        this.Close();
                    }

                    else 
                    {
                        MessageBox.Show("Usuario y/o contraseña incorrectos");
                    }
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
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Program.Ciclo = false;
        }
    }
}
