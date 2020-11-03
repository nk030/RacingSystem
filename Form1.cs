using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RacingSystem.Classes;

namespace RacingSystem
{
    public partial class Form1 : Form
    {
        Car[] Carz = new Car[4];
        Punter[] punters = new Punter[3];
        Car winnerCar;

        Timer timer1, timer2, timer3, timer4;

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        

        public Form1()
        {
            InitializeComponent();
            PrepareRace();//Calling method to start game
        }

        private void PrepareRace()////Initilize all data to start game
        {
            // Cars Info
            Carz[0] = new Car() { CarName = "Car 1", RaceTrackLength = 830, MyPictureBox = pictureBox1 };
            Carz[1] = new Car() { CarName = "Car 2", RaceTrackLength = 830, MyPictureBox = pictureBox2 };
            Carz[2] = new Car() { CarName = "Car 3", RaceTrackLength = 830, MyPictureBox = pictureBox3 };
            Carz[3] = new Car() { CarName = "Car 4", RaceTrackLength = 830, MyPictureBox = pictureBox4 };

            //Punter Info
            punters[0] = Factory.GetAPunter("Joe");
            punters[1] = Factory.GetAPunter("Bob");
            punters[2] = Factory.GetAPunter("AI");

            punters[0].MyLabel = label3;
            punters[0].MyRadioButton = radioButton1;
            punters[0].MyText = textBox1;
            punters[0].MyRadioButton.Text = punters[0].Name;


            punters[1].MyLabel = label3;
            punters[1].MyRadioButton = radioButton2;
            punters[1].MyText = textBox2;
            punters[1].MyRadioButton.Text = punters[1].Name;


            punters[2].MyLabel = label3;
            punters[2].MyRadioButton = radioButton3;
            punters[2].MyText = textBox3;
            punters[2].MyRadioButton.Text = punters[2].Name;

            numericUpDown2.Minimum = 1;
            numericUpDown2.Maximum = 4;
            numericUpDown2.Value = 1;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            SetupBet(); //calling to place bet
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text.Contains("Game"))
            {
                MessageBox.Show("Game Over Thank You");
                return;
            }
                timer1 = new Timer();
            timer1.Interval = 15;
            timer1.Tick += Cycling_Tick;

            timer2 = new Timer();
            timer2.Interval = 15;
            timer2.Tick += Cycling_Tick;

            timer3 = new Timer();
            timer3.Interval = 15;
            timer3.Tick += Cycling_Tick;

            timer4 = new Timer();
            timer4.Interval = 15;
            timer4.Tick += Cycling_Tick;

            timer1.Start();
            timer2.Start();
            timer3.Start();
            timer4.Start();

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int count = 0;
            int total_active = 0;
            foreach (Punter punter in punters)
            {
                if (punter.Busted)
                {
                    //MessageBox.Show("Bet is Not Placed Because " + punter.Name + " is BUSTED");
                }
                else
                {
                    total_active++;
                    if (punter.MyRadioButton.Checked)
                    {
                        if (punter.MyBet == null)
                        {
                            int number = (int)numericUpDown2.Value;
                            int amount = (int)numericUpDown1.Value;
                            bool alreadyPlaced = false;
                            foreach (Punter pun in punters)
                            {
                                if (pun.MyBet != null && pun.MyBet.Car == Carz[number - 1])
                                {
                                    alreadyPlaced = true;
                                    break;
                                }
                            }
                            if (alreadyPlaced)
                            {
                                MessageBox.Show("This Car Number is Already Taken By Another Better.");
                            }
                            else
                            {
                                punter.MyBet = new Bet() { Amount = amount, Car = Carz[number - 1] };
                            }

                        }
                        else
                        {
                            MessageBox.Show("You Already Place Bet for " + punter.Name);
                        }
                    }
                    if (punter.MyBet != null)
                    {
                        count++;
                    }
                }
            }
            SetupBet();

        }

        private void Cycling_Tick(object sender, EventArgs e)
        {
            if (sender is Timer)
            {
                int index = -1;
                Timer timer = sender as Timer;
                if (timer == timer1)
                {
                    index = 0;
                }
                else if (timer == timer2)
                {
                    index = 1;
                }
                else if (timer == timer3)
                {
                    index = 2;
                }
                else if (timer == timer4)
                {
                    index = 3;
                }

                if (index != -1)
                {
                    PictureBox pbox = Carz[index].MyPictureBox;
                    if (pbox.Location.X + pbox.Width > Carz[index].RaceTrackLength)
                    {
                        if (winnerCar == null)
                        {
                            winnerCar = Carz[index];
                        }
                        timer1.Stop();
                        timer2.Stop();
                        timer3.Stop();
                        timer4.Stop();
                    }
                    else
                    {
                        int jump = new Random().Next(1, 15);
                        pbox.Location = new Point(pbox.Location.X + jump, pbox.Location.Y);
                    }
                }
            }
            if (winnerCar != null)
            {
                MessageBox.Show("Congratulation! " + winnerCar.CarName + " Win The Race");
                SetupBet();
                foreach (Punter punter in punters)
                {
                    if (punter.MyBet != null)
                    {
                        if (punter.MyBet.Car == winnerCar)
                        {
                            punter.Cash += punter.MyBet.Amount;
                            punter.MyText.Text = punter.Name + " Won and now has $" + punter.Cash;
                            punter.Winner = true;
                        }
                        else
                        {
                            punter.Cash -= punter.MyBet.Amount;
                            if (punter.Cash == 0)
                            {
                                punter.MyText.Text = "BUSTED";
                                punter.Busted = true;
                                punter.MyRadioButton.Enabled = false;
                            }
                            else
                            {
                                punter.MyText.Text = punter.Name + " Lost and now has $" + punter.Cash;
                            }
                        }
                    }
                }
                winnerCar = null;
                timer1 = timer2 = timer3 = timer4 = null;
                int count = 0;
                foreach (Punter punter in punters)
                {
                    if (punter.Busted)
                    {
                        count++;
                    }
                    if (punter.MyRadioButton.Enabled && punter.MyRadioButton.Checked)
                    {
                        label1.Text = "Max Bet is $" + punter.Cash;
                        numericUpDown1.Maximum = punter.Cash;
                        numericUpDown1.Minimum = 1;
                    }
                    punter.MyBet = null;
                    punter.Winner = false;
                }
                if (count == punters.Length)
                {
                    button2.Text = "Game Over";

                }
                foreach (Car Car in Carz)
                {
                    Car.MyPictureBox.Location = new Point(12, Car.MyPictureBox.Location.Y);
                }
            }
        }
        private void SetupBet()//place bet with this method
        {
            foreach (Punter punter in punters)
            {
                if (punter.Busted)
                {
                    punter.MyText.Text = "BUSTED";
                }
                else
                {
                    if (punter.MyBet == null)
                    {
                        punter.MyText.Text = punter.Name + " hasn't placed a bet";
                    }
                    else
                    {
                        punter.MyText.Text = punter.Name + " bets $" + punter.MyBet.Amount + " on " + punter.MyBet.Car.CarName;
                    }
                    if (punter.MyRadioButton.Checked)
                    {
                        label2.Text = "Max Bet Amount is $" + punter.Cash.ToString();
                        button3.Text = "Place Bet for " + punter.Name;
                        punter.MyLabel.Text = punter.Name + " Bets Amount $";
                        numericUpDown1.Minimum = 1;
                        numericUpDown1.Maximum = punter.Cash;
                        numericUpDown1.Value = 1;
                    }
                }
            }
        }

        
    }
}
