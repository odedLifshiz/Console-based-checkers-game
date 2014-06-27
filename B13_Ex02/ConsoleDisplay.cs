namespace B13_Ex02
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using System.ComponentModel;

    public static class ConsoleDisplay
    {

        private static char[] m_arrOfLettersForCols = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j' };
        private static char[] m_arrOfLetterForRows = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

        public static void DisplayBoard(eCellType[,] i_Board)
        {
            int boardSize = (int)Math.Sqrt(i_Board.Length);

            // print the first row
            Console.Write("   ");
            for (int indexInArray = 0; indexInArray < boardSize; indexInArray++)
            {
                Console.Write(m_arrOfLetterForRows[indexInArray] + "   ");
            }

            // go down one line
            Console.Write("{0}",Environment.NewLine);
            printARowOfEaqualSymbol(boardSize);

            // go down one line
            Console.Write("{0}", Environment.NewLine);

            // print the array itself
            for (int currRow = 0; currRow < boardSize; currRow++)
            {
                for (int currCol = 0; currCol < boardSize; currCol++)
                {
                    if (currCol == 0)
                    {
                        Console.Write(m_arrOfLettersForCols[currRow] + "| " + GetEnumDescription(i_Board[currRow, currCol]) + " ");
                    }
                    else if (currCol == boardSize - 1)
                    {
                        Console.Write("| " + GetEnumDescription(i_Board[currRow, currCol]) + " |");
                    }
                    else
                    {
                        Console.Write("| " + GetEnumDescription(i_Board[currRow, currCol]) + " ");
                    }
                }

                // go down one line
                Console.WriteLine("");
                printARowOfEaqualSymbol(boardSize);
                Console.WriteLine("");
            }
        }

        private static void printARowOfEaqualSymbol(int i_BoardSize)
        {
            // print a row of ==
            for (int indexInArray = 0; indexInArray < i_BoardSize + 1; indexInArray++)
            {
                if (indexInArray == 0)
                {
                    Console.Write(" ==");
                }
                else if (indexInArray == i_BoardSize)
                {
                    Console.Write("===");
                }
                else
                {
                    Console.Write("====");
                }
            }
        }

        public static void ClearScreen()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }

        public static void DisplayTurnMessage(Player i_Player)
        {
            Console.WriteLine("{0}'s Turn ({1}):{2}Enter '{3}' if you wish to end the current game", i_Player.Name, GetEnumDescription(i_Player.PawnSymbol), Environment.NewLine, ConsoleReader.k_exitSymbol);
        }

        public static string GetEnumDescription(Enum i_Value)
        {
            string enumDescriptionAsString = null;
            FieldInfo fi = i_Value.GetType().GetField(i_Value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                enumDescriptionAsString = attributes[0].Description;
            }
            else
            {

                enumDescriptionAsString = i_Value.ToString();
            }

            return enumDescriptionAsString;
        }

        public static void DisplayDrawMessage()
        {
            Console.WriteLine("The game ended in a draw");
        }

        public static void DisplayWinningMessage(Player i_Player)
        {
            Console.WriteLine("{0} won", i_Player.Name);
        }

        public static void DisplayTheScore(Player i_Player1, Player i_Player2)
        {
            string msg = string.Format(
                "{0}The current score is:{0}Number of wins for {1} is: {2}{0}Number of wins for {3} is: {4}",
                Environment.NewLine, i_Player1.Name, i_Player1.Score, i_Player2.Name, i_Player2.Score);
            Console.WriteLine(msg);
        }
        public static void DisplayQuitMessage(Player i_PlayerThatQuit, Player i_PlayerThatWon)
        {
            string msg = string.Format("{0} quit the game{1}{2} won", i_PlayerThatQuit.Name, Environment.NewLine, i_PlayerThatWon.Name);
            Console.WriteLine(msg);
        }

        public static void DisplayBadMoveMessage(eMoveStatusCode i_MoveStatusCode)
        {
            Console.WriteLine("Invalid move");
            switch (i_MoveStatusCode)
            {
                case eMoveStatusCode.InvalidCoordinates: Console.WriteLine("Bad coordinates");
                    break;
                case eMoveStatusCode.MustJump: Console.WriteLine("You must jump");
                    break;
            }

            Console.WriteLine("Please enter move again");
        }

        public static void DisplayPlayerMustJumpAgainMessage(Player i_Player)
        {

            Console.WriteLine("{0} has an additional jump", i_Player.Name);
        }

        public static void DisplayWouldYouLikeToExitTheProgramMessage()
        {
            Console.WriteLine("If you wish to exit the game enter 2{0}To continue playing enter 1", Environment.NewLine);
        }

        public static void DisplayLastMove(Player i_Player, CheckersMove i_Move)
        {
            char colFrom = CheckersData.getColSymbolFromNumber(i_Move.FromCol);
            char rowFrom = CheckersData.getRowSymbolFromNumber(i_Move.FromRow);
            char colTo = CheckersData.getColSymbolFromNumber(i_Move.ToCol);
            char rowTo = CheckersData.getRowSymbolFromNumber(i_Move.ToRow);
            Console.WriteLine("{0}'s move was ({1}): {2}{3}>{4}{5}{6}"
                ,i_Player.Name, GetEnumDescription(i_Player.PawnSymbol), colFrom, rowFrom, colTo, rowTo, Environment.NewLine);
        }

        public static void DisplayListOfMoves(Player i_Player, List<CheckersMove> i_MovesTaken)
        {
            if (i_MovesTaken.Count == 1)
            {
                Console.WriteLine("{0}'s move was ({1}): ", i_Player.Name, GetEnumDescription(i_Player.PawnSymbol));
            }
            else
            {
                Console.WriteLine("{0}'s moves were ({1}): ", i_Player.Name, GetEnumDescription(i_Player.PawnSymbol));
            }
            for (int currentMove = 0; currentMove < i_MovesTaken.Count; currentMove++)
            {
                char colFrom = CheckersData.getColSymbolFromNumber(i_MovesTaken.ElementAt(currentMove).FromCol);
                char rowFrom = CheckersData.getRowSymbolFromNumber(i_MovesTaken.ElementAt(currentMove).FromRow);
                char colTo = CheckersData.getColSymbolFromNumber(i_MovesTaken.ElementAt(currentMove).ToCol);
                char rowTo = CheckersData.getRowSymbolFromNumber(i_MovesTaken.ElementAt(currentMove).ToRow);
                Console.Write("{0}{1}>{2}{3}", colFrom, rowFrom, colTo, rowTo);
                if (currentMove != i_MovesTaken.Count - 1)
                {
                    Console.Write(", ");
                }
            }
            Console.WriteLine("{0}", Environment.NewLine);
        }


        public static void DisplayGetPlayerNameMessage()
        {
            Console.WriteLine("Please enter player 1 name");
        }

        public static void DisplayGetBoardSizeMessage()
        {
            Console.WriteLine("please enter board size (6 8 or 10)");
        }

        public static void DisplayBadBoardSizeMessage()
        {
            Console.WriteLine("Bad input, board size must be 6 8 or 10");
        }

        public static void DisplayBadFormatForMoveMessage()
        {
            Console.WriteLine("Bad input, format must be COLrow>COLrow{0}Please enter move again: ", Environment.NewLine);
        }

        public static void DisplayChooseNumberOfHumanPlayersMessage()
        {
            Console.WriteLine("Please choose 1 or 2 players,{0}If you choose 1 you will play against the computer{0}" +
             "if you choose 2 you will play each other", Environment.NewLine);
        }

        public static void DisplayBadInputForChoosingNumberOfHumanPlayers()
        {
            Console.WriteLine("Bad input, must choose 1 or 2");
        }

        public static void DisplayGetSecondPlayerNameMessage()
        {
            Console.WriteLine("Please enter player 2 name");
        }

        public static void DisplayNextRoundMessage()
        {
            Console.WriteLine("Next round starts now{0}", Environment.NewLine);
        }

        public static void DisplayBadSelectionMessage()
        {
            Console.WriteLine("Bad selection, press 1 to continue playing or 2 to exit");
        }
    }

}
