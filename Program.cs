using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ImageShaper
{
    static class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                AttachConsole(ATTACH_PARENT_PROCESS);

                if (args[0].Contains("?"))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("commandline usage");
                    Console.WriteLine("-c=[0-Undefined,1-Uncompressed,2-RLE_Zero,3-Detect_best_size,4-Uncompressed_Full_Frame] set compression e.g. -c=3");
                    Console.WriteLine("-p=[path+filename] to specify the palette (*.pal) or palettesetup (*.isps)");
                    Console.WriteLine("-i=[path+filename] input file(s) e.g. -i=\"d:\\test folder\\myimg*.png\"");
                    Console.WriteLine("-o=[path+filename] output file e.g. -o=\"d:\\test folder\\myimg.shp\" (no wildcards allowed)");
                    Console.WriteLine("-z Don't close ImageShaper after the SHP conversion is finished.");
                    Console.WriteLine("-optcan=[0-off-no,1-on-yes] set the \"optimize canvas\" option e.g. -optcan=off");
                    Console.WriteLine("-centered=[0-off-no,1-on-yes] set the \"keep centered\" option e.g. -centered=yes");
                    Console.WriteLine("-stopwobblebug=[0-off-no,1-on-yes] set the \"prevent TS wobble bug\" option e.g. -stopwobblebug=0");
                    Console.WriteLine("-split=# sets the Split result option. e.g. -split=2 to split the result into 2 SHPs");
                    Console.ResetColor();
                    return;
                }
                //parse the batch parameters
                Form_ImageShaper fis = new Form_ImageShaper();
                fis.RunCommand(args);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form_ImageShaper());
            }
        }
    }
}
