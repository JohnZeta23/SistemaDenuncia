using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramaSistemaDenuncia
{
    internal static class Program
    {
        public static int Opciones = 0;
        public static bool Ciclo = true;
        public static string Usuario = String.Empty;
        public static string Password = String.Empty;

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            secuencia();
        }

        public static void secuencia() 
        {
            while (Ciclo == true) {
                Application.Run(new FormLogin());

                if (Opciones == 1)
                {
                    Application.Run(new FormRegistrar());
                }

                if (Opciones == 2)
                {
                    Application.Run(new FormMenu());
                }
            }
        }
    }
}
