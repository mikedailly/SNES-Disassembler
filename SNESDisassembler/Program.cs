namespace SNESDisassembler
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SNESDissassembler snes = new SNESDissassembler();
            string[] args =
                {
                    "snes.exe",
                    "-n",
                    @"C:\\source\\SNESDisassembler\\Lemmings 2 - The Tribes (U).smc"
                };

            snes.main(args);


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}