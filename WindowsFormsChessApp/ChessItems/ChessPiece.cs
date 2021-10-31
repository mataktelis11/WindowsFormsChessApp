using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsChessApp.ChessItems
{
    public class ChessPiece : PictureBox
    {
        protected Form Form { get; }    //reference to form1
        static public string ImageFolder { set; get; }  //directory of the image//all chess pieces have the same folder
        protected string ImageName { set; get; }    //name of the image (used for changing the style)
        public bool ColorBW { get; }  //color of the chesspiece (true:black    false:white)
        protected bool Movement { get; set; }           //bool: true if the mouse is pressed (for drag and drop movement)
        protected Point InitialPosition { get; set; }   //position before it changes (by drag and drop movement)
        protected Point MousePosition { get; set; }     //position of the mouse (for drag and drop movement)
        protected ChessBoard ChessBoard { get; set; }   //reference to the chessboard
        public int CurrentPosX { get; set; }    //current coordinates of the position on the chessboard
        public int CurrentPosY { get; set; }    //
        public int StartingPosX { get; set; }   //coordinates of the position when the game starts
        public int StartingPosY { get; set; }   //

        protected List<Tile> allowedPositions = new List<Tile>();       //tiles the chesspiece can go
        protected List<Tile> canDestroyPositions = new List<Tile>();    //tiles the chesspiece can threaten

        public bool FirstMove { get; set; } = true; //true if the chesspiece hasnt moved at all

        public ChessPiece(Form form, ChessBoard parent,bool color, Point position,int posX,int posY)
        {
            this.Form = form;           
            this.ColorBW = color;

            Location = position;

            this.CurrentPosX = posX;
            this.CurrentPosY = posY;

            this.StartingPosX = posX;
            this.StartingPosY = posY;

            Size = new System.Drawing.Size(68, 68);
            SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            Form.Controls.Add(this);
            Parent = parent;
            BackColor = System.Drawing.Color.Transparent;
            Movement = false;
            ChessBoard = parent;

            this.MouseUp += new System.Windows.Forms.MouseEventHandler(moveMouseUp);    //add the events for the movement
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(moveMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(moveMouse);

            ChessBoard.Squares[CurrentPosX][CurrentPosY].IsEmpty = false;   //position X,Y is no longer empty
            ChessBoard.Squares[CurrentPosX][CurrentPosY].Pointer = this;    //tyle points to his chesspiece
            //maybe not needed
            this.BringToFront();

        }

        //re-assign the image
        public void reloadImage() 
        {
            ImageLocation = ImageFolder + ImageName;
        }

        //returns the distance between 2 points
        protected double distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        //returns the center point of a control
        protected Point center(Control c)
        {
            return new Point(c.Width / 2 + c.Location.X, c.Height / 2 + c.Location.Y);
        }

        //'event' method : when you hold down a chesspiece with the mouse
        protected virtual void moveMouseDown(object sender, MouseEventArgs e)
        {
            //if it is in a FlowLayoutPanel : return
            if (Parent.GetType() == typeof(FlowLayoutPanel))
            {
                this.Enabled = false;
            }

            ChessPiece p = (ChessPiece)sender;
            p.Movement = true;    //enable the movement
            p.MousePosition = e.Location;     //get the location of the mouse 
            p.InitialPosition = Location;     // save the current location of the chesspiece
            p.BringToFront();    //bring the chess piece up so it is visible

            //color allowedPositions with green
            foreach (Tile tile in allowedPositions)
            {

                tile.BackColor = Color.Green;
                tile.Visible = true;

            }

            //color canDestroyPositions with red only if they contain a chesspiece of opposite color
            foreach (Tile tile in canDestroyPositions)
            {
                if (!tile.IsEmpty)
                {
                    if (tile.Pointer.ColorBW != p.ColorBW)
                        tile.Pointer.BackColor = Color.Red;
                }
            }

        }

        //'event' method : when you let the mouse 
        protected virtual void moveMouseUp(object sender, MouseEventArgs e)
        {

            string initialPositionNote = new Point(CurrentPosX,CurrentPosY).ToString(); //save string with initial position for the history stringbuilder

            ChessPiece p = (ChessPiece)sender;
            p.Movement = false;

            //first go through all the boxes , see which one is closest to the chesspiece
            bool match = false;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //chessPieces[i].BackColor = Color.Transparent;   
                    ChessBoard.Squares[i][j].Visible = false;                    //hide all the boxes
                    //if you are close to square i,j
                    if (distance(center(this), center(ChessBoard.Squares[i][j])) <= 30)
                    {

                        if (ChessBoard.Squares[i][j].IsEmpty)
                        {
                            //if you are allowed to go there
                            if (allowedPositions.Contains(ChessBoard.Squares[i][j]))
                            {
                                
                                this.Location = ChessBoard.Squares[i][j].Location;  //set the new location
                                ChessBoard.Squares[CurrentPosX][CurrentPosY].IsEmpty = true;    //old square is now empty
                                ChessBoard.Squares[CurrentPosX][CurrentPosY].Pointer = null;    //old square is pointer is now null
                                CurrentPosX = i;    //set the new location coordinates
                                CurrentPosY = j;
                                ChessBoard.Squares[CurrentPosX][CurrentPosY].IsEmpty = false;   //new square is not empty
                                ChessBoard.Squares[CurrentPosX][CurrentPosY].Pointer = this;    //update square pointer
                                match = true;   //bool: match found
                                FirstMove = false;
                                ((Form1)Form).moveMade(this);   //call movemade function of form1

                                //append line to stringbuilder for history
                                ((Form1)Form).Sb.Append("Moved ");
                                if(ColorBW)
                                    ((Form1)Form).Sb.Append("black ");
                                else
                                    ((Form1)Form).Sb.Append("white ");
                                ((Form1)Form).Sb.Append(this.GetType().Name.ToString());
                                ((Form1)Form).Sb.Append(" from ");
                                ((Form1)Form).Sb.Append(initialPositionNote);
                                ((Form1)Form).Sb.Append(" to ");
                                ((Form1)Form).Sb.Append(new Point(CurrentPosX, CurrentPosY).ToString());
                                ((Form1)Form).Sb.Append("\n");
                            }

                        }
                        else
                        {
                            //if tyle is not empty and is in canDestroyPositions
                            if (canDestroyPositions.Contains(ChessBoard.Squares[i][j])) 
                            {
                                //if the chesspiece in this tyle is of the same color, continue
                                if (this.ColorBW == (ChessBoard.Squares[i][j].Pointer).ColorBW)
                                    continue;

                                //else

                                ChessBoard.Squares[i][j].Pointer.BackColor = Color.Transparent; //it was colored red -> set it back to transparent

                                this.Location = ChessBoard.Squares[i][j].Location;  //get ans set the location
                                ChessBoard.Controls.Remove(ChessBoard.Squares[i][j].Pointer);   //remove the enemy from the chessboard
                                (ChessBoard.Squares[i][j].Pointer).Size = new Size(30, 30);     //make enemy smaller
                                //add enemy to the correct flowLayoutPanel
                                if ((ChessBoard.Squares[i][j].Pointer).ColorBW)
                                    ((Form1)Form).flowLayoutPanel1.Controls.Add(ChessBoard.Squares[i][j].Pointer);
                                else
                                    ((Form1)Form).flowLayoutPanel2.Controls.Add(ChessBoard.Squares[i][j].Pointer);

                                (ChessBoard.Squares[i][j].Pointer).Enabled = false; //deactivate the enemy
                                ChessBoard.Squares[CurrentPosX][CurrentPosY].IsEmpty = true;    //old position of the current chesspiece is empty now
                                ChessBoard.Squares[CurrentPosX][CurrentPosY].Pointer = null;
                                CurrentPosX = i;    //get new coordinates
                                CurrentPosY = j;
                                ChessBoard.Squares[CurrentPosX][CurrentPosY].Pointer = this;    //tyle now points to the current chesspiece
                                match = true;
                                FirstMove = false;
                                ((Form1)Form).moveMade(this);   //call movemade function of form1

                                //append line to stringbuilder for history
                                ((Form1)Form).Sb.Append("Moved ");
                                if (ColorBW)
                                    ((Form1)Form).Sb.Append("black ");
                                else
                                    ((Form1)Form).Sb.Append("white ");
                                ((Form1)Form).Sb.Append(this.GetType().Name.ToString());
                                ((Form1)Form).Sb.Append(" from ");
                                ((Form1)Form).Sb.Append(initialPositionNote);
                                ((Form1)Form).Sb.Append(" to ");
                                ((Form1)Form).Sb.Append(new Point(CurrentPosX, CurrentPosY).ToString());
                                ((Form1)Form).Sb.Append("\n");
                            }
                        }

                       
                    }

                }
            }
            //if no match was found, return to original position
            if (!match)
            {
                this.Location = this.InitialPosition;
                //return;
            }
            //enemy pieces canDestroyPositions were colored red -> set them back to transparent
            foreach (Tile tile in canDestroyPositions)
            {
                if(!tile.IsEmpty)
                    tile.Pointer.BackColor = Color.Transparent;
            }
           
        }

        //event 'move' for the chesspiece
        protected virtual void moveMouse(object sender, MouseEventArgs e)
        {
            //if it is in a FlowLayoutPanel : disable the chesspiece
            if (Parent.GetType() == typeof(FlowLayoutPanel))
            {
                this.Enabled = false;
            }

            ChessPiece p = (ChessPiece)sender;
            if (p.Movement)
            {
                p.Location = new Point(e.X + p.Location.X - p.MousePosition.X, e.Y + p.Location.Y - p.MousePosition.Y);

                //change color the boxes that are close
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (distance(center(this), center(ChessBoard.Squares[i][j])) <= 30)
                        {
                            ChessBoard.Squares[i][j].BackColor = Color.Green;
                            ChessBoard.Squares[i][j].Visible = true;
                        }

                        else
                            ChessBoard.Squares[i][j].Visible = false;
                    }
                }

                //for all in allowedPositions
                foreach (Tile tile in allowedPositions)
                {
                    //if it is close : paint to green
                    if (distance(center(this), center(tile)) <= 30)
                    {
                        tile.BackColor = Color.Green;
                        tile.Visible = true;
                    }
                    else
                    {
                        tile.BackColor = Color.DarkGreen;
                        tile.Visible = true;
                    }
                }
            }
        }

        //method used to set the chesspiece to its original position for the game to start
        public virtual void reset()
        {
            this.Parent = ChessBoard;
            this.Location = ChessBoard.Squares[StartingPosX][StartingPosY].Location;    //location set as the starting location
            this.Size = new System.Drawing.Size(68, 68);
            CurrentPosX = StartingPosX; //get new coordintates
            CurrentPosY = StartingPosY;
            ChessBoard.Squares[CurrentPosX][CurrentPosY].IsEmpty = false;   //its current position now is not empty
            ChessBoard.Squares[CurrentPosX][CurrentPosY].Pointer = this;
            this.Enabled = true;

            FirstMove = true;
            BackColor = Color.Transparent;
        }

    }
}
