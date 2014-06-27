namespace B13_Ex02
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public class GameRunner
    {
        // holds the status of each turn
        public enum eTurnStatusCode
        {
            turnSuccessfull,
            draw,
            otherPlayerWon,
            exitCurrentGame,
        }

        // starting point of the program
        public static void Main()
        {
            run_game();
        }

        // runs the game
        private static void run_game()
        {
            int boardSize;
            int numberOfHumanPlayers;
            ePlayerType player1Type;
            ePlayerType player2Type;
            Boolean exitProgram = false;
            Boolean roundOver;
            String player1Name = null;
            String player2Name = null;

            // get first player name
            player1Name = ConsoleReader.GetPlayerName();

            // get board size
            boardSize = ConsoleReader.GetBoardSize();

            // get number of human players
            numberOfHumanPlayers = ConsoleReader.GetNumberOfHumanPlayers();

            // if the user decided the game has two player he is asked to enter the second
            // player's name, we also set the player types
            player1Type = ePlayerType.human;

            if (numberOfHumanPlayers == 2)
            {
                player2Name = ConsoleReader.GetSecondPlayerName();
                player2Type = ePlayerType.human;
            }
            else
            {
                player2Type = ePlayerType.computer;
                player2Name = "computer";
            }

            // create two players
            // the first player moves down and uses the 'O' symbol for normal pawns and 'Q' for kings
            // the second player moves up and uses the 'X' symbol for normal pawns and 'K' for kings
            Player player1 = new Player(player1Name, player1Type, 0, eCellType.Player1RegularPawn, eCellType.Player1King, true);
            Player player2 = new Player(player2Name, player2Type, 0, eCellType.Player2RegularPawn, eCellType.Player2King, false);

            // create a board
            CheckersData checkersData = new CheckersData(boardSize);

            // this loop runs until the user chooses to exit the program
            while (exitProgram == false)
            {
                // initialize the board 
                checkersData.initializeBoard();

                // clear screen and display the board
                ConsoleDisplay.ClearScreen();
                ConsoleDisplay.DisplayBoard(checkersData.Board);
                roundOver = false;

                // this loop handles each game
                while (roundOver == false)
                {
                    // handle first player's turn
                    roundOver = handleTurn(player1, player2, checkersData);

                    // handle second player's turn
                    if (!roundOver)
                    {
                        roundOver = handleTurn(player2, player1, checkersData);
                    }

                }
                // the round is over, display the score and check if the user wants to exit the program
                ConsoleDisplay.DisplayTheScore(player1, player2);
                exitProgram = ConsoleReader.checkIfUserWantsToExit();
                if (exitProgram == false)
                {
                    ConsoleDisplay.DisplayNextRoundMessage();
                }
            }
        }

        // handle's a turn and displays the proper messages after each turn
        // if the current game is over return false
        private static Boolean handleTurn(Player i_PlayerThatActs, Player i_OtherPlayer, CheckersData i_CheckersData)
        {
            Boolean roundOver = false;
            eTurnStatusCode currentTurnStatusCode;
            currentTurnStatusCode = doTurn(i_PlayerThatActs, i_OtherPlayer, i_CheckersData);
            switch (currentTurnStatusCode)
            {
                case eTurnStatusCode.turnSuccessfull:
                    break;
                case eTurnStatusCode.draw: ConsoleDisplay.DisplayDrawMessage();
                    roundOver = true;
                    break;
                case eTurnStatusCode.otherPlayerWon: ConsoleDisplay.DisplayWinningMessage(i_OtherPlayer);
                    i_OtherPlayer.Score++;
                    roundOver = true;
                    break;
                case eTurnStatusCode.exitCurrentGame: ConsoleDisplay.DisplayQuitMessage(i_PlayerThatActs, i_OtherPlayer);
                    i_OtherPlayer.Score++;
                    roundOver = true;
                    break;
                default: break;
            }
            return roundOver;
        }

        // runs a turn and return a status code 
        private static eTurnStatusCode doTurn(Player i_PlayerThatActs, Player i_OtherPlayer, CheckersData i_CheckersData)
        {
            Boolean playerHasMoreMoves = true;
            CheckersMove move = null;
            List<CheckersMove> possibleMoves;
            List<CheckersMove> movesTaken = new List<CheckersMove>();
            eTurnStatusCode turnStatusCode = eTurnStatusCode.turnSuccessfull;
            int delayToDisplayMove = 2000;

            // get all possible moves for the player that plays this turn as a list
            // If the player has a jump the list will contain jumps only
            // if the list is null the player has no valid moves
            possibleMoves = i_CheckersData.GetLegalMoves(i_PlayerThatActs);

            // if the player doesn't have any valid moves
            // check if the other player has valid moves
            // if so he won, else it's a draw
            if (possibleMoves == null)
            {
                if (i_CheckersData.GetLegalMoves(i_OtherPlayer) == null)
                {
                    turnStatusCode = eTurnStatusCode.draw;
                }
                else
                {
                    turnStatusCode = eTurnStatusCode.otherPlayerWon;
                }
            }

             // if the game did not end handle the turn
            else
            {

                if (i_PlayerThatActs.TypeOfPlayer == ePlayerType.human)
                {
                    ConsoleDisplay.DisplayTurnMessage(i_PlayerThatActs);
                }
                
                // this loop handles each turn
                while (playerHasMoreMoves)
                {

                    // get a valid move from the player, if the player is a computer
                    // a move is randomly selected and move is never null
                    if ((move = getValidMove(i_PlayerThatActs, i_OtherPlayer, i_CheckersData, possibleMoves)) == null)
                    {
                        // if the method returned null the user input was the 'exit the game' symbol
                        turnStatusCode = eTurnStatusCode.exitCurrentGame;
                        break;
                    }

                    else
                    {
                        // else perform the move, this method returns true if the player must jump again
                        playerHasMoreMoves = i_CheckersData.DoMakeMove(i_PlayerThatActs, move);

                        // if the player is human diplay the new board and the move that was made
                        if (i_PlayerThatActs.TypeOfPlayer == ePlayerType.human)
                        {
                            ConsoleDisplay.ClearScreen();
                            ConsoleDisplay.DisplayBoard(i_CheckersData.Board);
                            ConsoleDisplay.DisplayLastMove(i_PlayerThatActs, move);
                        }

                        // else the player is a computer, we add all moves taken to a list and diplay 
                        // all the moves when the turn is over
                        else
                        {
                            movesTaken.Add(move);
                        }

                        possibleMoves = i_CheckersData.GetLegalMoves(i_PlayerThatActs);
                        if (playerHasMoreMoves && i_PlayerThatActs.TypeOfPlayer == ePlayerType.human)
                        {
                            ConsoleDisplay.DisplayPlayerMustJumpAgainMessage(i_PlayerThatActs);
                        }
                    }
                }
            }

            if (i_PlayerThatActs.TypeOfPlayer == ePlayerType.computer && turnStatusCode == eTurnStatusCode.turnSuccessfull)
            {
                Thread.Sleep(delayToDisplayMove);
                ConsoleDisplay.ClearScreen();
                ConsoleDisplay.DisplayBoard(i_CheckersData.Board);
                ConsoleDisplay.DisplayListOfMoves(i_PlayerThatActs, movesTaken);
            }

            return turnStatusCode;
        }

        // get's a valid move from the user, works for both computer and human players
        private static CheckersMove getValidMove(Player i_PlayerThatActs, Player i_OtherPlayer, CheckersData i_CheckersData, List<CheckersMove> i_ListOfMoves)
        {
            CheckersMove move = null;
            Boolean moveIsInvalid = true;
            eMoveStatusCode moveStatusCode;

            // this loop runs until the user inputs a valid move or enters 'exit the game' symbol
            while (moveIsInvalid)
            {
                move = getMoveFromPlayer(i_PlayerThatActs, i_ListOfMoves, (int)Math.Sqrt(i_CheckersData.Board.Length));

                // if the player is a computer the move is always valid so break this loop
                if (i_PlayerThatActs.TypeOfPlayer == ePlayerType.computer)
                {
                    break;
                }

                // if the move is null the user input was the 'exit the game' symbol
                // display proper message , update the score and continue to next game
                if (move == null)
                {
                    break;
                }

                // make sure the move is valid must verify that if the list of moves contains a jump the
                // user entered a valid jump. else the user is asked to re-enter the move
                if ((moveStatusCode = i_CheckersData.CheckIfMoveIsValid(i_PlayerThatActs, move, i_ListOfMoves)) == eMoveStatusCode.Successful)
                {
                    // the user entered a valid move
                    moveIsInvalid = false;
                }
                else
                {
                    // the move is invalid, display a message to re-enter the move and continue the loop
                    ConsoleDisplay.DisplayBadMoveMessage(moveStatusCode);
                }
            }

            return move;
        }

        // returns a move in the correct foramt, this method does not check the validity of the turn in relation to the board
        // if the player is human we read from console, if the player is a computer 
        // we use getComputerMove
        private static CheckersMove getMoveFromPlayer(Player i_Player, List<CheckersMove> i_ListOfMoves, int i_BoardLength)
        {
            CheckersMove move;
            if (i_Player.TypeOfPlayer == ePlayerType.computer)
            {
                move = getComputerMove(i_ListOfMoves);
            }
            else
            {
                move = ConsoleReader.GetMove(i_BoardLength);
            }

            return move;
        }

        // return the computer move 
        private static CheckersMove getComputerMove(List<CheckersMove> i_ListOfMoves)
        {
            int startingIndexOfList = 0;
            int endIndexOfList = i_ListOfMoves.Count - 1;
            CheckersMove computerMove;
            Random random = new Random();
            int randomNumber = random.Next(startingIndexOfList, endIndexOfList);
            computerMove = i_ListOfMoves.ElementAt(randomNumber);

            return computerMove;
        }

    }
}
