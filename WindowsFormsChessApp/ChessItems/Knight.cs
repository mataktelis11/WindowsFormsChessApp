using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsChessApp.ChessItems
{
    class Knight : ChessPiece
    {
        public Knight(Form form, ChessBoard c, bool color, Point position, int x, int y) : base(form, c, color, position,x,y)
        {
            if (color)
            {
                ImageLocation = ImageFolder + "knight.png";
                ImageName = "knight.png";
            }
            else
            {
                ImageLocation = ImageFolder + "knightw.png";
                ImageName = "knightw.png";
            }
        }


        protected override void moveMouseDown(object sender, MouseEventArgs e)
        {
            //clear the lists
            allowedPositions.Clear();
            canDestroyPositions.Clear();
            //create them
            //for this chesspiece they are the same


            Point[] points = {new Point(CurrentPosX + 2, CurrentPosY + 1) , new Point(CurrentPosX + 2, CurrentPosY - 1), new Point(CurrentPosX + 1, CurrentPosY + 2), new Point(CurrentPosX + 1, CurrentPosY - 2), new Point(CurrentPosX - 2, CurrentPosY + 1), new Point(CurrentPosX - 2, CurrentPosY - 1), new Point(CurrentPosX - 1, CurrentPosY + 2), new Point(CurrentPosX - 1, CurrentPosY - 2) };

            foreach(Point point in points)
            {
                try 
                { 
                    allowedPositions.Add(ChessBoard.Squares[point.X][point.Y]);
                    canDestroyPositions.Add(ChessBoard.Squares[point.X][point.Y]);
                }
                catch (IndexOutOfRangeException) { }
            }
            


            base.moveMouseDown(sender, e);
        }

        protected override void moveMouseUp(object sender, MouseEventArgs e)
        {
            base.moveMouseUp(sender, e);
        }

        protected override void moveMouse(object sender, MouseEventArgs e)
        {
            base.moveMouse(sender, e);
        }
    }
}
