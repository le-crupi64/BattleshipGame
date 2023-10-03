using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;

namespace BattleshipLite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();
            PlayerInfoModel activePlayer = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");

            PlayerInfoModel winner = null;

            do
            {
                DisplayShotGrid(activePlayer);

                RecordPlayerShot(activePlayer, opponent);

                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

                if(doesGameContinue == true)
                {
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }

            } while (winner == null);

            IdentifyWinner(winner);

            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congradulations to {winner.PlayerName} for winning!");
            Console.WriteLine($"{winner.PlayerName} took {GameLogic.GetShotCount(winner)} shots.");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            bool isValidShot = false;
            string row = "";
            int column = 0;
            do
            {
                string shot = AskForShot(activePlayer);
                (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                isValidShot = GameLogic.ValidateShot(activePlayer, row, column);

                if(isValidShot == false)
                {
                    Console.WriteLine( "Invalid shot location. Please try again: ");
                }
            } while (isValidShot == false);

            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);
            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);
            DisplayShotResults(row, column, isAHit);
        }

        private static void DisplayShotResults(string row, int column, bool isAHit)
        {
            if(isAHit)
            {
                Console.WriteLine($"{row}{column} is a Hit!");
            }
            else
            {
                Console.WriteLine($"{row}{column} is a miss.");
            }
            Console.WriteLine();
        }

        private static string AskForShot(PlayerInfoModel player)
        {
            Console.WriteLine($"{player.PlayerName}, please Enter your shot Selection: ");
            string output = Console.ReadLine();

            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
           string currentRow = activePlayer.ShotGrid[0].SpotLetter;

           foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }


                if (gridSpot.Status == GridSpotStatus.Empty) 
                {
                    Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber} ");
                }
                else if(gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write("x ");
                }
                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write("o ");
                }
                else
                {
                    Console.Write("? ");
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battleship Lite");
            Console.WriteLine("Created by Leigh Crupi");
            Console.WriteLine("For the IamTimCorey C# Master Course");
            Console.WriteLine();
        }
        private static PlayerInfoModel CreatePlayer( string playerTitle)
        {
            Console.WriteLine($"Player info for {playerTitle}:");
            PlayerInfoModel output = new PlayerInfoModel();
            output.PlayerName = AskForUsersName();
            GameLogic.InitializeGrid(output);
            PlaceShips(output);
            Console.Clear();
            return output;


        }
        private static string AskForUsersName()
        {
            Console.WriteLine("What is Your name: ");
            string output = Console.ReadLine();

            return output;
        }
        private static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.WriteLine($"Where do you want to place ship number { model.ShipLocations.Count + 1}: ");
                string location = Console.ReadLine();

                bool isValidLocation = GameLogic.PlaceShip(model, location);

                if (isValidLocation == false)
                {
                    Console.WriteLine("That was not a valid location. Please try again.");
                }

            } while (model.ShipLocations.Count < 5);
        }
    }
}
