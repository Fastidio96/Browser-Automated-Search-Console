namespace Browser_Automated_Search_Console
{
    using System;

    public class Messages
    {
        public static void Header()
        {
            GiveMeSomeSpace();
            Msg("BROWSER AUTOMATED SEARCH CONSOLE by Fastidio96");
            GiveMeSomeSpace();
        }

        public static void GiveMeSomeSpace() //And wear the mask! :D
        {
            Console.WriteLine("\n\r");
        }

        public static void Clear()
        {
            Console.Clear();
            Header();
        }

        public static void Log(string message, bool inline = false)
        {
            if (inline)
            {
                Console.Write(">> " + message);
                return;
            }

            Console.WriteLine(">> " + message);
        }

        public static void Msg(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(">> " + message);
            Console.ResetColor();
        }

        public static void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(">> " + message);
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(">> " + message);
            Console.ResetColor();
        }
        public static void Error(string message, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(">> " + message);
            Console.WriteLine(">> " + exception.Message);
            Console.ResetColor();
        }
        
    }
}
