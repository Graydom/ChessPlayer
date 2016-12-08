using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChessEmulator
{
    public class Board
    {
        //Used for collision detection -1 = non king black -2 = king black 1 = non king white 2 = king white
        public int[,] BoardState = new int[8, 8];

        //USed for display
        public Piece[,] BoardDisplay = new Piece[8, 8];


        public void InitializeBoardState()
        {
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
        }

        public static Point[] GetNeighbours(Point p, int size)
        {
            IEnumerable<Point> neighbours = from x in Enumerable.Range(p.X - 1, 3)
                         from y in Enumerable.Range(p.Y - 1, 3)
                         where x >= 0 && y >= 0 && x < size && y < size
                         select new Point ( x, y );

            return neighbours.ToArray();
        }
    }
}
