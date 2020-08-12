using CSharply;
using System;
using System.Collections.Generic;
using System.Text;


namespace CSharplyTesting
{
    class Testing
    {
        public static void Main()
        {
            Console.ReadKey();
            Console.SetWindowSize(200, 40);
            Console.WriteLine(Console.BufferWidth);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            ConsoleSection x = new ConsoleSection(5, 2, 11, 3);
            ConsoleSection y = new ConsoleSection(20, 1, 19, 10);

            x.Config.TextBackgroundColor = ConsoleColor.Green;
            x.Config.TextForegroundColor = ConsoleColor.Black;

            y.Config.TextBackgroundColor = ConsoleColor.Yellow;
            y.Config.TextForegroundColor = ConsoleColor.Blue;

            y.Config.TextWrap = true;

            x.NewLine();
            x.AppendText(" CSharply!");
            y.NewLine();
            y.AppendText("  Someday it will   actually do cool      stuff!");

            x.Draw();
            y.Draw();

            Console.SetCursorPosition(1, 12);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Press any key to continue...");
            Console.ReadKey();


            //ConsoleContainer t = new ConsoleContainer();



        }
    }
}
