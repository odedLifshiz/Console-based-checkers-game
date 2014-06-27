namespace B13_Ex02
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum ePlayerType
    {
        computer,
        human
    }

    public class Player
    {
        private int m_Score;
        private String m_Name;
        private ePlayerType m_TypeOfPlayer;
        private eCellType m_PawnSymbol;
        private eCellType m_KingSymbol;
        private Boolean m_MovesDown;

        public Player(String i_name, ePlayerType i_TypeOfPlayer, int i_Score, eCellType i_PawnSymbol, eCellType i_KingSymbol, Boolean i_MovesDown)
        {
            this.m_Name = i_name;
            this.m_Score = i_Score;
            this.m_TypeOfPlayer = i_TypeOfPlayer;
            this.m_PawnSymbol = i_PawnSymbol;
            this.m_KingSymbol = i_KingSymbol;
            this.m_MovesDown = i_MovesDown;
        }

        public Boolean MovesDown
        {
            get
            {
                return this.m_MovesDown;
            }
            set
            {
                this.m_MovesDown = value;
            }
        }


        public eCellType KingSymbol
        {
            get
            {
                return this.m_KingSymbol;
            }
            set
            {
                this.m_KingSymbol = value;
            }
        }

        public eCellType PawnSymbol
        {
            get
            {
                return this.m_PawnSymbol;
            }
            set
            {
                this.m_PawnSymbol = value;
            }
        }
        public int Score
        {
            get
            {
                return this.m_Score;
            }
            set
            {
                this.m_Score = value;
            }
        }

        public ePlayerType TypeOfPlayer
        {
            get
            {
                return this.m_TypeOfPlayer;
            }
            set
            {
                this.m_TypeOfPlayer = value;
            }
        }

        public String Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                this.m_Name = value;
            }
        }
    }
}