using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsChessApp.ChessItems
{
    public class ChessBoard : Panel
    {

        public Tile[][] Squares { get; set; }       //the squares of the chessboard
        public string ImagePath { get; set; }       //directory of the image

        public ChessBoard(Form form,string imagePath)
        {
            this.ImagePath = imagePath;
            BackgroundImage = Image.FromFile(ImagePath + "chess_board.png");
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            Location = new System.Drawing.Point(45, 40);
            Name = "chessboard1";
            Size = new System.Drawing.Size(544, 544);

            form.Controls.Add(this);

            //initialize the boxes
            Squares = new Tile[8][];
            for (int i = 0; i < 8; i++)
            {
                Squares[i] = new Tile[8];
                for (int j = 0; j < 8; j++)
                {
                    Squares[i][j] = new Tile();

                    Squares[i][j].BackColor = Color.White;

                    Squares[i][j].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

                    Squares[i][j].Location = new System.Drawing.Point(j * 68, i * 68);

                    Squares[i][j].Name = "square" + (i + j).ToString();
                    Squares[i][j].Size = new System.Drawing.Size(68, 68);
                    this.Controls.Add(Squares[i][j]);
                    Squares[i][j].Parent = this;
                    Squares[i][j].Visible = false;

                }
            }
        }

        //used to re-assign the BackgroundImage
        public void resetImage(string imagePath)
        {
            this.ImagePath = imagePath;
            BackgroundImage = Image.FromFile(ImagePath + "chess_board.png");
        }

    }
}
