using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsChessApp.ChessItems
{
    class Bishop : ChessPiece
    {
        public Bishop(Form form, ChessBoard c, bool color, Point position, int x, int y) : base(form , c, color,position,x,y)
        {
            if (color)
            {
                ImageLocation = ImageFolder + "bishop.png";
                ImageName = "bishop.png";
            }
            else
            {
                ImageLocation = ImageFolder + "bishopw.png";
                ImageName = "bishopw.png";
            }
        }


        protected override void moveMouseDown(object sender, MouseEventArgs e)
        {
            //clear the lists
            allowedPositions.Clear();
            canDestroyPositions.Clear();
            //create them
            //for this chesspiece they are the same

            //create 4 lists of coordinates
            List<Point> points1 = new List<Point>();
            List<Point> points2 = new List<Point>();
            List<Point> points3 = new List<Point>();
            List<Point> points4 = new List<Point>();

            //add the arrays to a list
            List<List<Point>> pointsAll = new List<List<Point>>();

            pointsAll.Add(points1);
            pointsAll.Add(points2);
            pointsAll.Add(points3);
            pointsAll.Add(points4);

            if (CurrentPosX >= 0 && CurrentPosY >= 0)
            {
                for (int i = 1; i < 8; i++)
                {
                    points1.Add(new Point(CurrentPosX - i, CurrentPosY - i));
                    if (CurrentPosX - i == 0 || CurrentPosY - i == 0)
                        break;
                }
            }
            
            if (CurrentPosX <= 7 && CurrentPosY <= 7)
            {
                for (int i = 1; i < 8; i++)
                {
                    points2.Add(new Point(CurrentPosX + i, CurrentPosY + i));
                    if (CurrentPosX + i == 7 || CurrentPosY + i ==7 )
                        break;
                }
            }

            if (CurrentPosY <= 7)
            {
                for (int i = 1; i < 8; i++)
                {
                    points3.Add(new Point(CurrentPosX - i, CurrentPosY + i));
                    if (CurrentPosY + i == 7)
                        break;
                }
            }

            if (CurrentPosX <= 7)
            {
                for (int i = 1; i < 8; i++)
                {
                    points4.Add(new Point(CurrentPosX + i, CurrentPosY - i));
                    if (CurrentPosX + i == 7)
                        break;
                }
            }



            //add to allowedPositions and canDestroyPositions only if they are valid coordinates
            foreach (List<Point> pointsList in pointsAll)
            {
                foreach (Point point in pointsList)
                {
                    try
                    {
                        if (ChessBoard.Squares[point.X][point.Y].IsEmpty)
                        {
                            allowedPositions.Add(ChessBoard.Squares[point.X][point.Y]);
                            canDestroyPositions.Add(ChessBoard.Squares[point.X][point.Y]);
                        }
                        else if (ChessBoard.Squares[point.X][point.Y].Pointer.ColorBW != ColorBW)
                        {
                            allowedPositions.Add(ChessBoard.Squares[point.X][point.Y]);
                            canDestroyPositions.Add(ChessBoard.Squares[point.X][point.Y]);
                            break;
                        }
                        else
                            break;
                    }
                    catch (IndexOutOfRangeException) { }
                }
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
