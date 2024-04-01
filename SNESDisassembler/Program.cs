using System.Text;

namespace SNESDisassembler
{
    internal static class Program
    {
        public static SymbolManager Symbols;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Symbols = new SymbolManager();
            DefaultSymbols.Init(Symbols);
            DefaultSymbols.AddDefaults();

            SNESDissassembler snes = new SNESDissassembler(Symbols);
            string[] args =
                {
                    "snes.exe",
                    "-n",
                    "-i",
                    @"C:\\source\\SNESDisassembler\\Lemmings 2 - The Tribes (U).smc"
                };

            snes.main(args);
            StringBuilder res = snes.Dissassembly;


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(snes.Segments));
        }
    }
}