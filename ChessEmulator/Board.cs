using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ChessEmulator
{
    public class Board
    {
        public static Image KING;
        public static Image QUEEN;
        public static Image BISHOP;
        public static Image KNIGHT;
        public static Image CASTLE;
        public static Image PAWN;
        public static Image BLANK;

        internal Emulator emulatorREF;

        //Used for collision detection -1 = non king black -2 = king black 1 = non king white 2 = king white
        public int[,] BoardState = new int[8, 8];

        //USed for display
        public Piece[,] BoardCalculations = new Piece[8, 8];

        public PictureBox[,] BoardPicture = new PictureBox[8, 8];

        public void InitializeBoardState(Image Blank, Image King, Image Queen, Image Bishop, Image Knight, Image Castle, Image Pawn, Emulator REF)
        {
            emulatorREF = REF;
            KING = King;
            QUEEN = Queen;
            BISHOP = Bishop;
            KNIGHT = Knight;
            CASTLE = Castle;
            PAWN = Pawn;
            BLANK = Blank;
            float tileSize = 50;
            float margin = 5;
            #region collision_board
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    BoardState[x, y] = 0;
                }
            }
            BoardState[0, 0] = 1;
            BoardState[1, 0] = 1;
            BoardState[2, 0] = 1;
            BoardState[3, 0] = 1;
            BoardState[4, 0] = 2;
            BoardState[5, 0] = 1;
            BoardState[6, 0] = 1;
            BoardState[7, 0] = 1;
            BoardState[0, 1] = 1;
            BoardState[1, 1] = 1;
            BoardState[2, 1] = 1;
            BoardState[3, 1] = 1;
            BoardState[4, 1] = 1;
            BoardState[5, 1] = 1;
            BoardState[6, 1] = 1;
            BoardState[7, 1] = 1;

            BoardState[0, 7] = -1;
            BoardState[1, 7] = -1;
            BoardState[2, 7] = -1;
            BoardState[3, 7] = -1;
            BoardState[4, 7] = -2;
            BoardState[5, 7] = -1;
            BoardState[6, 7] = -1;
            BoardState[7, 7] = -1;
            BoardState[0, 6] = -1;
            BoardState[1, 6] = -1;
            BoardState[2, 6] = -1;
            BoardState[3, 6] = -1;
            BoardState[4, 6] = -1;
            BoardState[5, 6] = -1;
            BoardState[6, 6] = -1;
            BoardState[7, 6] = -1;
            #endregion

            #region calculations_board
            BoardCalculations[0, 1] = new Pawn(1);
            BoardCalculations[1, 1] = new Pawn(1);
            BoardCalculations[2, 1] = new Pawn(1);
            BoardCalculations[3, 1] = new Pawn(1);
            BoardCalculations[4, 1] = new Pawn(1);
            BoardCalculations[5, 1] = new Pawn(1);
            BoardCalculations[6, 1] = new Pawn(1);
            BoardCalculations[7, 1] = new Pawn(1);

            BoardCalculations[7, 0] = new Castle(1);
            BoardCalculations[0, 0] = new Castle(1);

            BoardCalculations[6, 0] = new Knight(1);
            BoardCalculations[1, 0] = new Knight(1);

            BoardCalculations[5, 0] = new Bishop(1);
            BoardCalculations[2, 0] = new Bishop(1);

            BoardCalculations[4, 0] = new King(1);
            BoardCalculations[3, 0] = new Queen(1);

            BoardCalculations[0, 6] = new Pawn(-1);
            BoardCalculations[1, 6] = new Pawn(-1);
            BoardCalculations[2, 6] = new Pawn(-1);
            BoardCalculations[3, 6] = new Pawn(-1);
            BoardCalculations[4, 6] = new Pawn(-1);
            BoardCalculations[5, 6] = new Pawn(-1);
            BoardCalculations[6, 6] = new Pawn(-1);
            BoardCalculations[7, 6] = new Pawn(-1);

            BoardCalculations[7, 7] = new Castle(-1);
            BoardCalculations[0, 7] = new Castle(-1);

            BoardCalculations[6, 7] = new Knight(-1);
            BoardCalculations[1, 7] = new Knight(-1);

            BoardCalculations[5, 7] = new Bishop(-1);
            BoardCalculations[2, 7] = new Bishop(-1);

            BoardCalculations[4, 7] = new King(-1);
            BoardCalculations[3, 7] = new Queen(-1);

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if(BoardCalculations[x,y] != null)
                    {
                        BoardCalculations[x, y].curPoint = new Point(x, y);
                    }
                }
            }
            #endregion

            #region picture_board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BoardPicture[i, j] = new PictureBox();
                    BoardPicture[i, j].Image = Blank;
                    BoardPicture[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    BoardPicture[i, j].Location =
                        new Point((int)((7 - i) * (tileSize + margin)), (int)((7 - j) * (tileSize + margin)));
                    BoardPicture[i, j].Size = new Size((int)tileSize, (int)tileSize);
                    BoardPicture[i, j].Name = i + "," + j;
                    BoardPicture[i, j].MouseHover += new EventHandler(tile_hover);
                    BoardPicture[i, j].MouseLeave += new EventHandler(tile_leave);
                    BoardPicture[i, j].MouseClick += new MouseEventHandler(tile_select);
                }
            }
            //White pieces
            BoardPicture[0, 1].Image = Pawn;
            BoardPicture[1, 1].Image = Pawn;
            BoardPicture[2, 1].Image = Pawn;
            BoardPicture[3, 1].Image = Pawn;
            BoardPicture[4, 1].Image = Pawn;
            BoardPicture[5, 1].Image = Pawn;
            BoardPicture[6, 1].Image = Pawn;
            BoardPicture[7, 1].Image = Pawn;

            BoardPicture[7, 0].Image = Castle;
            BoardPicture[0, 0].Image = Castle;

            BoardPicture[6, 0].Image = Knight;
            BoardPicture[1, 0].Image = Knight;

            BoardPicture[5, 0].Image = Bishop;
            BoardPicture[2, 0].Image = Bishop;

            BoardPicture[4, 0].Image = King;
            BoardPicture[3, 0].Image = Queen;

            //Black Pieces
            BoardPicture[0, 6].Image = Emulator.InvertImage(Pawn);
            BoardPicture[1, 6].Image = Emulator.InvertImage(Pawn);
            BoardPicture[2, 6].Image = Emulator.InvertImage(Pawn);
            BoardPicture[3, 6].Image = Emulator.InvertImage(Pawn);
            BoardPicture[4, 6].Image = Emulator.InvertImage(Pawn);
            BoardPicture[5, 6].Image = Emulator.InvertImage(Pawn);
            BoardPicture[6, 6].Image = Emulator.InvertImage(Pawn);
            BoardPicture[7, 6].Image = Emulator.InvertImage(Pawn);

            BoardPicture[7, 7].Image = Emulator.InvertImage(Castle);
            BoardPicture[0, 7].Image = Emulator.InvertImage(Castle);

            BoardPicture[6, 7].Image = Emulator.InvertImage(Knight);
            BoardPicture[1, 7].Image = Emulator.InvertImage(Knight);

            BoardPicture[5, 7].Image = Emulator.InvertImage(Bishop);
            BoardPicture[2, 7].Image = Emulator.InvertImage(Bishop);

            BoardPicture[4, 7].Image = Emulator.InvertImage(King);
            BoardPicture[3, 7].Image = Emulator.InvertImage(Queen);
            #endregion

        }

        public Board Clone(Board b)
        {
            Board newB = new Board();
            newB.emulatorREF = emulatorREF;
            newB.BoardState = this.BoardState;
            newB.BoardPicture = this.BoardPicture;
            newB.BoardCalculations = this.BoardCalculations;

            return newB;
        }

        public static Point[] GetNeighbours(Point p, int size)
        {
            IEnumerable<Point> neighbours = from x in Enumerable.Range(p.X - 1, 3)
                                            from y in Enumerable.Range(p.Y - 1, 3)
                                            where x >= 0 && y >= 0 && x < size && y < size
                                            select new Point(x, y);

            return neighbours.ToArray();
        }

        public List<Piece> getPieces(int side)
        {
            List<Piece> pieces = new List<Piece>();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (this.BoardCalculations[x, y] != null)
                    {
                        if (this.BoardCalculations[x, y].side == side)
                        {
                            pieces.Add(BoardCalculations[x, y]);
                        }
                    }
                }
            }

            return pieces;
        }
        

        public List<Move> getAllMoves(int side, Board b)
        {
            List<Move> moves = new List<Move>();
            List<Piece> p = this.getPieces(side);

            foreach (Piece pc in p)
            {

                foreach (Point loc in pc.PotentialMoves(b))
                {
                    Move m;
                    m.move = pc;
                    m.moveTo = loc;
                    moves.Add(m);
                }

            }

            return moves;
        }

        /// <summary>
        /// Special get all moves check that has special handling for kings.
        /// Use this whenever instead of getAllMoves when you think that  
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public List<Move> getAllmostAllMoves(int side, Board b)
        {
            List<Move> moves = new List<Move>();
            List<Piece> p = this.getPieces(side);

            foreach (Piece pc in p)
            {
                if (pc.name == "King")
                //This was causing infinite loops as the king has to check if its move will result in its death, 
                //calling canBeKilled which calls getAllMoves which calls potentialMoves
                {
                    //TODO don't allow a king to move into another king's potential moves
                }
                else
                {
                    foreach (Point loc in pc.PotentialMoves(b))
                    {
                        Move m;
                        m.move = pc;
                        m.moveTo = loc;
                        moves.Add(m);
                    }
                }
            }

            return moves;
        }

        public bool canBeKilled(Point p, int side)
        {
            int otherSide = side == 1 ? -1 : 1;

            foreach(Move mv in getAllmostAllMoves(otherSide, this))
            {
                if (mv.moveTo == p)
                    return true;
            }
            return false;
        }

        public bool canKingBeKilled(int side, Board b)
        {
            int otherSide = side == 1 ? -1 : 1;

            Point p = new Point(0, 0);

            foreach(Piece pc in getPieces(side))
            {
                if(pc.name == "King")
                {
                    p = pc.curPoint;
                    break;
                }
            }

            foreach (Move mv in getAllmostAllMoves(otherSide, b))
            {
                if (mv.moveTo == p)
                    return true;
            }
            return false;
        }

        public bool willMoveSaveKing(Move mv)
        {
            if (!canKingBeKilled(mv.move.side, this))//King isn't in danger
                return true;

            int otherSide = mv.move.side == 1 ? -1 : 1;

            Point p = new Point(0, 0);

            foreach (Piece pc in getPieces(mv.move.side))
            {
                if (pc.name == "King")
                {
                    p = pc.curPoint;
                    break;
                }
            }
            Board duplicate = this.Clone(this);
            mv.move.Move(mv.moveTo, duplicate);

            return !canKingBeKilled(mv.move.side, duplicate);
           

        }

        #region userInput
        private void tile_hover(object sender, System.EventArgs e)
        {
            PictureBox b = sender as PictureBox;
            string[] s = b.Name.Split(',');
            int x = Convert.ToInt16(s[0]);
            int y = Convert.ToInt16(s[1]);
            if (BoardCalculations[x, y] != null)
                foreach (Point m in BoardCalculations[x, y].PotentialMoves(this))
                {
                    BoardPicture[m.X, m.Y].Image = Emulator.TintImageGreen(BoardPicture[m.X, m.Y].Image);
                }
        }

        private void tile_leave(object sender, System.EventArgs e)
        {
            PictureBox b = sender as PictureBox;
            if (b.Image == null)
                return;
            string[] s = b.Name.Split(',');
            int x = Convert.ToInt16(s[0]);
            int y = Convert.ToInt16(s[1]);

            if (BoardCalculations[x, y] != null && new Point(x,y) != selectedTile)
            {
                foreach (Point m in BoardCalculations[x, y].PotentialMoves(this))
                {
                    int X = m.X;
                    int Y = m.Y;
                    if (BoardCalculations[X, Y] == null)
                    {
                        BoardPicture[X, Y].Image = Board.BLANK;
                    }
                    else
                    {
                        Piece p = BoardCalculations[X, Y];
                        Image img = Board.PAWN;
                        switch (p.name)
                        {
                            case "King": img = Board.KING; break;
                            case "Queen": img = Board.QUEEN; break;
                            case "Bishop": img = Board.BISHOP; break;
                            case "Knight": img = Board.KNIGHT; break;
                            case "Castle": img = Board.CASTLE; break;
                            case "Pawn": img = Board.PAWN; break;
                        }
                        BoardPicture[X, Y].Image = (p.side == 1 ? img : Emulator.InvertImage(img));
                    }
                }
            }
        }


        Point selectedTile = new Point(-1,-1);
        private bool currentlyMoving = false;
        
        private void tile_select(object sender, System.EventArgs e)
        {
            PictureBox b = sender as PictureBox;
            string[] s = b.Name.Split(',');
            int x = Convert.ToInt16(s[0]);
            int y = Convert.ToInt16(s[1]);

            //Very first check to see if the player tried to complete a move
            if (new Point(x, y) != selectedTile
                && currentlyMoving
                && BoardCalculations[selectedTile.X, selectedTile.Y].PotentialMoves(this).Contains(new Point(x, y))
                && Emulator.playersTurn
                && BoardCalculations[selectedTile.X, selectedTile.Y].side == Emulator.curSide)
            {

                //Unhighlight selected move
                Piece p = BoardCalculations[selectedTile.X, selectedTile.Y];
                Image img = Board.PAWN;
                switch (p.name)
                {
                    case "King": img = Board.KING; break;
                    case "Queen": img = Board.QUEEN; break;
                    case "Bishop": img = Board.BISHOP; break;
                    case "Knight": img = Board.KNIGHT; break;
                    case "Castle": img = Board.CASTLE; break;
                    case "Pawn": img = Board.PAWN; break;
                }
                BoardPicture[selectedTile.X, selectedTile.Y].Image = (p.side == 1 ? img : Emulator.InvertImage(img));


                //Unhighlight potential moves
                foreach (Point m in BoardCalculations[selectedTile.X, selectedTile.Y].PotentialMoves(this))
                {
                    int X = m.X;
                    int Y = m.Y;
                    if (BoardCalculations[X, Y] == null)
                    {
                        BoardPicture[X, Y].Image = Board.BLANK;
                    }
                    else
                    {
                        p = BoardCalculations[X, Y];
                        img = Board.PAWN;
                        switch (p.name)
                        {
                            case "King": img = Board.KING; break;
                            case "Queen": img = Board.QUEEN; break;
                            case "Bishop": img = Board.BISHOP; break;
                            case "Knight": img = Board.KNIGHT; break;
                            case "Castle": img = Board.CASTLE; break;
                            case "Pawn": img = Board.PAWN; break;
                        }
                        BoardPicture[X, Y].Image = (p.side == 1 ? img : Emulator.InvertImage(img));
                    }
                }

                //Move selected piece
                BoardCalculations[selectedTile.X, selectedTile.Y].Move(new Point(x, y), this);
                emulatorREF.NextTurn();
                currentlyMoving = false;

                selectedTile = new Point(-1, -1);
                return;
            }


            if (BoardCalculations[x, y] != null)
            {
                //If the player was not currently making a move, but just selected a tile
                if (new Point(x, y) != selectedTile && !currentlyMoving)
                {
                    selectedTile = new Point(x, y);
                    BoardPicture[x, y].Image = Emulator.TintImageBlue(BoardPicture[x, y].Image);
                    foreach (Point m in BoardCalculations[x, y].PotentialMoves(this))
                    {
                        BoardPicture[m.X, m.Y].Image = Emulator.TintImageGreen(BoardPicture[m.X, m.Y].Image);
                    }

                    currentlyMoving = true;
                }
                //If the player selected a new tile that was not part of the potential moves change 
                //the selected tile to that tile and unhighlight the previous tiles
                else if (new Point(x, y) != selectedTile && currentlyMoving
                    && !BoardCalculations[selectedTile.X,selectedTile.Y].PotentialMoves(this).Contains(new Point(x,y)))
                {
                    BoardPicture[x, y].Image = Emulator.TintImageBlue(BoardPicture[x, y].Image);
                    foreach (Point m in BoardCalculations[x, y].PotentialMoves(this))
                    {
                        BoardPicture[m.X, m.Y].Image = Emulator.TintImageGreen(BoardPicture[m.X, m.Y].Image);
                    }

                    //Unhighlight selected move
                    Piece p = BoardCalculations[selectedTile.X, selectedTile.Y];
                    Image img = Board.PAWN;
                    switch (p.name)
                    {
                        case "King": img = Board.KING; break;
                        case "Queen": img = Board.QUEEN; break;
                        case "Bishop": img = Board.BISHOP; break;
                        case "Knight": img = Board.KNIGHT; break;
                        case "Castle": img = Board.CASTLE; break;
                        case "Pawn": img = Board.PAWN; break;
                    }
                    BoardPicture[selectedTile.X, selectedTile.Y].Image = (p.side == 1 ? img : Emulator.InvertImage(img));


                    //Unhighlight potential moves
                    foreach (Point m in BoardCalculations[selectedTile.X, selectedTile.Y].PotentialMoves(this))
                    {
                        int X = m.X;
                        int Y = m.Y;
                        if (BoardCalculations[X, Y] == null)
                        {
                            BoardPicture[X, Y].Image = Board.BLANK;
                        }
                        else
                        {
                            p = BoardCalculations[X, Y];
                            img = Board.PAWN;
                            switch (p.name)
                            {
                                case "King": img = Board.KING; break;
                                case "Queen": img = Board.QUEEN; break;
                                case "Bishop": img = Board.BISHOP; break;
                                case "Knight": img = Board.KNIGHT; break;
                                case "Castle": img = Board.CASTLE; break;
                                case "Pawn": img = Board.PAWN; break;
                            }
                            BoardPicture[X, Y].Image = (p.side == 1 ? img : Emulator.InvertImage(img));
                        }
                    }
                    selectedTile = new Point(x, y);
                }
                
                
            }
            else if(currentlyMoving)
            {
                
                //Unhighlight selected move
                Piece p = BoardCalculations[selectedTile.X, selectedTile.Y];
                Image img = Board.PAWN;
                switch (p.name)
                {
                    case "King": img = Board.KING; break;
                    case "Queen": img = Board.QUEEN; break;
                    case "Bishop": img = Board.BISHOP; break;
                    case "Knight": img = Board.KNIGHT; break;
                    case "Castle": img = Board.CASTLE; break;
                    case "Pawn": img = Board.PAWN; break;
                }
                BoardPicture[selectedTile.X, selectedTile.Y].Image = (p.side == 1 ? img : Emulator.InvertImage(img));
                

                //Unhighlight potential moves
                foreach (Point m in BoardCalculations[selectedTile.X, selectedTile.Y].PotentialMoves(this))
                {
                    int X = m.X;
                    int Y = m.Y;
                    if (BoardCalculations[X, Y] == null)
                    {
                        BoardPicture[X, Y].Image = Board.BLANK;
                    }
                    else
                    {
                        p = BoardCalculations[X, Y];
                        img = Board.PAWN;
                        switch (p.name)
                        {
                            case "King": img = Board.KING; break;
                            case "Queen": img = Board.QUEEN; break;
                            case "Bishop": img = Board.BISHOP; break;
                            case "Knight": img = Board.KNIGHT; break;
                            case "Castle": img = Board.CASTLE; break;
                            case "Pawn": img = Board.PAWN; break;
                        }
                        BoardPicture[X, Y].Image = (p.side == 1 ? img : Emulator.InvertImage(img));
                    }
                }
                selectedTile = new Point(-1, -1);
                currentlyMoving = false;
            }
        }
        #endregion userInput
    }
}
