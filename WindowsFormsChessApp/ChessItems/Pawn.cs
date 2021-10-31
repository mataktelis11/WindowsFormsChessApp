using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsChessApp.ChessItems
{
    public class Pawn : ChessPiece
    {

        //private bool FirstMove = true;

        public Pawn(Form form, ChessBoard c, bool color, Point position, int x, int y) : base(form, c, color,position,x,y)
        {
            if (color)
            {
                ImageLocation = ImageFolder + "pawn.png";
                ImageName = "pawn.png";
            }
            else
            {
                ImageLocation = ImageFolder + "pawnw.png";
                ImageName = "pawnw.png";
            }
        }


        protected override void moveMouseDown(object sender, MouseEventArgs e)
        {

            allowedPositions.Clear();
            canDestroyPositions.Clear();

            if (ColorBW)
            {
                if (FirstMove)
                {
                    allowedPositions.Add(ChessBoard.Squares[CurrentPosX - 1][CurrentPosY]);
                    allowedPositions.Add(ChessBoard.Squares[CurrentPosX - 2][CurrentPosY]);

                    if ((CurrentPosY - 1) >= 0)
                        canDestroyPositions.Add(ChessBoard.Squares[CurrentPosX - 1][CurrentPosY - 1]);
                    if (7 >= (CurrentPosY + 1))
                        canDestroyPositions.Add(ChessBoard.Squares[CurrentPosX - 1][CurrentPosY + 1]);
                    
                }
                else
                {
                    allowedPositions.Add(ChessBoard.Squares[CurrentPosX - 1][CurrentPosY]);
                    if ((CurrentPosY - 1) >= 0)
                        canDestroyPositions.Add(ChessBoard.Squares[CurrentPosX - 1][CurrentPosY - 1]);
                    if (7 >= (CurrentPosY + 1))
                        canDestroyPositions.Add(ChessBoard.Squares[CurrentPosX - 1][CurrentPosY + 1]);
                    
                }

            }
            else
            {

                if (FirstMove)
                {
                    allowedPositions.Add(ChessBoard.Squares[CurrentPosX + 1][CurrentPosY]);
                    allowedPositions.Add(ChessBoard.Squares[CurrentPosX + 2][CurrentPosY]);
                    if ((CurrentPosY - 1) >= 0)
                        canDestroyPositions.Add(ChessBoard.Squares[CurrentPosX + 1][CurrentPosY - 1]);
                    if (7 >= (CurrentPosY + 1))
                        canDestroyPositions.Add(ChessBoard.Squares[CurrentPosX + 1][CurrentPosY + 1]);
                }
                else
                {
                    allowedPositions.Add(ChessBoard.Squares[CurrentPosX + 1][CurrentPosY]);
                    if ((CurrentPosY - 1) >= 0)
                        canDestroyPositions.Add(ChessBoard.Squares[CurrentPosX + 1][CurrentPosY - 1]);
                    if (7 >= (CurrentPosY + 1))
                        canDestroyPositions.Add(ChessBoard.Squares[CurrentPosX + 1][CurrentPosY + 1]);
                }


            }



            base.moveMouseDown(sender, e);
        }

        protected override void moveMouseUp(object sender, MouseEventArgs e)
        {          
            base.moveMouseUp(sender, e);
            if (this.ColorBW)
            {
                for(int i = 0; i < 8; i++)
                {
                    //if it is black and reached the end
                    if (this.Location == ChessBoard.Squares[0][i].Location)
                    {
                        ((Form1)Form).pawntransform(this,i);
                        return;
                    }
                        
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    //if it is white and reached the end
                    if (this.Location == ChessBoard.Squares[7][i].Location)
                    {
                        ((Form1)Form).pawntransform(this,i);
                        return;
                    }
                        
                }
            }
            //FirstMove = false;
        }

        protected override void moveMouse(object sender, MouseEventArgs e)
        {
            base.moveMouse(sender, e);         
        }


    }
}
