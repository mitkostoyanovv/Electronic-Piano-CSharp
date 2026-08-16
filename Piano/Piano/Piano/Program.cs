using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// namespace трябва да съвпада с твоя MainForm.cs
namespace PianoProject
{
    internal static class Program
    {
        /// <summary>
        /// Главната точка за стартиране на приложението.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}