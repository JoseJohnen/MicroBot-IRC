using System;
using System.IO;

namespace BOT_IRC_SINGLE_INSTANCE
{
    class Program
    {
        ///<summary> Recuerda después de compilar mover el Release de 'BOT_IRC' a una carpeta
        ///con ese mismo nombre a donde vayas instalar el 'BOT_ADMIN', y cambiar el boolean
        ///en el método 'RunBOT' de 'true' a 'false'</summary>
        public static Bot bot;
        static void Main(string[] args)
        {
            SingleStance();
        }

        public static void SingleStance()
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                if (CommsDAO.PrepareLoadChat())
                {
                    bot = RunBOT();

                    if (bot != null)
                    {
                        bot.WorkingBot();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due To: " + ex.Message);
                Console.ReadLine();
            }
        }

        public static Bot RunBOT()
        {
            Restart:
            string PathToApp = Environment.CurrentDirectory + "\\Config.txt";
            string[] someString = new string[3];
            if (File.Exists(PathToApp))
            {
                Console.WriteLine("El archivo de configuracion Existe!" + "\n" + "Connectando a:");
                someString = File.ReadAllLines(PathToApp);
                Console.WriteLine("Servidor: "+someString[0]);
                Console.WriteLine("Nickname: "+someString[1]);
                Console.WriteLine("Canal: "+someString[2]);
                Console.WriteLine();
                return new Bot(someString[0], someString[1], someString[2]);
            }
            else
            {
                Console.WriteLine("NO EXISTE el archivo de configuración, creando uno con parametros estandar...");

                someString[0] = "chat.freenode.net";
                someString[1] = "RollBot";
                someString[2] = "#locos";

                File.WriteAllLines(PathToApp, someString);
                Console.Write("Archivo creado exitosamente, Comenzando conexión con parametros estandar");
                Console.WriteLine();
                goto Restart;
            }
        }
        
    }
}
