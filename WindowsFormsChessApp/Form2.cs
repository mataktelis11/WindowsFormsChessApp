using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsChessApp.ChessItems;

namespace WindowsFormsChessApp
{
    public partial class Form2 : Form
    {
        private Button[] buttons = new Button[4];
        private string artstyle;    //directory of images
        private bool color;         //black of white

        private Form1 pointer;  //reference to form1
        private Pawn pawn;      //pawn to be 'transformed'
        private int i;          //Y coordinate of pawn

        public Form2(string artstyle,bool color, Form1 pointer, Pawn pawn,int i)
        {
            InitializeComponent();

            this.artstyle = artstyle;
            this.color = color;
            this.pointer = pointer;
            this.pawn = pawn;
            this.i = i;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Focus();
            //create the 4 buttons
            string[] names = { "rook", "queen", "bishop","knight" };

            for(int i = 0; i < 4; i++)
            {
                buttons[i] = new Button();
                buttons[i].FlatStyle = System.Windows.Forms.FlatStyle.Popup;
                buttons[i].Location = new System.Drawing.Point(12 + i*120, 100);
                buttons[i].Name = names[i];
                buttons[i].Size = new System.Drawing.Size(100, 100);
                buttons[i].Click += new System.EventHandler(clickMe);   //register click event
                //give corrent name for image source
                if(color)
                    buttons[i].BackgroundImage = Image.FromFile(artstyle + names[i] + ".png");
                else
                    buttons[i].BackgroundImage = Image.FromFile(artstyle + names[i] + "w.png");
                buttons[i].BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                this.Controls.Add(buttons[i]);
            }

        }

        //click event
        private void clickMe(object sender, EventArgs e)
        {
            pointer.pawntransformStep2(pawn,i,((Button)sender).Name);   //call method of form1 and close / send the pawn, Y coordinate , and the  name of the button that was pressed
            this.Close(); 
        }

        //when is closed
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            pointer.Enabled = true;
            pointer.Focus();
        }
    }
}
