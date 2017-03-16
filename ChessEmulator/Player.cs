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
        public Piece move;
        public Point moveTo;
    }
    public abstract class Player
    {
        public static Random rand = new Random();
        public string name;
        public int side = 1;
        public abstract Move computeMove(Board b);
    }

    public class Rando : Player
    {
        public Rando(int SIDE)
        {
            side = SIDE;
        }

        public Rando() { }
        public override Move computeMove(Board b)
        {
            List<Move> moves = new List<Move>();
            List<Piece> p = b.getPieces(side);

            foreach(Piece pc in p)
            {
                foreach(Point loc in pc.PotentialMoves(b))
                {
                    Move m;
                    m.move = pc;
                    m.moveTo = loc;
                    moves.Add(m);
                }
            }
            return moves[rand.Next(moves.Count)];
        }
    }
}
