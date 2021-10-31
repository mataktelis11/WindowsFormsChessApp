using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsChessApp.ChessItems
{
    class King : ChessPiece
    {
        public King(Form form, ChessBoard c, bool color, Point position, int x, int y) : base(form, c, color, position,x,y)
        {
            if (color)
            {
                ImageLocation = ImageFolder + "king.png";
                ImageName = "king.png";
            }
            else
            {
                ImageLocation = ImageFolder + "kingw.png";
                ImageName = "kingw.png";
            }
        }


        protected override void moveMouseDown(object sender, MouseEventArgs e)
        {

            //clear the lists
            allowedPositions.Clear();
            canDestroyPositions.Clear();
            //create them
            //for this chesspiece they are the same

            List<Point> points = new List<Point>();
            points.Add(new Point(CurrentPosX - 1 ,CurrentPosY - 1));
            points.Add(new Point(CurrentPosX - 1, CurrentPosY));
            points.Add(new Point(CurrentPosX - 1, CurrentPosY + 1));

            points.Add(new Point(CurrentPosX, CurrentPosY + 1));
            points.Add(new Point(CurrentPosX, CurrentPosY - 1));

            points.Add(new Point(CurrentPosX + 1, CurrentPosY - 1));
            points.Add(new Point(CurrentPosX + 1, CurrentPosY));
            points.Add(new Point(CurrentPosX + 1, CurrentPosY + 1));
            


            foreach (Point point in points)
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
