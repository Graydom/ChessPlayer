using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChessEmulator
{
    public abstract class Piece
    {
        /// <summary>
        /// 1 = white, -1 = black
        /// </summary>
        public int side;

        /// <summary>
        /// Name of the piece   
        /// </summary>
        public string name;
        public bool isAlive;
        public Point curPoint;
        public int ID;

        public static int curID = 0;

        public abstract Point[] PotentialMoves(Board b);

        public abstract Point[] PotentialMoves(Point p, Board b);//This probably isn't necessary

        public virtual bool Move(Point p, Board b)
        {
            if (b.BoardState[p.X, p.Y] == side)
                Console.WriteLine("Captured a " + b.BoardCalculations[p.X, p.Y].name);

            int isKing = 1;
            if (name == "King")
                isKing = 2;
            b.BoardState[curPoint.X, curPoint.Y] = 0;
            b.BoardState[p.X, p.Y] = side == -1 ? (-isKing) : (isKing);


            b.BoardCalculations[curPoint.X, curPoint.Y] = null;
            b.BoardCalculations[p.X, p.Y] = this;
            b.BoardPicture[curPoint.X, curPoint.Y].Image = Board.BLANK;

            Image img = Board.PAWN;
            switch (name)
            {
                case "King": img = Board.KING; break;
                case "Queen": img = Board.QUEEN; break;
                case "Bishop": img = Board.BISHOP; break;
                case "Knight": img = Board.KNIGHT; break;
                case "Castle": img = Board.CASTLE; break;
                case "Pawn": img = Board.PAWN; break;
            }
            b.BoardPicture[p.X, p.Y].Image = (side == 1 ? img : Emulator.InvertImage(img));

            curPoint = p;
            return true;


        }

    }

    public class Pawn : Piece
    {
        bool unmoved = true;
        public Pawn()
        {
            ID = curID++;
            name = "Pawn";
            side = 0;
            curPoint = new Point(0, 0);
            isAlive = true;
        }

        public Pawn(int SIDE)
        {
            ID = curID++;
            name = "Pawn";
            side = SIDE;
            curPoint = new Point(0, 0);
            isAlive = true;
        }

        public override Point[] PotentialMoves(Board b)
        {
            List<Point> moves = new List<Point>();
            int dir = (side == -1 ? 1 : -1);

            //Initial moves
            if (unmoved)
            {
                if (b.BoardState[curPoint.X, curPoint.Y - (dir)] == 0)
                {
                    moves.Add(new Point(curPoint.X, curPoint.Y - (dir)));
                }
                if (b.BoardState[curPoint.X, curPoint.Y - (2 * dir)] == 0 && b.BoardState[curPoint.X, curPoint.Y - (dir)] == 0)
                {
                    moves.Add(new Point(curPoint.X, curPoint.Y - (2 * dir)));
                }
            }

            //Normal move
            if (curPoint.Y != (side == -1 ? 0 : 7))
                if (b.BoardState[curPoint.X, curPoint.Y - (dir)] == 0)
                    moves.Add(new Point(curPoint.X, curPoint.Y - (dir)));

            //Capturing move
            if (curPoint.Y != (side == -1 ? 0 : 7) && curPoint.X != 7)//Check to make sure it is not on an edge
                if (b.BoardState[curPoint.X + 1, curPoint.Y - dir] == -side)
                    moves.Add(new Point(curPoint.X + 1, curPoint.Y - dir));

            if (curPoint.Y != (side == -1 ? 0 : 7) && curPoint.X != 0)
                if (b.BoardState[curPoint.X - 1, curPoint.Y - dir] == -side)
                    moves.Add(new Point(curPoint.X - 1, curPoint.Y - dir));

            //TODO En Passente


            return moves.Distinct().ToArray();
        }
        public override Point[] PotentialMoves(Point p, Board b)
        {
            throw new NotImplementedException();
        }

        public override bool Move(Point p, Board b)
        {
            unmoved = false;
            return base.Move(p, b);
        }
    }
    public class King : Piece
    {
        public King()
        {
            ID = curID++;
            name = "King";
            side = 0;
            curPoint = new Point(0, 0);
            isAlive = true;
        }

        public King(int SIDE)
        {
            ID = curID++;
            name = "King";
            side = SIDE;
            curPoint = new Point(0, 0);
            isAlive = true;
        }

        public override Point[] PotentialMoves(Board b)
        {
            List<Point> moves = new List<Point>();

            return moves.Distinct().ToArray();
        }
        public override Point[] PotentialMoves(Point p, Board b)
        {
            throw new NotImplementedException();
        }
    }

    public class Queen : Piece
    {
        public Queen()
        {
            ID = curID++;
            name = "Queen";
            side = 0;
            curPoint = new Point(0, 0);
            isAlive = true;
        }

        public Queen(int SIDE)
        {
            ID = curID++;
            name = "Queen";
            side = SIDE;
            curPoint = new Point(0, 0);
            isAlive = true;
        }

        public override Point[] PotentialMoves(Board b)
        {
            List<Point> moves = new List<Point>();

            return moves.Distinct().ToArray();
        }
        public override Point[] PotentialMoves(Point p, Board b)
        {
            throw new NotImplementedException();
        }
    }

    public class Bishop : Piece
    {
        public Bishop()
        {
            ID = curID++;
            name = "Bishop";
            side = 0;
            curPoint = new Point(0, 0);
            isAlive = true;
        }

        public Bishop(int SIDE)
        {
            ID = curID++;
            name = "Bishop";
            side = SIDE;
            curPoint = new Point(0, 0);
            isAlive = true;
        }

        public override Point[] PotentialMoves(Board b)
        {
            List<Point> moves = new List<Point>();

            return moves.Distinct().ToArray();
        }
        public override Point[] PotentialMoves(Point p, Board b)
        {
            throw new NotImplementedException();
        }


    }

    public class Knight : Piece
    {
        public Knight()
        {
            ID = curID++;
            name = "Knight";
            side = 0;
            curPoint = new Point(0, 0);
            isAlive = true;
        }

        public Knight(int SIDE)
        {
            ID = curID++;
            name = "Knight";
            side = SIDE;
            curPoint = new Point(0, 0);
            isAlive = true;
        }

        public override Point[] PotentialMoves(Board b)
        {
            List<Point> moves = new List<Point>();

            return moves.Distinct().ToArray();
        }
        public override Point[] PotentialMoves(Point p, Board b)
        {
            throw new NotImplementedException();
        }
    }

    public class Castle : Piece
    {
        public Castle()
        {
            ID = curID++;
            name = "Castle";
            side = 0;
            curPoint = new Point(0, 0);
            isAlive = true;
        }

        public Castle(int SIDE)
        {
            ID = curID++;
            name = "Castle";
            side = SIDE;
            curPoint = new Point(0, 0);
            isAlive = true;
        }

        public override Point[] PotentialMoves(Board b)
        {
            List<Point> moves = new List<Point>();

            return moves.Distinct().ToArray();
        }
        public override Point[] PotentialMoves(Point p, Board b)
        {
            throw new NotImplementedException();
        }
    }
}
