using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsChessApp.ChessItems;

namespace WindowsFormsChessApp
{
    public partial class Form1 : Form
    {
        string currentArtStyle;

        public ChessPiece[] chesspiecesW = new ChessPiece[16];  //array of white pieces
        public ChessPiece[] chesspiecesB = new ChessPiece[16];  //array of black pieces
        List<Tomp> graveyard = new List<Tomp>();   //array of tomps of the 'dead' pawns that transformed
        List<ChessPiece> newChessPieces = new List<ChessPiece>();   //array of newly added pieces(of any color) that 'transformed' from a pawn

        ChessBoard chessBoard;

        public TimeSpan ts1;    //countdown of white team
        public TimeSpan ts2;    //countdown of black team

        //indicators
        bool gameOver = false;
        bool ongoingGame = false;
        public DateTime Date { get; set; }  //timestamp for the game
        public StringBuilder Sb { get; set; }   //stringbuilder for the history
        public TimeSpan GivenTime { get; set; } //given time for each player//assigned by form3//only used for the database 

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer3.Enabled = true;//enable timer for date and time display
            Sb = new StringBuilder();
            
            chessBoard = new ChessBoard(this, "chess_set1/");   // create a chessboard

            //create chesspieces:

            //pawns
            for (int i = 0; i < 8; i++)
            {
                chesspiecesW[i] = new Pawn(this, chessBoard, false, chessBoard.Squares[1][i].Location,1,i);
                chesspiecesB[i] = new Pawn(this, chessBoard, true, chessBoard.Squares[6][i].Location,6,i);
            }
            //whites
            chesspiecesW[8] = new Rook(this, chessBoard, false, chessBoard.Squares[0][0].Location,0,0);
            chesspiecesW[9] = new Knight(this, chessBoard, false, chessBoard.Squares[0][1].Location,0,1);
            chesspiecesW[10] = new Bishop(this, chessBoard, false, chessBoard.Squares[0][2].Location,0,2);
            chesspiecesW[11] = new King(this, chessBoard, false, chessBoard.Squares[0][3].Location,0,3);
            chesspiecesW[12] = new Queen(this, chessBoard, false, chessBoard.Squares[0][4].Location,0,4);
            chesspiecesW[13] = new Bishop(this, chessBoard, false, chessBoard.Squares[0][5].Location,0,5);
            chesspiecesW[14] = new Knight(this, chessBoard, false, chessBoard.Squares[0][6].Location,0,6);
            chesspiecesW[15] = new Rook(this, chessBoard, false, chessBoard.Squares[0][7].Location,0,7);
            //blacks
            chesspiecesB[8] = new Rook(this, chessBoard, true, chessBoard.Squares[7][0].Location,7,0);
            chesspiecesB[9] = new Knight(this, chessBoard, true, chessBoard.Squares[7][1].Location,7,1);
            chesspiecesB[10] = new Bishop(this, chessBoard, true, chessBoard.Squares[7][2].Location,7,2);
            chesspiecesB[11] = new King(this, chessBoard, true, chessBoard.Squares[7][3].Location,7,3);
            chesspiecesB[12] = new Queen(this, chessBoard, true, chessBoard.Squares[7][4].Location,7,4);
            chesspiecesB[13] = new Bishop(this, chessBoard, true, chessBoard.Squares[7][5].Location,7,5);
            chesspiecesB[14] = new Knight(this, chessBoard, true, chessBoard.Squares[7][6].Location,7,6);
            chesspiecesB[15] = new Rook(this, chessBoard, true, chessBoard.Squares[7][7].Location,7,7);

            woodenToolStripMenuItem.PerformClick(); //set the theme

            //create labels around the chessboard
            char[] letters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

            for (int i = 0; i < 8; i++)
            {
                this.Controls.Add(
                    new Label()
                    {
                        AutoSize = true,
                        Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        Location = new System.Drawing.Point(20, 60 + i * 68),
                        Size = new System.Drawing.Size(31, 33),
                        Text = (8 - i).ToString(),
                        Visible = true
                    });

                this.Controls.Add(
                    new Label()
                    {
                        AutoSize = true,
                        Font = new System.Drawing.Font("Stencil", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        Location = new System.Drawing.Point(70 + i * 68, 585),
                        Size = new System.Drawing.Size(31, 33),
                        Text = letters[i].ToString(),
                        Visible = true
                    });
            }

            //initialize and display the timers
            ts1 = new TimeSpan();            
            label1.Text = ts1.ToString();
            ts2 = new TimeSpan();            
            label5.Text = ts2.ToString();

            //chesspieces are invisible in the begining
            foreach (ChessPiece chessPiece1 in chesspiecesB)
            {
                chessPiece1.Visible = false;
            }
            foreach (ChessPiece chessPiece1 in chesspiecesW)
            {
                chessPiece1.Visible = false;
            }

        }

        //countdown timer of white team
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ts1.TotalMilliseconds == 0)
            {
                timer1.Enabled = false;               
                postScreen(false);//game ends : black wins                
            }
            else
            {
                ts1 = ts1.Subtract(TimeSpan.FromSeconds(1));
                label1.Text = ts1.ToString();
            }
        }

        //countdown timer of black team
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (ts2.TotalMilliseconds == 0)
            {
                timer2.Enabled = false;                
                postScreen(true);//game ends : white wins
            }
            else
            {
                ts2 = ts2.Subtract(TimeSpan.FromSeconds(1));
                label5.Text = ts2.ToString();
            }
        }

        //timer3 only displays date and time on the game
        private void timer3_Tick(object sender, EventArgs e)
        {
            label7.Text = DateTime.Now.ToString("HH:mm");
            label8.Text = DateTime.Now.ToString("ss");
            label9.Text = DateTime.Now.ToString("MM/dd/yyyy");
            label10.Text = DateTime.Now.ToString("dddd");
        }


        //method to 'transform' pawn that reached the other side (called from a Pawn-type)
        //argeuments : pawn (the pawn), i(position (Y coordinate))
        public void pawntransform(Pawn pawn,int i)
        {
            if (gameOver)
                return;

            //create form2 so the user chooses the new piece / form2 will the call pawntransformStep2
            Form2 form2 = new Form2(currentArtStyle,pawn.ColorBW,this,pawn,i);
            form2.Show();
            this.Enabled = false;
        }

        //method to complete the 'transformation' of the pawn (called from Form2)
        //argeuments : pawn (the pawn), i(position (Y)), choice("rook", "queen", "bishop","knight")
        public void pawntransformStep2(Pawn pawn, int i,string choice)
        {
            //write to History
            Sb.Append("Transformed ");
            if(pawn.ColorBW)
                Sb.Append("black pawn at ");
            else
                Sb.Append("whitepawn at ");
            Sb.Append(new Point(pawn.CurrentPosX, pawn.CurrentPosY).ToString());
            Sb.Append(" into a ");

            switch (choice)
            {
                case "rook":
                    Sb.Append("rook\n");//write to History
                    if (pawn.ColorBW)   //if it is black
                    {
                        int index = Array.IndexOf(chesspiecesB, (ChessPiece)pawn);  //get index in array
                        chessBoard.Controls.Remove(pawn);   //remove from chessboard
                        graveyard.Add(new Tomp(index, pawn));  //create a tomp and add it to the graveyard
                        newChessPieces.Add(new Rook(this, chessBoard, true, chessBoard.Squares[0][i].Location, 0, i));  //create a new chesspiece, add it to the 'newChessPieces' / location is [0,i]
                    }
                    else    //if it is white
                    {
                        int index = Array.IndexOf(chesspiecesW, (ChessPiece)pawn);
                        chessBoard.Controls.Remove(pawn);
                        graveyard.Add(new Tomp(index, pawn));
                        newChessPieces.Add(new Rook(this, chessBoard, false, chessBoard.Squares[7][i].Location, 7, i)); // location is [7,i]
                    }
                    break;
                case "queen":
                    Sb.Append("queen\n");
                    if (pawn.ColorBW)
                    {
                        int index = Array.IndexOf(chesspiecesB, (ChessPiece)pawn);
                        chessBoard.Controls.Remove(pawn);
                        graveyard.Add(new Tomp(index, pawn));
                        newChessPieces.Add(new Queen(this, chessBoard, true, chessBoard.Squares[0][i].Location, 0, i));
                    }
                    else
                    {
                        int index = Array.IndexOf(chesspiecesW, (ChessPiece)pawn);
                        chessBoard.Controls.Remove(pawn);
                        graveyard.Add(new Tomp(index, pawn));
                        newChessPieces.Add(new Queen(this, chessBoard, false, chessBoard.Squares[7][i].Location, 7, i));
                    }
                    break;
                case "knight":
                    Sb.Append("knight\n");
                    if (pawn.ColorBW)
                    {
                        int index = Array.IndexOf(chesspiecesB, (ChessPiece)pawn);
                        chessBoard.Controls.Remove(pawn);
                        graveyard.Add(new Tomp(index, pawn));
                        newChessPieces.Add(new Knight(this, chessBoard, true, chessBoard.Squares[0][i].Location, 0, i));
                    }
                    else
                    {
                        int index = Array.IndexOf(chesspiecesW, (ChessPiece)pawn);
                        chessBoard.Controls.Remove(pawn);
                        graveyard.Add(new Tomp(index, pawn));
                        newChessPieces.Add(new Knight(this, chessBoard, false, chessBoard.Squares[7][i].Location, 7, i));
                    }
                    break;
                case "bishop":
                    Sb.Append("bishop\n");
                    if (pawn.ColorBW)
                    {
                        int index = Array.IndexOf(chesspiecesB, (ChessPiece)pawn);
                        chessBoard.Controls.Remove(pawn);
                        graveyard.Add(new Tomp(index, pawn));
                        newChessPieces.Add(new Bishop(this, chessBoard, true, chessBoard.Squares[0][i].Location, 0, i));
                    }
                    else
                    {
                        int index = Array.IndexOf(chesspiecesW, (ChessPiece)pawn);
                        chessBoard.Controls.Remove(pawn);
                        graveyard.Add(new Tomp(index, pawn));
                        newChessPieces.Add(new Bishop(this, chessBoard, false, chessBoard.Squares[7][i].Location, 7, i));
                    }
                    break;
            }


            if (pawn.ColorBW)
            {
                foreach (ChessPiece chessPiece1 in newChessPieces)
                {
                    if (chessPiece1.ColorBW)
                        chessPiece1.Enabled = false;
                    else
                        chessPiece1.Enabled = true;
                }
            }
            else
            {

                foreach (ChessPiece chessPiece1 in newChessPieces)
                {
                    if (chessPiece1.ColorBW)
                        chessPiece1.Enabled = true;
                    else
                        chessPiece1.Enabled = false;
                }
            }

        }

        //changes active timer and active chesspiece
        private void changeTimers()
        {
            if (timer2.Enabled && ongoingGame)
            {
                timer1.Enabled = true;
                timer2.Enabled = false;

                foreach (ChessPiece chessPiece1 in chesspiecesB)
                {
                    chessPiece1.Enabled = false;
                }
                foreach (ChessPiece chessPiece1 in chesspiecesW)
                {
                    chessPiece1.Enabled = true;
                }
                foreach (ChessPiece chessPiece1 in newChessPieces)
                {
                    if (chessPiece1.ColorBW)
                        chessPiece1.Enabled = false;
                    else
                        chessPiece1.Enabled = true;
                }
            }
            else if (timer1.Enabled && ongoingGame)
            {
                timer2.Enabled = true;
                timer1.Enabled = false;

                foreach (ChessPiece chessPiece1 in chesspiecesW)
                {
                    chessPiece1.Enabled = false;
                }
                foreach (ChessPiece chessPiece1 in chesspiecesB)
                {
                    chessPiece1.Enabled = true;
                }
                foreach (ChessPiece chessPiece1 in newChessPieces)
                {
                    if (chessPiece1.ColorBW)
                        chessPiece1.Enabled = true;
                    else
                        chessPiece1.Enabled = false;
                }
            }
        }


        //method called when move was made
        public void moveMade(ChessPiece chessPiece)
        {
            SoundPlayer player = new SoundPlayer("sounds/effect.wav");
            player.Play();

            //MessageBox.Show("move was made");
            if (chessPiece.ColorBW)
            {
                //if white king is out
                if (flowLayoutPanel2.Contains(chesspiecesW[11]))
                {
                    //MessageBox.Show("game over : White king is dead");
                    postScreen(false);
                }
            }
            else
            {
                //if black king is out
                if (flowLayoutPanel1.Contains(chesspiecesB[11]))
                {
                    //MessageBox.Show("game over : Black king is dead");
                    postScreen(true);
                }
            }
            changeTimers();
        }

        private void performCastleSmall()
        {
            if (!ongoingGame)
            {
                MessageBox.Show("No game is being played right now", "Chess Game", 0, MessageBoxIcon.Error);
                return;
            }
             
            

            if (timer1.Enabled)
            {
                //MessageBox.Show("perform castle small for white player");

                if(chessBoard.Squares[0][1].IsEmpty && chessBoard.Squares[0][2].IsEmpty && chesspiecesW[8].FirstMove && chesspiecesW[11].FirstMove)
                {
                    SoundPlayer player = new SoundPlayer("sounds/castle.wav");
                    player.Play();

                    chesspiecesW[8].FirstMove = false;
                    chesspiecesW[11].FirstMove = false;

                    chessBoard.Squares[0][0].IsEmpty = true;
                    chessBoard.Squares[0][3].IsEmpty = true;

                    chessBoard.Squares[0][1].Pointer = null;
                    chessBoard.Squares[0][2].Pointer = null;

                    chessBoard.Squares[0][1].IsEmpty = false;
                    chessBoard.Squares[0][2].IsEmpty = false;

                    chessBoard.Squares[0][1].Pointer = chesspiecesW[11];
                    chessBoard.Squares[0][2].Pointer = chesspiecesW[8];

                    chesspiecesW[11].Location = chessBoard.Squares[0][1].Location;
                    chesspiecesW[11].CurrentPosX = 0;
                    chesspiecesW[11].CurrentPosY = 1;

                    chesspiecesW[8].Location = chessBoard.Squares[0][2].Location;
                    chesspiecesW[8].CurrentPosX = 0;
                    chesspiecesW[8].CurrentPosY = 2;

                    Sb.Append("White team performed small castle\n");

                    changeTimers();

                }
                else
                {
                    MessageBox.Show("Cannot perform castle small for white player", "Chess Game", 0, MessageBoxIcon.Error);
                }

            }
            else
            {
                //MessageBox.Show("perform castle small for black player");

                if (chessBoard.Squares[7][1].IsEmpty && chessBoard.Squares[7][2].IsEmpty && chesspiecesB[8].FirstMove && chesspiecesB[11].FirstMove)
                {
                    SoundPlayer player = new SoundPlayer("sounds/castle.wav");
                    player.Play();

                    chesspiecesB[8].FirstMove = false;
                    chesspiecesB[11].FirstMove = false;

                    chessBoard.Squares[7][0].IsEmpty = true;
                    chessBoard.Squares[7][3].IsEmpty = true;

                    chessBoard.Squares[7][1].Pointer = null;
                    chessBoard.Squares[7][2].Pointer = null;

                    chessBoard.Squares[7][1].IsEmpty = false;
                    chessBoard.Squares[7][2].IsEmpty = false;

                    chessBoard.Squares[7][1].Pointer = chesspiecesW[11];
                    chessBoard.Squares[7][2].Pointer = chesspiecesW[8];

                    chesspiecesB[11].Location = chessBoard.Squares[7][1].Location;
                    chesspiecesB[11].CurrentPosX = 7;
                    chesspiecesB[11].CurrentPosY = 1;

                    chesspiecesB[8].Location = chessBoard.Squares[7][2].Location;
                    chesspiecesB[8].CurrentPosX = 7;
                    chesspiecesB[8].CurrentPosY = 2;

                    Sb.Append("Black team performed small castle\n");

                    changeTimers();


                }
                else
                {
                    MessageBox.Show("Cannot perform castle small for black player", "Chess Game", 0, MessageBoxIcon.Error);
                }
            }
        }

        private void performCastleBig()
        {

            if (!ongoingGame)
            {
                MessageBox.Show("No game is being played right now", "Chess Game", 0, MessageBoxIcon.Error);
                return;
            }


            if (timer1.Enabled)
            {
                // MessageBox.Show("perform castle big for white player");


                if (chessBoard.Squares[0][4].IsEmpty && chessBoard.Squares[0][5].IsEmpty && chessBoard.Squares[0][6].IsEmpty && chesspiecesW[15].FirstMove && chesspiecesW[11].FirstMove)
                {
                    SoundPlayer player = new SoundPlayer("sounds/castle.wav");
                    player.Play();

                    chesspiecesW[15].FirstMove = false;
                    chesspiecesW[11].FirstMove = false;

                    chessBoard.Squares[0][3].IsEmpty = true;
                    chessBoard.Squares[0][7].IsEmpty = true;

                    chessBoard.Squares[0][3].Pointer = null;
                    chessBoard.Squares[0][7].Pointer = null;

                    chessBoard.Squares[0][5].IsEmpty = false;
                    chessBoard.Squares[0][6].IsEmpty = false;

                    chessBoard.Squares[0][5].Pointer = chesspiecesW[15];
                    chessBoard.Squares[0][6].Pointer = chesspiecesW[11];

                    chesspiecesW[11].Location = chessBoard.Squares[0][6].Location;
                    chesspiecesW[11].CurrentPosX = 0;
                    chesspiecesW[11].CurrentPosY = 6;

                    chesspiecesW[15].Location = chessBoard.Squares[0][5].Location;
                    chesspiecesW[15].CurrentPosX = 0;
                    chesspiecesW[15].CurrentPosY = 5;

                    Sb.Append("White team performed big castle\n");

                    changeTimers();

                }
                else
                {
                    MessageBox.Show("Cannot perform castle big for white player", "Chess Game", 0, MessageBoxIcon.Error);
                }

            }
            else
            {
                // MessageBox.Show("perform castle big for black player");



                if (chessBoard.Squares[7][4].IsEmpty && chessBoard.Squares[7][5].IsEmpty && chessBoard.Squares[7][6].IsEmpty && chesspiecesB[15].FirstMove && chesspiecesB[11].FirstMove)
                {
                    SoundPlayer player = new SoundPlayer("sounds/castle.wav");
                    player.Play();

                    chesspiecesB[15].FirstMove = false;
                    chesspiecesB[11].FirstMove = false;

                    chessBoard.Squares[7][3].IsEmpty = true;
                    chessBoard.Squares[7][7].IsEmpty = true;

                    chessBoard.Squares[7][3].Pointer = null;
                    chessBoard.Squares[7][7].Pointer = null;

                    chessBoard.Squares[7][5].IsEmpty = false;
                    chessBoard.Squares[7][6].IsEmpty = false;

                    chessBoard.Squares[7][5].Pointer = chesspiecesB[15];
                    chessBoard.Squares[7][6].Pointer = chesspiecesB[11];

                    chesspiecesB[11].Location = chessBoard.Squares[7][6].Location;
                    chesspiecesB[11].CurrentPosX = 7;
                    chesspiecesB[11].CurrentPosY = 6;

                    chesspiecesB[15].Location = chessBoard.Squares[7][5].Location;
                    chesspiecesB[15].CurrentPosX = 7;
                    chesspiecesB[15].CurrentPosY = 5;

                    Sb.Append("Black team performed big castle\n");

                    changeTimers();
                }
                else
                {
                    MessageBox.Show("Cannot perform castle big for white player");
                }
            }
        }



        //method that starts the game
        public void setup()
        {           
            ongoingGame = true; //game just started
            gameOver = false;   //game is not over

            Sb = new StringBuilder();   //make a new stringbuilder object for the history

            //squares become invisible (for safety)
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    chessBoard.Squares[i][j].Visible = false;
                }
            }
            //remove ALL newly added chesspieces from EVERYWHERE    
            foreach (ChessPiece cp in newChessPieces)
            {
                chessBoard.Controls.Remove(cp);
                flowLayoutPanel1.Controls.Remove(cp);
                flowLayoutPanel2.Controls.Remove(cp);
            }
            //restore all pawns that transformed
            foreach (Tomp tomp in graveyard)
            {
                chessBoard.Controls.Add(tomp.pawn); //put them back to the chessboard
            }
            graveyard.Clear();
            newChessPieces.Clear();

            foreach(Tile[] tileArray in chessBoard.Squares)
            {
                foreach (Tile tile in tileArray)
                {
                    tile.IsEmpty = true;
                    tile.Pointer = null;
                }
            }


            //now run 'reset' for all blacks and whites (see Chesspiece class)
            foreach (ChessPiece cp in chesspiecesB)
            {
                cp.reset();
                cp.Visible = true;
            }
            foreach (ChessPiece cp in chesspiecesW)
            {
                cp.reset();
                cp.Visible = true;
            }

            label1.Text = ts1.ToString();
            label5.Text = ts2.ToString();
           
            //enable timer only for white
            timer1.Enabled = true;
            timer2.Enabled = false;
            //activate only white pieces
            foreach (ChessPiece chessPiece1 in chesspiecesB)
            {
                chessPiece1.Enabled = false;
            }
            foreach (ChessPiece chessPiece1 in chesspiecesW)
            {
                chessPiece1.Enabled = true;
            }
          
            Date = DateTime.Now;    //timestamp of the game that just started
        }

        //method called when the game ends
        //color : true -> white won
        //color : false -> black won
        private void postScreen(bool color)
        {
            SoundPlayer player = new SoundPlayer("sounds/win.wav");
            player.Play();

            //turn off the timers and set the states
            ongoingGame = false;
            gameOver = true;
            timer1.Enabled = false;
            timer2.Enabled = false;

            /* if (color)
                MessageBox.Show("winner is the white team");
            else
                MessageBox.Show("winner is the black team");*/

            //show Form4 for the database
            Form4 form4 = new Form4(this, color);
            form4.Show();
            this.Enabled = false;

            //deactivate ALL chesspieces
            foreach (ChessPiece chessPiece in newChessPieces)
            {
                chessPiece.Enabled = false;
            }
            for (int i = 0; i < 16; i++)
            {
                chesspiecesB[i].Enabled = false;
                chesspiecesW[i].Enabled = false;
            }
        }


        //debug button / it is invisible
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (chessBoard.Squares[i][j].IsEmpty)
                        Console.Write("T");
                    if (!chessBoard.Squares[i][j].IsEmpty)
                        Console.Write("F");
                }
                Console.Write("\n");
            }
            MessageBox.Show(Sb.ToString());
        }


        ////////////////// menustrip //////////////////

        //options:
        private void standardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setThemeTo("chess_set1/", sender);
            this.BackColor = Color.FromArgb(185, 189, 199);
        }

        private void androidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setThemeTo("chess_set3/", sender);
            this.BackColor = Color.FromArgb(52, 235, 134);
        }

        private void humanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setThemeTo("chess_set4/", sender);
            this.BackColor = Color.FromArgb(231, 230, 235);
        }

        private void setThemeTo(string directory, object sender)
        {
            ChessPiece.ImageFolder = directory;
            for (int i = 0; i < 16; i++)
            {
                chesspiecesB[i].reloadImage();
                chesspiecesW[i].reloadImage();
            }
            foreach (ChessPiece cp in newChessPieces)
            {
                cp.reloadImage();
            }
            chessBoard.resetImage(directory);
            currentArtStyle = directory;

            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
            ToolStripMenuItem parent = (ToolStripMenuItem)(toolStripMenuItem.OwnerItem);
            foreach (ToolStripDropDownItem item in parent.DropDownItems)
            {
                ((ToolStripMenuItem)item).Checked = false;
            }
            ((ToolStripMenuItem)sender).Checked = true;
        }

        private void woodenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            this.BackColor = Color.FromArgb(252, 191, 101);
            //different way
            ChessPiece.ImageFolder = "chess_set2/";
            for (int i = 0; i < 16; i++)
            {
                chesspiecesB[i].reloadImage();
                chesspiecesW[i].reloadImage();
            }
            foreach (ChessPiece cp in newChessPieces)
            {
                cp.reloadImage();
            }
            chessBoard.resetImage("chess_set2/");
            currentArtStyle = "chess_set2/";

            //using linq
            var senderItem = (ToolStripMenuItem)sender;
            ((ToolStripMenuItem)senderItem.OwnerItem).DropDownItems
                .OfType<ToolStripMenuItem>().ToList()
                .ForEach(chesspiece1 =>
                {
                    chesspiece1.Checked = false;
                });
            senderItem.Checked = true;
        }

        //Game:
        private void New_Game_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //deactive the form
            this.Enabled = false;
            if (ongoingGame)
            {
                //save state of the timers and pause
                bool t1 = timer1.Enabled;
                bool t2 = timer2.Enabled;
                timer1.Enabled = false;
                timer2.Enabled = false;

                //if user doesnt want to start a new game
                if (MessageBox.Show("Are you sure you want to start a new game ?" + Environment.NewLine + "Any progress made will be lost", "Chess Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    //restore the game and continue
                    this.Enabled = true;
                    timer1.Enabled = t1;
                    timer2.Enabled = t2;
                    return;
                }
            }  

            //deactivate ALL the chesspieces
            foreach(ChessPiece chessPiece in newChessPieces)
            {
                chessPiece.Enabled = false;
            }
            for(int i=0;i<16;i++)
            {
                chesspiecesB[i].Enabled = false;
                chesspiecesW[i].Enabled = false;
            }

            gameOver = true;    //game is over
            ongoingGame = false;

            Form3 form3 = new Form3(this);
            form3.Show();

        }

        private void surrenderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if user clicks 'surrend' while the game is over , exit
            if (gameOver)
            {
                MessageBox.Show("The game is over. You can start a new game");
                return;
            }

            //if user clicks 'surrend' before starting any game , exit
            if (!ongoingGame)
            {
                MessageBox.Show("The game has not yet started. You can start a new game");
                return;
            }

            //save state of the timers and pause
            bool t1 = timer1.Enabled;
            bool t2 = timer2.Enabled;
            timer1.Enabled = false;
            timer2.Enabled = false;

            //if user doesnt want to start a new game
            if (MessageBox.Show("Are you sure you want to surrender ?", "Chess Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                //restore the game and continue
                this.Enabled = true;
                timer1.Enabled = t1;
                timer2.Enabled = t2;
                return;
            }

            //call the post screen
            if (t1)
            {
                postScreen(false);
            }
            else
            {
                postScreen(true);
            }
        }

        //perform move:
        private void castleSmallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            performCastleSmall();
        }

        private void casleBigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            performCastleBig();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
