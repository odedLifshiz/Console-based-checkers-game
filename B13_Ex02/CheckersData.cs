namespace B13_Ex02
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;

    // an object of this class contains the board and all of the logic behind the checkers game
    public enum eCellType
    {
        [Description(" ")]
        EMPTY,
        [Description("O")]
        Player1RegularPawn,
        [Description("Q")]
        Player1King,
        [Description("X")]
        Player2RegularPawn,
        [Description("K")]
        Player2King,
    }

    // values that represent each move status code
    public enum eMoveStatusCode
    {
        Successful,
        MustJump,
        InvalidCoordinates
    }

    public class CheckersData
    {
        private eCellType[,] m_Board;
        private int m_BoardSize;

        //Constructor. Creates the board and set it up for a new game.
        public CheckersData(int i_BoardSize)
        {
            this.m_BoardSize = i_BoardSize;
            m_Board = new eCellType[m_BoardSize, m_BoardSize];
        }

        // getter for the board
        public eCellType[,] Board
        {
            get
            {
                return this.m_Board;
            }
        }

        // initializes the cells of the board to their starting values
        public void initializeBoard()
        {
            int numberOfRowsOfSoldiersForEachPlayer = m_BoardSize / 2 - 1;

            for (int row = 0; row < m_BoardSize; row++)
            {
                for (int col = 0; col < m_BoardSize; col++)
                {
                    // if this is not an empty cell
                    if (!(((row + col) % 2) == 0))
                    {
                        // if this cell should contain a black player
                        if (row < numberOfRowsOfSoldiersForEachPlayer)
                        {
                            m_Board[row, col] = eCellType.Player1RegularPawn;
                        }

                         // if this cell should contain a red player
                        else if (row > m_BoardSize - numberOfRowsOfSoldiersForEachPlayer - 1)
                        {
                            m_Board[row, col] = eCellType.Player2RegularPawn;
                        }
                        else
                        {
                            m_Board[row, col] = eCellType.EMPTY;
                        }
                    }
                    else
                    {
                        m_Board[row, col] = eCellType.EMPTY;
                    }
                }
            }
        }

        /**
        * Return a list containing all the legal CheckersMoves
        * for the specified player on the current board.  If the player
        * has no legal moves, null is returned. 
         * If the returned value is non-null, it consists
        * entirely of jump moves or entirely of regular moves, since
        * if the player can jump, only jumps are legal moves.
        */
        public List<CheckersMove> GetLegalMoves(Player i_Player)
        {
            eCellType playerPawnSybol = i_Player.PawnSymbol;
            eCellType playerKingSymbol = i_Player.KingSymbol;
            List<CheckersMove> listOfMoves = new List<CheckersMove>();

            /*  First, check for any possible jumps.  Look at each square on the board.
            If that square contains one of the player's pieces, look at a possible
            jump in each of the four directions from that square.  If there is 
            a legal jump in that direction, put it in the moves list.
            */
            for (int row = 0; row < m_BoardSize; row++)
            {
                for (int col = 0; col < m_BoardSize; col++)
                {
                    // check for all 4 possible jumps
                    if (m_Board[row, col] == i_Player.PawnSymbol || m_Board[row, col] == i_Player.KingSymbol)
                    {
                        if (canJump(i_Player, row, col, row + 1, col + 1, row + 2, col + 2))
                        {
                            listOfMoves.Add(new CheckersMove(row, col, row + 2, col + 2));
                        }
                        if (canJump(i_Player, row, col, row + 1, col - 1, row + 2, col - 2))
                        {
                            listOfMoves.Add(new CheckersMove(row, col, row + 2, col - 2));
                        }
                        if (canJump(i_Player, row, col, row - 1, col - 1, row - 2, col - 2))
                        {
                            listOfMoves.Add(new CheckersMove(row, col, row - 2, col - 2));
                        }
                        if (canJump(i_Player, row, col, row - 1, col + 1, row - 2, col + 2))
                        {
                            listOfMoves.Add(new CheckersMove(row, col, row - 2, col + 2));
                        }
                    }
                }
            }

            /*  If any jump moves were found, then the user must jump, so we don't 
            add any regular moves. However, if no jumps were found, check for
            any legal regular moves.  Look at each square on the board.
            If that square contains one of the player's pawns, look at a possible
            move in each of the four directions from that square.  If there is 
            a legal move in that direction, put it in the moves list.
            */
            if (listOfMoves.Count == 0)
            {
                for (int row = 0; row < m_BoardSize; row++)
                {
                    for (int col = 0; col < m_BoardSize; col++)
                    {

                        // check 4 possible moves
                        if (m_Board[row, col] == playerPawnSybol || m_Board[row, col] == playerKingSymbol)
                        {
                            if (checkRegularMove(i_Player, row, col, row + 1, col - 1))
                            {
                                listOfMoves.Add(new CheckersMove(row, col, row + 1, col - 1));
                            }
                            if (checkRegularMove(i_Player, row, col, row - 1, col - 1))
                            {
                                listOfMoves.Add(new CheckersMove(row, col, row - 1, col - 1));
                            }
                            if (checkRegularMove(i_Player, row, col, row + 1, col + 1))
                            {
                                listOfMoves.Add(new CheckersMove(row, col, row + 1, col + 1));
                            }
                            if (checkRegularMove(i_Player, row, col, row - 1, col + 1))
                            {
                                listOfMoves.Add(new CheckersMove(row, col, row - 1, col + 1));
                            }
                        }
                    }
                }
            }

            // If no legal moves have been found, return null.  Otherwise, return the list of moves
            if (listOfMoves.Count() == 0)
            {
                listOfMoves = null;
            }
            return listOfMoves;
        }

        /**    
        * This method checks if a move can be made from r1,c1 to r3,c3
        * it is assumed that the player has a pawn in r1 c1. 
        */
        private Boolean checkRegularMove(Player i_Player, int i_R1, int i_C1, int i_R2, int i_C2)
        {
            Boolean isValidMove = true;

            // if r2,c2 is off the board return false
            if (i_R2 < 0 || i_R2 >= m_BoardSize || i_C2 < 0 || i_C2 >= m_BoardSize)
            {
                isValidMove = false;
            }

            // check the move is diagonal
            else if (Math.Abs(i_R1 - i_R2) != 1 || Math.Abs(i_C1 - i_C2) != 1)
            {
                isValidMove = false;
            }

            // if r2,c2 is not empty return false
            else if (m_Board[i_R2, i_C2] != eCellType.EMPTY)
            {
                isValidMove = false;
            }

            // if the pawn in r1,c1 is a normal pawn and r2,c2 
            // is not in the direction the player is moving return false
            else if (m_Board[i_R1, i_C1] == eCellType.Player1RegularPawn && i_R1 >= i_R2)
            {
                isValidMove = false;
            }
            else if (m_Board[i_R1, i_C1] == eCellType.Player2RegularPawn && i_R1 <= i_R2)
            {
                isValidMove = false;
            }

            return isValidMove;
        }

        private Boolean isJump(CheckersMove i_Move)
        {
            // Test whether this move is a jump.  It is assumed that
            // the move is legal.  In a jump, the piece moves two
            // rows.  (In a regular move, it only moves one row.)
            return isJump(i_Move.FromRow, i_Move.FromCol, i_Move.ToRow, i_Move.ToCol);
        }

        private Boolean isJump(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol)
        {
            Boolean isJump = (Math.Abs(i_FromRow - i_ToRow) == 2) && (Math.Abs(i_FromCol - i_ToCol) == 2);
            return isJump;
        }

        /**    
        * This method checks if a jump can be made from r1,c1 over r2,c2 to r3,c3
        * it is assumed that the player has a pawn in r1 c1. 
        */
        private Boolean canJump(Player i_Player, int i_FromRow, int i_FromCol, int i_MiddleRow, int i_MiddleCol, int i_ToRow, int i_ToCol)
        {
            Boolean canJump = true;

            // if destination row,col are off the board return false
            if (i_ToRow < 0 || i_ToRow >= m_BoardSize || i_ToCol < 0 || i_ToCol >= m_BoardSize)
            {
                canJump = false;
            }

            // if destination row,col already contain a piece return false
            else if (m_Board[i_ToRow, i_ToCol] != eCellType.EMPTY)
            {
                canJump = false;
            }

            // if the cell in middle row,col is empty return false
            else if (m_Board[i_MiddleRow, i_MiddleCol] == eCellType.EMPTY)
            {
                canJump = false;
            }

            // if the pawns in  source row,col and middle row,col  belong to player 1 return false
            else if ((m_Board[i_FromRow, i_FromCol] == eCellType.Player1RegularPawn || m_Board[i_FromRow, i_FromCol] == eCellType.Player1King)
                && (m_Board[i_MiddleRow, i_MiddleCol] == eCellType.Player1RegularPawn || m_Board[i_MiddleRow, i_MiddleCol] == eCellType.Player1King))
            {
                canJump = false;
            }

            // if the pawns in  source row,col and middle row,col  belong to player 2 return false
            else if ((m_Board[i_FromRow, i_FromCol] == eCellType.Player2RegularPawn || m_Board[i_FromRow, i_FromCol] == eCellType.Player2King)
                && (m_Board[i_MiddleRow, i_MiddleCol] == eCellType.Player2RegularPawn || m_Board[i_MiddleRow, i_MiddleCol] == eCellType.Player2King))
            {
                canJump = false;
            }

           // if this is a regualr pawn check that the move is in the direction the player is moving
            else if ((m_Board[i_FromRow, i_FromCol] == eCellType.Player1RegularPawn) && (i_ToRow < i_FromRow))
            {
                canJump = false;
            }

            // if this is a player1 regualr pawn check that the move is in the direction the player is moving
            else if ((m_Board[i_FromRow, i_FromCol] == eCellType.Player1RegularPawn) && (i_ToRow < i_FromRow))
            {
                canJump = false;
            }

               // if this is a player2 regualr pawn check that the move is in the direction the player is moving
            else if ((m_Board[i_FromRow, i_FromCol] == eCellType.Player2RegularPawn) && (i_ToRow > i_FromRow))
            {
                canJump = false;
            }

            return canJump;
        }

        // Check if a move is valid given all the possible moves
        // nothing is assumed on the validity of the move
        // this method uses canJump and the checkRegularMove methods, both methods assume we have the move 
        // source row and col has a pawn, we check that condition and than use them 
        // to check the move.
        // an enum is returned to represent the validity of the move
        public eMoveStatusCode CheckIfMoveIsValid(Player i_Player, CheckersMove i_Move, List<CheckersMove> i_ListOfMoves)
        {
            eMoveStatusCode moveStatusCode = eMoveStatusCode.Successful;
            // check the source row and col coordinates are on the board
            if (i_Move.FromRow < 0 || i_Move.FromRow >= m_BoardSize || i_Move.FromCol < 0 || i_Move.FromCol >= m_BoardSize)
            {
                moveStatusCode = eMoveStatusCode.InvalidCoordinates;
            }

            // check that the player has a pawn in the given source row and col
            else if (m_Board[i_Move.FromRow, i_Move.FromCol] != i_Player.PawnSymbol && m_Board[i_Move.FromRow, i_Move.FromCol] != i_Player.KingSymbol)
            {
                moveStatusCode = eMoveStatusCode.InvalidCoordinates;
            }

            // if the player can jump the move must be a valid jump
            else if (isJump(i_ListOfMoves[0]) && !isJump(i_Move.FromRow, i_Move.FromCol, i_Move.ToRow, i_Move.ToCol))
            {
                moveStatusCode = eMoveStatusCode.MustJump;
            }

            // if the move is a jump check if it is a valid jump
            else if (isJump(i_Move.FromRow, i_Move.FromCol, i_Move.ToRow, i_Move.ToCol))
            {
                // r2,i_c2 are the row and col between the starting point and ending point of the jump
                int r2 = (i_Move.FromRow + i_Move.ToRow) / 2;
                int c2 = (i_Move.FromCol + i_Move.ToCol) / 2;

                // if it is an invalid jump return false
                if (!canJump(i_Player, i_Move.FromRow, i_Move.FromCol, r2, c2, i_Move.ToRow, i_Move.ToCol))
                {
                    moveStatusCode = eMoveStatusCode.InvalidCoordinates;
                }
            }
            else
            {
                // else the move is a regular move check if it is a valid move
                if (!checkRegularMove(i_Player, i_Move.FromRow, i_Move.FromCol, i_Move.ToRow, i_Move.ToCol))
                {
                    moveStatusCode = eMoveStatusCode.InvalidCoordinates;
                }
            }

            return moveStatusCode;
        }

        // performs the move and checks for additional jumps
        public Boolean DoMakeMove(Player i_Player, CheckersMove i_Move)
        {
            Boolean hasMoreJumps = false;
            List<CheckersMove> jumps;

            // performs the given move, it is assumed the move is valid
            makeMove(i_Player, i_Move);

            /* If the move was a jump, check if the player has another
             jump from the square that the player just jumped to. 
             if so return true
             */
            if (isJump(i_Move) && (jumps = getLegalJumpsFrom(i_Player, i_Move.ToRow, i_Move.ToCol)) != null)
            {
                hasMoreJumps = true;
            }

            return hasMoreJumps;
        }

        /**
     * Return a list of the legal jumps that the specified player can
     * make starting from the specified row and column. If no such
     * jumps are possible, null is returned.  The logic is similar
     * to the logic of the getLegalMoves() method.
     */
        private List<CheckersMove> getLegalJumpsFrom(Player i_Player, int i_Row, int i_Col)
        {

            List<CheckersMove> listOfMoves = new List<CheckersMove>();

            if (m_Board[i_Row, i_Col] == i_Player.PawnSymbol || m_Board[i_Row, i_Col] == i_Player.KingSymbol)
            {
                if (canJump(i_Player, i_Row, i_Col, i_Row + 1, i_Col + 1, i_Row + 2, i_Col + 2))
                {
                    listOfMoves.Add(new CheckersMove(i_Row, i_Col, i_Row + 2, i_Col + 2));
                }
                if (canJump(i_Player, i_Row, i_Col, i_Row - 1, i_Col + 1, i_Row - 2, i_Col + 2))
                {
                    listOfMoves.Add(new CheckersMove(i_Row, i_Col, i_Row - 2, i_Col + 2));
                }
                if (canJump(i_Player, i_Row, i_Col, i_Row + 1, i_Col - 1, i_Row + 2, i_Col - 2))
                {
                    listOfMoves.Add(new CheckersMove(i_Row, i_Col, i_Row + 2, i_Col - 2));
                }
                if (canJump(i_Player, i_Row, i_Col, i_Row - 1, i_Col - 1, i_Row - 2, i_Col - 2))
                {
                    listOfMoves.Add(new CheckersMove(i_Row, i_Col, i_Row - 2, i_Col - 2));
                }
            }
            if (listOfMoves.Count == 0)
            {
                listOfMoves = null;
            }

            return listOfMoves;
        }

        /**
        * Make the specified move.  It is assumed that move
        * is non-null and that the move it represents is legal.
        */
        private void makeMove(Player i_Player, CheckersMove i_Move)
        {
            makeMove(i_Player, i_Move.FromRow, i_Move.FromCol, i_Move.ToRow, i_Move.ToCol);
        }

        /**
           * Make the move from fromRow,fromCol to (toRow,toCol).  It is
           * assumed that this move is legal. If the move is a jump, the
           * jumped piece is removed from the board.  If a piece moves
           * the last row on the opponent's side of the board, the 
           * piece becomes a king.
           */
        private void makeMove(Player i_Player, int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol)
        {
            m_Board[i_ToRow, i_ToCol] = m_Board[i_FromRow, i_FromCol];
            m_Board[i_FromRow, i_FromCol] = eCellType.EMPTY;
            if (Math.Abs(i_FromRow - i_ToRow) == 2)
            {
                // The move is a jump.  Remove the piece we jumped above from the board;
                int jumpedPawnRow = (i_FromRow + i_ToRow) / 2;
                int jumpPawnCol = (i_FromCol + i_ToCol) / 2;
                m_Board[jumpedPawnRow, jumpPawnCol] = eCellType.EMPTY;
            }

            // if the player that moves down reached the bottom row with a regular pawn make this pawn a king
            if (i_ToRow == m_BoardSize - 1 && i_Player.MovesDown && !(m_Board[i_ToRow, i_ToCol] == eCellType.Player2King))
            {
                m_Board[i_ToRow, i_ToCol] = eCellType.Player1King; ;
            }

            // if the player that moves up reached row 0 with a regualr pawn make this pawn a king
            if (i_ToRow == 0 && !i_Player.MovesDown && !(m_Board[i_ToRow, i_ToCol] == eCellType.Player2King))
            {
                m_Board[i_ToRow, i_ToCol] = eCellType.Player2King;
            }
        }

        public static char getRowSymbolFromNumber(int i_RowNumber)
        {
            return (char)(i_RowNumber + 97);
        }

        public static char getColSymbolFromNumber(int i_ColNumber)
        {
            return (char)(i_ColNumber + 65);
        }

        public static int getRowNumberFromSymbol(char i_RowSymbol)
        {
            return i_RowSymbol - 97;
        }

        public static int getColNumberFromSymbol(char i_ColSymbol)
        {
            return i_ColSymbol - 65;
        }
    }
}




