using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsChessApp
{
    public partial class Form4 : Form
    {

        Model1Container container = new Model1Container();

        bool winner;    //true : white team won / false: black team won
        Form1 form1;    //reference to form1

        TimeSpan duration;  //to calculate duration of the game

        public Form4(Form1 form1, bool winner)
        {
            InitializeComponent();
            this.form1 = form1;
            this.winner = winner;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            if (winner)
            {
                label2.Text = "White team";
            }
            else
            {
                label2.Text = "Black team";
            }

            duration = DateTime.Now - form1.Date;   //calculate duration of the game

        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.Enabled = true;
            form1.Focus();
        }

        //upload button
        private void button1_Click(object sender, EventArgs e)
        {
            //both textboxes must be filled
            if(textBox1.Text.Length==0 || (textBox2.Text.Length == 0)){
                MessageBox.Show("Please fill in both fields", "Chess Game", 0, MessageBoxIcon.Error);
                return;
            }
            else if (textBox1.Text.Equals(textBox2.Text))
            {
                MessageBox.Show("Cannot take identical nicknames", "Chess Game", 0, MessageBoxIcon.Error);
                return;
            }

            //database guide

            //textBox1.Text                 nickname of black player
            //textBox2.Text                 nickname of white player
            //label2.Text                   winner
            //form1.Date.ToString()         timestamp
            //duration.ToString()           duration of the game
            //form1.GivenTime.ToString()    given time for the players
            //form1.ts1.ToString()          remaining time for white player
            //form1.ts2.ToString()          remaining time for black player 
            //form1.Sb.ToString()           match history

            //upload
            Game game = new Game();
            game.Player_black_nickname = textBox1.Text;
            game.Player_white_nickname = textBox2.Text;
            game.Winner = label2.Text;
            game.Timestamp = form1.Date.ToString();
            game.Duration = duration.ToString();
            game.GivenTime = form1.GivenTime.ToString();
            game.Remaining_white_player = form1.ts1.ToString();
            game.Remaining_black_player = form1.ts2.ToString();

            History history = new History { Move = form1.Sb.ToString() };
            game.Histories.Add(history);
            container.Games.Add(game);
            container.SaveChanges();

            MessageBox.Show("Game uploaded to the database successfully!", "Chess Game", 0, MessageBoxIcon.Information);
            this.Close();

        }
    }
}
