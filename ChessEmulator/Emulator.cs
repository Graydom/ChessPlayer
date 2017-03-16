using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ChessEmulator
{
    public partial class Emulator : Form
    {
        internal Board b;
        public Emulator()
        {
            InitializeComponent();

            //Create Picture Boxes
            createBoard();
        }

        int curSide = 1;
        Player p1 = new Rando(1);
        Player p2 = new Rando(-1);

        private void NextTurn()
        {
            if(curSide == -1)
            {
                Move c = p1.computeMove(b);
                c.move.Move(c.moveTo, b);
            }
            else
            {
                Move c = p2.computeMove(b);
                c.move.Move(c.moveTo, b);
            }
            curSide = (curSide >= 1 ? -1 : 1);

            //TODO put in victory check
        }

        private void createBoard()
        {
            Image pawn   = ChessEmulator.Properties.Resources.Pawn;
            Image king   =  ChessEmulator.Properties.Resources.King;
            Image queen  = ChessEmulator.Properties.Resources.Queen;
            Image bishop = ChessEmulator.Properties.Resources.Bishop;
            Image knight = ChessEmulator.Properties.Resources.Knight;
            Image castle = ChessEmulator.Properties.Resources.Castle;
            Image blank  = ChessEmulator.Properties.Resources.Blank;
            b = new Board();
            b.InitializeBoardState(blank, king, queen, bishop, knight, castle, pawn);

            for(int i = 0; i < 8;i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    this.Controls.Add(b.BoardPicture[i,j]);
                }

            }
        }

        public static Bitmap InvertImage(Image n)
        {
            Bitmap pic = new Bitmap(n);
            for (int y = 0; (y <= (pic.Height - 1)); y++)
            {
                for (int x = 0; (x <= (pic.Width - 1)); x++)
                {
                    Color inv = pic.GetPixel(x, y);
                    inv = Color.FromArgb(255, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                    pic.SetPixel(x, y, inv);
                }
            }
            return pic;
        }

        public static Bitmap TintImage(Image n, float greenTint)
        {
            Bitmap pic = new Bitmap(n);
            for (int y = 0; (y <= (pic.Height - 1)); y++)
            {
                for (int x = 0; (x <= (pic.Width - 1)); x++)
                {
                    Color inv = pic.GetPixel(x, y);
                    inv = Color.FromArgb(255, inv.R, 255, inv.B);
                    pic.SetPixel(x, y, inv);
                }
            }
            return pic;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            NextTurn();
        }
    }
}
