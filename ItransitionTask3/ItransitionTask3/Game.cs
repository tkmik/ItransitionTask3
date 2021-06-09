using System;
using System.Security.Cryptography;
using System.Text;

namespace ItransitionTask3
{
    internal class Game
    {
        private User CurrentUser { get; set; }
        private Computer CurrentComputer { get; set; }
        private string[] CurrentSigns { get; set; }

        public Game(string[] args)
        {
            CurrentUser = new User(args);
            CurrentComputer = new Computer(args);
            CurrentSigns = args;
        }

        public void Menu()
        {
            Console.WriteLine("Available moves:");
            for (int i = 0; i < CurrentSigns.Length; i++)
            {
                Console.WriteLine($"{i+1} - {CurrentSigns[i]}");
            }        
            Console.WriteLine("0 - Exit");
        }

        public int ReadSign()
        {
            Console.Write("Enter your move: ");
            int signNumber = 0;
            while(!int.TryParse(Console.ReadLine(),out signNumber) 
                || signNumber < 0 
                || signNumber > CurrentSigns.Length)
            {
                Menu();
            }
            CurrentUser.UserNumber = signNumber - 1;
            if (CurrentUser.UserNumber < 0)
            {
                return 0;
            }
            if (CurrentUser.UserNumber < CurrentSigns.Length && CurrentUser.UserNumber >= 0)
            {
                CurrentUser.SetMove();
                return 1;
            }
            return 0;
        }

        public void GetUserMove()
        {
            Console.Write("Your move: ");
            CurrentUser.GetMove();
        }

        public void GetComputerMove()
        {
            Console.Write("Computer move: ");
            CurrentComputer.ShowComputerMove();
        }

        public void GameRule()
        {
            Console.WriteLine();
        }

        private int CompareMoves(int userMove, int computerMove)
        {
            short[,] matrix = new short[CurrentSigns.Length, CurrentSigns.Length];
            for (int i = 0; i < CurrentSigns.Length; i++)
            {
                for (int j = i; j < CurrentSigns.Length; j++)
                {
                    if (i == j)
                    {
                        matrix[i, i] = 0;
                    }
                    else if (Math.Abs(i - j) % 2 == 1)
                    {
                        matrix[i, j] = 1;
                        matrix[j, i] = -1;
                    }
                    else if (Math.Abs(i - j) % 2 == 0)
                    {
                        matrix[i, j] = -1;
                        matrix[j, i] = 1;
                    }                       
                }
            }

            return matrix[userMove, computerMove];
        }

        public void ShowResult()
        {
            switch (CompareMoves(CurrentUser.UserNumber, CurrentComputer.ComputerNumber))
            {
                case 1:
                    Console.WriteLine("You win!");
                    break;
                case -1:
                    Console.WriteLine("You lose!");
                    break;
                default:
                    Console.WriteLine("Draw!");
                    break;
            }
        }

        public void ShowHMAC()
        {
            CurrentComputer.ShowHMAC();
        }

        public void ShowHMACKey()
        {
            Console.Write("HMAC key: ");
            CurrentComputer.ShowKey();
        }

        private class User
        {
            public int UserNumber { get; set; }
            public string UserMove { get; set; }
            public string[] CurrentSigns { get; set; }

            public User(string[] args)
            {
                CurrentSigns = args;
            }

            public void GetMove()
            {
                Console.WriteLine(UserMove);
            }

            public void SetMove()
            {
                UserMove = CurrentSigns[UserNumber];
            }
        }

        private class Computer
        {
            public int ComputerNumber { get; set; }
            public string ComputerMove { get; set; }
            private byte[] RNGKey { get; set; }
            private string Signature{ get; set; }
            public string[] Signs { get; set; }

            public Computer(string[] args)
            {                
                var rng = new RNGCryptoServiceProvider();
                RNGKey = new byte[128];
                rng.GetBytes(RNGKey);
                Signs = args;
                GetMove(Signs.Length);
                Signature = HMACSHA256(ComputerMove.ToString(), Convert.ToBase64String(RNGKey));
            }

            public string HMACSHA256(string text, string key)
            {
                byte[] bkey = Encoding.Default.GetBytes(key);
                using (var hmac = new HMACSHA256(bkey))
                {
                    byte[] btext = Encoding.Default.GetBytes(text);
                    return Convert.ToBase64String(hmac.ComputeHash(btext));
                }
            }

            public void GetMove(int signNumber)
            {
                Random r = new Random();
                ComputerNumber = r.Next(signNumber);
                ComputerMove = Signs[ComputerNumber];
            }

            public void ShowComputerMove()
            {
                Console.WriteLine(ComputerMove.ToString());
            }

            public void ShowKey()
            {
                Console.WriteLine($"Key -> {Convert.ToBase64String(RNGKey)}");        
            }

            public void ShowHMAC()
            {
                Console.WriteLine($"HMAC:{Environment.NewLine}{Signature}");
            }
        }
    }
}
