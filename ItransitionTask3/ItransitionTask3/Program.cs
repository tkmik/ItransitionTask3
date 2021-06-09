using System;
using System.Linq;

namespace ItransitionTask3
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 3)
                {
                    throw new ArgumentException("Wrong number of parameters - must be more than 3");
                }
                else if (args.Length % 2 != 1)
                {
                    throw new ArgumentException("Wrong number of parameters - must be odd");
                }
                else if (args.Count() != args.Distinct().Count())
                {
                    throw new ArgumentException("Wrong names of parametres - must be unique");
                }
                else
                {
                    Game game = new Game(args);
                    game.ShowHMAC();
                    game.Menu();
                    if (game.ReadSign() == 1)
                    {
                        game.GetUserMove();
                        game.GetComputerMove();
                        game.ShowResult();
                        game.ShowHMACKey();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine($"Correct entry of parameters:{Environment.NewLine}Rock Paper Scissors ...{Environment.NewLine}");
            }                    
        }
    }
}
