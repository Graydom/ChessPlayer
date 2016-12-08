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
        public int side;
        public string name;
        public bool isAlive;
        public Point curPoint;

        public abstract Point[] PotentialMoves(Board b);

        public abstract Point[] PotentialMoves(Point p, Board b);

        public bool Move(Point p, Board b)
        {

            if (PotentialMoves(b).Contains(p))
            {
                b.BoardState[p.X, p.Y] = side == 0 ? -1 : 1;
                b.BoardDisplay[p.X, p.Y] = this;
                return true;
            }
            
            return false;
        }

    }
}
