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
    //used to choose time to start game
    public partial class Form3 : Form
    {
        Form1 form1;    //reference to form1

        public Form3(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //reset the timespans of form1
            form1.ts1 = new TimeSpan();
            form1.ts2 = new TimeSpan();

            //set them for each button
            if (radioButton1.Checked)
            {
                form1.ts1 = form1.ts1.Add(TimeSpan.FromMinutes(10));
                form1.ts2 = form1.ts2.Add(TimeSpan.FromMinutes(10));

                form1.GivenTime = TimeSpan.FromMinutes(10); //also write to the field for the database
            }
            else if(radioButton2.Checked)
            {
                form1.ts1 = form1.ts1.Add(TimeSpan.FromHours(1));
                form1.ts2 = form1.ts2.Add(TimeSpan.FromHours(1));

                form1.GivenTime = TimeSpan.FromHours(1);
            }
            else if (radioButton3.Checked)
            {
                if(numericUpDown1.Value == 0 && numericUpDown2.Value == 0)
                {
                    MessageBox.Show("Invalid input");
                    return;
                }
                else
                {
                    TimeSpan ts = new TimeSpan();
                    ts = ts.Add(TimeSpan.FromMinutes(int.Parse(numericUpDown2.Value.ToString())));
                    ts = ts.Add(TimeSpan.FromHours(int.Parse(numericUpDown1.Value.ToString())));

                    form1.ts1 = form1.ts1.Add(ts);
                    form1.ts2 = form1.ts2.Add(ts);

                    form1.GivenTime = ts;
                }
            }

            form1.setup();  //start game     
            this.Close();

        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.Enabled = true;
            form1.Focus();
        }
    }
}
