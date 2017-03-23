using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEmulator
{
    public struct Move
    {

        public Move(Piece p, Point mt)
        {
            move = p;
            moveTo = mt;
        }
        public Piece move;
        public Point moveTo;
    }
    public abstract class Player
    {
        public Player(int SIDE)
        {
            side = SIDE;
        }
        public Player() { }

        public static Random rand = new Random();
        public string name;
        public int side = 1;
        public abstract Move computeMove(Board b);
    }

    /// <summary>
    /// Picks random moves
    /// </summary>
    public class Rando : Player
    {
        
        public Rando()
        {
            name = "Rando";
        }

        public Rando(int SIDE)
        {
            side = SIDE;
            name = "Rando";
        }

        public override Move computeMove(Board b)
        {
            List<Move> moves = b.getAllMoves(side, b);
            return moves[rand.Next(moves.Count)];
        }
    }

    /// <summary>
    /// Will pick a random move unless it can capture a piece.
    /// </summary>
    public class Killer : Player
    {
        public Killer()
        {
            name = "Killer";
        }

        public Killer(int SIDE)
        {
            side = SIDE;
            name = "Killer";
        }

        public override Move computeMove(Board b)
        {
            List<Move> moves = b.getAllMoves(side, b);
            Move bestMove = moves[rand.Next(moves.Count)];
            int bestVal = -1;
            foreach (Move mv in moves)
            {
                if (b.BoardCalculations[mv.moveTo.X, mv.moveTo.Y] != null)
                {
                    string name = b.BoardCalculations[mv.moveTo.X, mv.moveTo.Y].name;
                    if (b.BoardCalculations[mv.moveTo.X, mv.moveTo.Y].side != side)
                    {
                        int curVal = -1;
                        switch (name)
                        {
                            case "Queen":
                                curVal = 8;
                                break;
                            case "Bishop":
                                curVal = 6;
                                break;
                            case "Knight":
                                curVal = 4;
                                break;
                            case "Castle":
                                curVal = 2;
                                break;
                            case "Pawn":
                                curVal = 1;
                                break;
                        }
                        if(curVal > bestVal)
                        {
                            bestVal = curVal;
                            bestMove = mv;
                        }
                    }
                }
            }
            return bestMove;

        }
    }

    /// <summary>
    /// Attempts to get to a position where it can castle as fast as possible
    /// </summary>
    public class Castler : Player
    {
        public Castler()
        {
            name = "Castler";
        }

        public Castler(int SIDE)
        {
            side = SIDE;
            name = "Castler";
        }

        public override Move computeMove(Board b)
        {
            //Find all valid moves
            List<Move> moves = b.getAllMoves(side, b);

            int sideToClear = (side == -1 ? 7 : 0);

            Move mv = new Move();
            mv.moveTo = new Point(-1, -1);

            //If we can castle, then castle
            foreach(Move v in moves)
            {
                if(v.move.name == "King")
                {
                    if(b.BoardCalculations[v.moveTo.X, v.moveTo.Y] != null)
                    {
                        if (b.BoardCalculations[v.moveTo.X, v.moveTo.Y].name == "Castle")
                        {
                            return v;
                        }
                    }
                }
            }


            foreach(Move v in moves)
            {
                if (v.move.name != "Castle" && v.move.name != "King" && v.moveTo.Y != sideToClear && v.move.curPoint.Y == sideToClear)
                    mv = v;
            }
            if(mv.moveTo.X == -1)//If we found no move that removed itself from the home row move an unmoved pawn
            {
                foreach (Move v in moves)
                {
                    if (v.move.name != "Castle" && v.move.name != "King" && v.move.name == "Pawn" && v.moveTo.Y != sideToClear)
                    {
                        Pawn cp = (Pawn)v.move;
                        if(cp.unmoved)
                            mv = v;
                    }
                        
                }
            }

            if (mv.moveTo.X == -1)//This shouldn't ever happen
                mv = moves[0];
            return mv;
        }
    }
}
