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

        public abstract Point[] PotentialMoves(Board b, bool checkKing);

        public abstract Point[] PotentialMoves(Point p, Board b);//This probably isn't necessary

        public virtual bool Move(Point p, Board b)
        {

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

        public static bool sameSide(int s1, int s2)
        {
            return (s1 > 0 && s2 > 0) || (s1 < 0 && s2 < 0);
        }

    }

    public class Pawn : Piece
    {
        public bool unmoved = true;
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

        public override Point[] PotentialMoves(Board b, bool checkForKing)
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

            if (checkForKing)
            {
                List<Point> validMoves = new List<Point>();
                foreach (Point mv in moves)
                {
                    if (b.willMoveSaveKing(new Move(this, mv)))
                        validMoves.Add(mv);
                }

                return validMoves.Distinct().ToArray();
            }

            int movesCount = moves.Count;
            for (int i = 0; i < movesCount; i++)
            {
                if (moves[i].Y == (side == -1 ? 0 : 7))
                {
                    moves.Add(new Point(moves[i].X + 100, moves[i].Y));
                    moves.Add(new Point(moves[i].X + 100, moves[i].Y));
                    moves.Add(new Point(moves[i].X + 100, moves[i].Y));
                    moves.Add(new Point(moves[i].X + 100, moves[i].Y));
                    moves.RemoveAt(i);
                }
            }

            return moves.Distinct().ToArray();
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


            //Promotion
            if (curPoint.Y == (side == -1 ? 0 : 7))
            {
                moves.Add(new Point(-100, side));//Convert to bishop
                moves.Add(new Point(-200, side));//Convert to rook
                moves.Add(new Point(-300, side));//Convert to knight
                moves.Add(new Point(-400, side));//Convert to queen
            }

            int movesCount = moves.Count;
            for(int i = 0; i < movesCount;i++)
            {
                if(moves[i].Y == (side == -1 ? 0 : 7))
                {
                    moves.Add(new Point(moves[i].X + 100, moves[i].Y));
                    moves.Add(new Point(moves[i].X + 100, moves[i].Y));
                    moves.Add(new Point(moves[i].X + 100, moves[i].Y));
                    moves.Add(new Point(moves[i].X + 100, moves[i].Y));
                }
            }

            //TODO En Passente
            return moves.Distinct().ToArray();
        }

        public override Point[] PotentialMoves(Point p, Board b)
        {
            throw new NotImplementedException();
        }

        public override bool Move(Point p, Board b)
        {
            int px = p.X;
            px = (int)Math.Floor((double)px / 100) * 100;
            //Promotion
            if (px >= 100)
            {
                Point loc = new Point(p.X - px, p.Y);
                Piece promote = new Pawn(side);
                switch (p.X)
                {
                    case 100:
                        promote = new Bishop(side);
                        break;
                    case 200:
                        promote = new Castle(side);
                        break;
                    case 300:
                        promote = new Knight(side);
                        break;
                    case 400:
                        promote = new Queen(side);
                        break;

                }
                b.BoardCalculations[loc.X, loc.Y] = promote;

                b.BoardPicture[loc.X, loc.Y].Image = Board.BLANK;

                Image img = Board.PAWN;
                switch (promote.name)
                {
                    case "King": img = Board.KING; break;
                    case "Queen": img = Board.QUEEN; break;
                    case "Bishop": img = Board.BISHOP; break;
                    case "Knight": img = Board.KNIGHT; break;
                    case "Castle": img = Board.CASTLE; break;
                    case "Pawn": img = Board.PAWN; break;
                }
                b.BoardPicture[loc.X, loc.Y].Image = (side == 1 ? img : Emulator.InvertImage(img));

                b.BoardCalculations[curPoint.X, curPoint.Y] = null;
                b.BoardPicture[curPoint.X, curPoint.Y].Image = Board.BLANK;
                b.BoardState[curPoint.X, curPoint.Y] = 0;

                return true;
            }
            else
            {
                unmoved = false;
                return base.Move(p, b);
            }
        }
    }
    public class King : Piece
    {
        public bool unmoved = true;
        public King()
        {
            ID = curID++;
            name = "King";
            side = 1;
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



            List<Point> n = new List<Point>();
            n.Add(new Point(curPoint.X + 1, curPoint.Y));
            n.Add(new Point(curPoint.X - 1, curPoint.Y));
            n.Add(new Point(curPoint.X, curPoint.Y + 1));
            n.Add(new Point(curPoint.X, curPoint.Y - 1));
            n.Add(new Point(curPoint.X + 1, curPoint.Y + 1));
            n.Add(new Point(curPoint.X - 1, curPoint.Y + 1));
            n.Add(new Point(curPoint.X + 1, curPoint.Y - 1));
            n.Add(new Point(curPoint.X - 1, curPoint.Y - 1));


            foreach (Point p in n)
            {
                if (p.X >= 0 && p.X <= 7 && p.Y >= 0 && p.Y <= 7)
                {
                    //Make sure we aren't moving onto a friendly or moving into a place we can be captured
                    if (!sameSide(b.BoardState[p.X, p.Y], side) 
                     && !b.canBeKilled(p, side))
                    {
                        moves.Add(p);
                    }
                }
            }

            //Castling

            List<Castle> castles = new List<Castle>();
            foreach (Piece pc in b.getPieces(side))
            {
                if (pc.name == "Castle")
                {
                    Castle c = (Castle)pc;
                    if(c.unmoved)
                        castles.Add((Castle)pc);
                }
            }
           
            foreach(Castle c in castles)
            {
                int dir = 1;
                if (curPoint.X - c.curPoint.X > 0)
                    dir = -1;
                
                int sy = side == -1 ? 7 : 0;

                if(dir == 1)
                {
                    //King can't move through or onto any point where it can be captured
                    if (b.BoardState[5, sy] == 0
                     && !b.canBeKilled(new Point(5, sy), side) 
                     && b.BoardState[6, sy] == 0 
                     && !b.canBeKilled(new Point(6, sy), side))
                        moves.Add(c.curPoint);
                }
                else if(dir == -1)
                {
                    //King can't move through or onto any point where it can be captured
                    if (b.BoardState[1, sy] == 0 
                     && !b.canBeKilled(new Point(1, sy), side) 
                     && b.BoardState[2, sy] == 0 
                     && !b.canBeKilled(new Point(2,sy), side) 
                     && b.BoardState[3, sy] == 0
                     && !b.canBeKilled(new Point(3, sy), side))
                        moves.Add(c.curPoint);
                }
                
            }

            List<Point> validMoves = new List<Point>();
            foreach (Point mv in moves)
            {
                if (b.willMoveSaveKing(new Move(this, mv)))
                    validMoves.Add(mv);
            }

            return validMoves.Distinct().ToArray();
        }

        public override Point[] PotentialMoves(Board b, bool checkForKing)
        {
            List<Point> moves = new List<Point>();



            List<Point> n = new List<Point>();
            n.Add(new Point(curPoint.X + 1, curPoint.Y));
            n.Add(new Point(curPoint.X - 1, curPoint.Y));
            n.Add(new Point(curPoint.X, curPoint.Y + 1));
            n.Add(new Point(curPoint.X, curPoint.Y - 1));
            n.Add(new Point(curPoint.X + 1, curPoint.Y + 1));
            n.Add(new Point(curPoint.X - 1, curPoint.Y + 1));
            n.Add(new Point(curPoint.X + 1, curPoint.Y - 1));
            n.Add(new Point(curPoint.X - 1, curPoint.Y - 1));


            foreach (Point p in n)
            {
                if (p.X >= 0 && p.X <= 7 && p.Y >= 0 && p.Y <= 7)
                {
                    //Make sure we aren't moving onto a friendly or moving into a place we can be captured
                    if (!sameSide(b.BoardState[p.X, p.Y], side)
                        && (checkForKing ? !b.canBeKilled(p, side) : true))
                    {
                        moves.Add(p);
                    }
                }
            }

            //Castling

            List<Castle> castles = new List<Castle>();
            foreach (Piece pc in b.getPieces(side))
            {
                if (pc.name == "Castle")
                {
                    Castle c = (Castle)pc;
                    if (c.unmoved)
                        castles.Add((Castle)pc);
                }
            }

            foreach (Castle c in castles)
            {
                int dir = 1;
                if (curPoint.X - c.curPoint.X > 0)
                    dir = -1;

                int sy = side == -1 ? 7 : 0;

                if (dir == 1)
                {

                }
                else if (dir == -1)
                {

                }

            }

            return moves.Distinct().ToArray();
        }
        public override Point[] PotentialMoves(Point p, Board b)
        {
            throw new NotImplementedException();
        }

        public override bool Move(Point p, Board b)
        {
            unmoved = false;

            //Check if move was a castling move
            if(b.BoardCalculations[p.X,p.Y] != null)//First make sure it isn't null
            {
                if(b.BoardCalculations[p.X,p.Y].name == "Castle" && sameSide(b.BoardCalculations[p.X,p.Y].side, side))
                        //If the move location is a castle on the same side as the king, castle
                {
                    int bot = side == 1 ? 0 : 7;

                    int dir = 1;
                    if (curPoint.X - p.X > 0)
                        dir = -1;

                    int kx = 0;//King x pos
                    int cx = 0;//Castle x pos

                    if (dir == 1)
                    {
                        kx = 5;
                        cx = 6;
                    }
                    else
                    {
                        kx = 2;
                        cx = 3;
                    }

                    return b.BoardCalculations[curPoint.X, curPoint.Y].Move(new Point(cx, bot), b) &&  b.BoardCalculations[p.X, p.Y].Move(new Point(kx, bot), b);
                }
            }

            return base.Move(p, b);
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

        public override Point[] PotentialMoves(Board b, bool checkForKing)
        {
            List<Point> moves = new List<Point>();

            List<Point> orth = new List<Point>();
            List<Point> dirs = new List<Point>();
            dirs.Add(new Point(1, 0));
            dirs.Add(new Point(1, -1));
            dirs.Add(new Point(1, 1));

            dirs.Add(new Point(-1, 0));
            dirs.Add(new Point(-1, -1));
            dirs.Add(new Point(-1, 1));

            dirs.Add(new Point(0, 1));
            dirs.Add(new Point(0, -1));

            foreach(Point p in dirs)
            {
                Point cur = curPoint;
                while (true)
                {
                    cur.X += p.X;
                    cur.Y += p.Y;
                    if (cur.X >= 0 && cur.X <= 7 && cur.Y >= 0 && cur.Y <= 7)
                    {
                        if (b.BoardState[cur.X, cur.Y] == 0)
                            orth.Add(new Point(cur.X, cur.Y));
                        else if (!sameSide(b.BoardState[cur.X, cur.Y], side))
                        {
                            orth.Add(new Point(cur.X, cur.Y));
                            break;
                        }
                        else
                            break;
                    }
                    else
                        break;
                } 
            }

            foreach (Point p in orth)
            {
                if (p.X >= 0 && p.X <= 7 && p.Y >= 0 && p.Y <= 7)//This shouldn't matter
                {
                    if (!sameSide(b.BoardState[p.X, p.Y], side))
                    {
                        moves.Add(p);
                    }
                }
            }
            return moves.Distinct().ToArray();
        }

        public override Point[] PotentialMoves(Board b)
        {
            List<Point> moves = new List<Point>();

            List<Point> orth = new List<Point>();
            List<Point> dirs = new List<Point>();
            dirs.Add(new Point(1, 0));
            dirs.Add(new Point(1, -1));
            dirs.Add(new Point(1, 1));

            dirs.Add(new Point(-1, 0));
            dirs.Add(new Point(-1, -1));
            dirs.Add(new Point(-1, 1));

            dirs.Add(new Point(0, 1));
            dirs.Add(new Point(0, -1));

            foreach (Point p in dirs)
            {
                Point cur = curPoint;
                while (true)
                {
                    cur.X += p.X;
                    cur.Y += p.Y;
                    if (cur.X >= 0 && cur.X <= 7 && cur.Y >= 0 && cur.Y <= 7)
                    {
                        if (b.BoardState[cur.X, cur.Y] == 0)
                            orth.Add(new Point(cur.X, cur.Y));
                        else if (!sameSide(b.BoardState[cur.X, cur.Y], side))
                        {
                            orth.Add(new Point(cur.X, cur.Y));
                            break;
                        }
                        else
                            break;
                    }
                    else
                        break;
                }
            }

            foreach (Point p in orth)
            {
                if (p.X >= 0 && p.X <= 7 && p.Y >= 0 && p.Y <= 7)//This shouldn't matter
                {
                    if (!sameSide(b.BoardState[p.X, p.Y], side))
                    {
                        moves.Add(p);
                    }
                }
            }


            return moves.Distinct().ToArray();
        }

        public override Point[] PotentialMoves(Point p, Board b)
        {
            throw new NotImplementedException();
        }
    }

    public class Bishop : Piece
    {
        public override Point[] PotentialMoves(Point b, Board p)
        {
            throw new NotImplementedException();
        }

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

            List<Point> orth = new List<Point>();
            List<Point> dirs = new List<Point>();

            dirs.Add(new Point(1, -1));
            dirs.Add(new Point(1, 1));

            dirs.Add(new Point(-1, -1));
            dirs.Add(new Point(-1, 1));


            foreach (Point p in dirs)
            {
                Point cur = curPoint;
                while (true)
                {
                    cur.X += p.X;
                    cur.Y += p.Y;
                    if (cur.X >= 0 && cur.X <= 7 && cur.Y >= 0 && cur.Y <= 7)
                    {
                        if (b.BoardState[cur.X, cur.Y] == 0)
                            orth.Add(new Point(cur.X, cur.Y));
                        else if (!sameSide(b.BoardState[cur.X, cur.Y], side))
                        {
                            orth.Add(new Point(cur.X, cur.Y));
                            break;
                        }
                        else
                            break;
                    }
                    else
                        break;
                } 
            }

            foreach (Point p in orth)
            {
                if (p.X >= 0 && p.X <= 7 && p.Y >= 0 && p.Y <= 7)//This shouldn't matter
                {
                    if (!sameSide(b.BoardState[p.X, p.Y], side))
                    {
                        moves.Add(p);
                    }
                }
            }


            return moves.Distinct().ToArray(); ;
        }
        public override Point[] PotentialMoves(Board b, bool checkForKing)
        {
            List<Point> moves = new List<Point>();

            List<Point> orth = new List<Point>();
            List<Point> dirs = new List<Point>();

            dirs.Add(new Point(1, -1));
            dirs.Add(new Point(1, 1));

            dirs.Add(new Point(-1, -1));
            dirs.Add(new Point(-1, 1));


            foreach (Point p in dirs)
            {
                Point cur = curPoint;
                while (true)
                {
                    cur.X += p.X;
                    cur.Y += p.Y;
                    if (cur.X >= 0 && cur.X <= 7 && cur.Y >= 0 && cur.Y <= 7)
                    {
                        if (b.BoardState[cur.X, cur.Y] == 0)
                            orth.Add(new Point(cur.X, cur.Y));
                        else if (!sameSide(b.BoardState[cur.X, cur.Y], side))
                        {
                            orth.Add(new Point(cur.X, cur.Y));
                            break;
                        }
                        else
                            break;
                    }
                    else
                        break;
                }
            }

            foreach (Point p in orth)
            {
                if (p.X >= 0 && p.X <= 7 && p.Y >= 0 && p.Y <= 7)//This shouldn't matter
                {
                    if (!sameSide(b.BoardState[p.X, p.Y], side))
                    {
                        moves.Add(p);
                    }
                }
            }


            return moves.Distinct().ToArray();
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

            List<Point> n = new List<Point>();
            n.Add(new Point(curPoint.X + 2, curPoint.Y + 1));
            n.Add(new Point(curPoint.X + 2, curPoint.Y - 1));
            n.Add(new Point(curPoint.X - 2, curPoint.Y + 1));
            n.Add(new Point(curPoint.X - 2, curPoint.Y - 1));

            n.Add(new Point(curPoint.X - 1, curPoint.Y - 2));
            n.Add(new Point(curPoint.X + 1, curPoint.Y - 2));
            n.Add(new Point(curPoint.X - 1, curPoint.Y + 2));
            n.Add(new Point(curPoint.X + 1, curPoint.Y + 2));

            foreach(Point p in n)
            {
                if (p.X >= 0 && p.X <= 7 && p.Y >= 0 && p.Y <= 7)
                {
                    if (!sameSide(b.BoardState[p.X, p.Y], side))
                    {
                        moves.Add(p);
                    }
                }
            }

            return moves.Distinct().ToArray();
        }
        public override Point[] PotentialMoves(Board b, bool checkForKing)
        {
            List<Point> moves = new List<Point>();

            List<Point> n = new List<Point>();
            n.Add(new Point(curPoint.X + 2, curPoint.Y + 1));
            n.Add(new Point(curPoint.X + 2, curPoint.Y - 1));
            n.Add(new Point(curPoint.X - 2, curPoint.Y + 1));
            n.Add(new Point(curPoint.X - 2, curPoint.Y - 1));

            n.Add(new Point(curPoint.X - 1, curPoint.Y - 2));
            n.Add(new Point(curPoint.X + 1, curPoint.Y - 2));
            n.Add(new Point(curPoint.X - 1, curPoint.Y + 2));
            n.Add(new Point(curPoint.X + 1, curPoint.Y + 2));

            foreach (Point p in n)
            {
                if (p.X >= 0 && p.X <= 7 && p.Y >= 0 && p.Y <= 7)
                {
                    if (!sameSide(b.BoardState[p.X, p.Y], side))
                    {
                        moves.Add(p);
                    }
                }
            }

            return moves.Distinct().ToArray();
        }

        public override Point[] PotentialMoves(Point b, Board p)
        {
            throw new NotImplementedException();
        }
    }

    public class Castle : Piece
    {
        public override Point[] PotentialMoves(Point b, Board p)
        {
            throw new NotImplementedException();
        }

        public bool unmoved = true;
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

            List<Point> orth = new List<Point>();

            List<Point> dirs = new List<Point>();
            dirs.Add(new Point(1, 0));
            dirs.Add(new Point(-1, 0));
            dirs.Add(new Point(0, 1));
            dirs.Add(new Point(0, -1));

            foreach (Point p in dirs)
            {
                Point cur = curPoint;
                while (true)
                {
                    cur.X += p.X;
                    cur.Y += p.Y;
                    if (cur.X >= 0 && cur.X <= 7 && cur.Y >= 0 && cur.Y <= 7)
                    {
                        if (b.BoardState[cur.X, cur.Y] == 0)
                            orth.Add(new Point(cur.X, cur.Y));
                        else if (!sameSide(b.BoardState[cur.X, cur.Y], side))
                        {
                            orth.Add(new Point(cur.X, cur.Y));
                            break;
                        }
                        else
                            break;
                    }
                    else
                        break;
                } 
                

            }

            foreach (Point p in orth)
            {
                if (p.X >= 0 && p.X <= 7 && p.Y >= 0 && p.Y <= 7)//This shouldn't matter
                {
                    if (!sameSide(b.BoardState[p.X, p.Y], side))
                    {
                        moves.Add(p);
                    }
                }
            }


            return moves.Distinct().ToArray();
        }
        public override Point[] PotentialMoves(Board b, bool checkForKing)
        {
            List<Point> moves = new List<Point>();

            List<Point> orth = new List<Point>();

            List<Point> dirs = new List<Point>();
            dirs.Add(new Point(1, 0));
            dirs.Add(new Point(-1, 0));
            dirs.Add(new Point(0, 1));
            dirs.Add(new Point(0, -1));

            foreach (Point p in dirs)
            {
                Point cur = curPoint;
                while (true)
                {
                    cur.X += p.X;
                    cur.Y += p.Y;
                    if (cur.X >= 0 && cur.X <= 7 && cur.Y >= 0 && cur.Y <= 7)
                    {
                        if (b.BoardState[cur.X, cur.Y] == 0)
                            orth.Add(new Point(cur.X, cur.Y));
                        else if (!sameSide(b.BoardState[cur.X, cur.Y], side))
                        {
                            orth.Add(new Point(cur.X, cur.Y));
                            break;
                        }
                        else
                            break;
                    }
                    else
                        break;
                }


            }

            foreach (Point p in orth)
            {
                if (p.X >= 0 && p.X <= 7 && p.Y >= 0 && p.Y <= 7)//This shouldn't matter
                {
                    if (!sameSide(b.BoardState[p.X, p.Y], side))
                    {
                        moves.Add(p);
                    }
                }
            }

            return moves.Distinct().ToArray();
        }


        public override bool Move(Point p, Board b)
        {
            unmoved = false;
            return base.Move(p, b);
        }

    }

   
}
