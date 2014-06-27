namespace B13_Ex02
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class ConsoleReader
    {
        public const string k_exitSymbol = "Q";

        // gets a move from the user and return an object representing the move or null if the move is invalid  
        public static CheckersMove GetMove(int i_BoardSize)
        {
            CheckersMove move = null;
            Boolean userPressedExit = false;

            // get user input as string
            String userInputAsString = Console.ReadLine();

            // if the user enters exit sybol return null
            if (userInputAsString.Equals(k_exitSymbol))
            {
                userPressedExit = true;
            }
            else
            {
                Regex r = null;
                switch (i_BoardSize)
                {
                    case 6:
                        r = new Regex("^[A-F][a-f]>[A-F][a-f]$");
                        break;

                    case 8:
                        r = new Regex("^[A-H][a-h]>[A-H][a-h]$");
                        break;

                    case 10:
                        r = new Regex("^[A-J][a-j]>[A-J][a-j]$");
                        break;
                }

                Match m = r.Match(userInputAsString);

                // this loop runs until the user enters a move in the right format (not necessarily a valid move yet).
                while (!m.Success)
                {
                    ConsoleDisplay.DisplayBadFormatForMoveMessage();
                    userInputAsString = Console.ReadLine();

                    // if the user enters exit sybol return null
                    if (userInputAsString.Equals(k_exitSymbol))
                    {
                        userPressedExit = true;
                        break;
                    }

                    m = r.Match(userInputAsString);
                }


                // if the user entered a move in the correct format extract the coordinates from it
                if (!userPressedExit)
                {
                    int playerCurrentCol;
                    int playerCurrentRow;
                    int playerMoveRow;
                    int playerMoveCol;

                    playerCurrentCol = CheckersData.getColNumberFromSymbol(userInputAsString.ElementAt(0));
                    playerCurrentRow = CheckersData.getRowNumberFromSymbol(userInputAsString.ElementAt(1));
                    playerMoveCol = CheckersData.getColNumberFromSymbol(userInputAsString.ElementAt(3));
                    playerMoveRow = CheckersData.getRowNumberFromSymbol(userInputAsString.ElementAt(4));
                    move = new CheckersMove(playerCurrentRow, playerCurrentCol, playerMoveRow, playerMoveCol);
                }
            }

            return move;
        }

        public static Boolean checkIfUserWantsToExit()
        {
            string userInput;
            Boolean wantsToExit = false;
            ConsoleDisplay.DisplayWouldYouLikeToExitTheProgramMessage();
            userInput = Console.ReadLine();

            while ((!userInput.Equals("2") && !userInput.Equals("1")))
            {
                ConsoleDisplay.DisplayBadSelectionMessage();
                userInput = Console.ReadLine();
            }

            if (userInput.Equals("2"))
            {
                wantsToExit = true;
            }
            else
            {
                wantsToExit = false;
            }

            return wantsToExit;
        }

        public static string GetPlayerName()
        {
            ConsoleDisplay.DisplayGetPlayerNameMessage();

            return Console.ReadLine();
        }

        public static int GetBoardSize()
        {
            int boardSize;
            ConsoleDisplay.DisplayGetBoardSizeMessage();
            String boardSizeAsString = Console.ReadLine();

            while (!(int.TryParse(boardSizeAsString, out boardSize)) || !(boardSize == 6 || boardSize == 8 || boardSize == 10))
            {
                ConsoleDisplay.DisplayBadBoardSizeMessage();
                boardSizeAsString = Console.ReadLine();
            }

            return boardSize;
        }

        // user decides if the game has two players or one player and a computer
        public static int GetNumberOfHumanPlayers()
        {
            int numberOfHumanPlayers;
            ConsoleDisplay.DisplayChooseNumberOfHumanPlayersMessage();
            String chooseOneOrTwoPlayersAsString = Console.ReadLine();

            while (!(int.TryParse(chooseOneOrTwoPlayersAsString, out numberOfHumanPlayers)) || !(numberOfHumanPlayers == 1 || numberOfHumanPlayers == 2))
            {
                ConsoleDisplay.DisplayBadInputForChoosingNumberOfHumanPlayers();
                chooseOneOrTwoPlayersAsString = Console.ReadLine();
            }

            return numberOfHumanPlayers;
        }

        public static string GetSecondPlayerName()
        {
            ConsoleDisplay.DisplayGetSecondPlayerNameMessage();

            return Console.ReadLine();

        }
    }




}
