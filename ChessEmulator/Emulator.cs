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

        public static bool playersTurn = true;
        public static int curSide = 1;
        public Player p1 = new Rando(1);
        public Player p2 = new Killer(-1);

        public Emulator()
        {
            InitializeComponent();

            //Create Picture Boxes
            createBoard();



            //Create players
            bool r = Player.rand.NextDouble() > .5;
            p1 = new Rando(r ? 1 : -1);
            p2 = new Killer(!r ? 1 : -1);

            name1.Text = p2.name;
            name2.Text = p1.name;
        }

        

        public void NextTurn()
        {
            //Increment current side
            curSide = (curSide >= 1 ? -1 : 1);


            //Draw check
            List<Move> moves = new List<Move>();
            List<Piece> p = b.getPieces(curSide);

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
            if(moves.Count <= 0)
            {
                infoBox.Text = "Draw";
                button1.Enabled = false;
            }

            p.Clear();
            p = (b.getPieces((curSide >= 1 ? -1 : 1)));
            moves.Clear();
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
            if (moves.Count <= 0)
            {
                infoBox.Text = "Draw";
                button1.Enabled = false;
            }

            //TODO put in victory check
            if(b.canKingBeKilled(-1,b))
            {
                foreach(Move mv in b.getAllMoves(-1, b))
                {
                    if (b.willMoveSaveKing(mv))
                        return;
                }
                playersTurn = false;
                infoBox.Text = "White victory";
                button1.Enabled = false;
            }
            else if(b.canKingBeKilled(1,b))
            {
                foreach (Move mv in b.getAllMoves(1, b))
                {
                    if (b.willMoveSaveKing(mv))
                        return;
                }
                playersTurn = false;
                infoBox.Text = "Black victory";
                button1.Enabled = false;
            }

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
            b.InitializeBoardState(blank, king, queen, bishop, knight, castle, pawn, this);

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

        public static Bitmap TintImageGreen(Image n)
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

        public static Bitmap TintImageBlue(Image n)
        {
            Bitmap pic = new Bitmap(n);
            for (int y = 0; (y <= (pic.Height - 1)); y++)
            {
                for (int x = 0; (x <= (pic.Width - 1)); x++)
                {
                    Color inv = pic.GetPixel(x, y);
                    inv = Color.FromArgb(255, inv.R, inv.G, 255);
                    pic.SetPixel(x, y, inv);
                }
            }
            return pic;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (curSide == 1)
            {
                Move c = p1.computeMove(b);
                c.move.Move(c.moveTo, b);
            }
            else
            {
                Move c = p2.computeMove(b);
                c.move.Move(c.moveTo, b);
            }
            NextTurn();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
