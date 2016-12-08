using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChessEmulator.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(){}
        public Pawn(Point p, int s)
        {
            curPoint = p;
            name = s==0?"Black":"White"+" Pawn";
            isAlive = true;
            side = s;
        }

        public override Point[] PotentialMoves(Board b)
        {
            throw new NotImplementedException();
        }

        public override Point[] PotentialMoves(Point p, Board b)
        {
            throw new NotImplementedException();
        }
    }
}
