using Avalonia.Controls;
using System.ComponentModel;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;

namespace CircularGaugeControl.Views
{
    public partial class MainView : UserControl
    {
        //Private variables
        DispatcherTimer? timer;
        readonly Game game1;
        readonly Game game2;
        readonly Game game3;
        readonly Game game4;

        public MainView()
        {
            InitializeComponent();
            //Set the current value of the gauges
            game1 = new Game(0);
            myGauge1.DataContext = game1;
            game2 = new Game(0);
            myGauge2.DataContext = game2;
            game3 = new Game(0);
            myGauge3.DataContext = game3;
            game4 = new Game(0);
            myGauge4.DataContext = game4;

            Loaded += MainView_Loaded;
        }

        private void MainView_Loaded(object? sender, RoutedEventArgs e)
        {
            //Start the timer
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(2500)
            };
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        void Timer_Tick(object? sender, EventArgs e)
        {
            //Update random scores
            var r = new Random();
            game1.Score = r.Next(0, 1000);
            double val = r.Next(1, 10);
            game2.Score = val / 10;
            game3.Score = r.Next(-50, 50);
            game4.Score = r.Next(50, 90)/10d;
        }
    }


    /// <summary>
    /// Helper class to simulate a game
    /// </summary>
    public class Game : INotifyPropertyChanged
    {
        private double score;

        public double Score
        {
            get { return score; }
            set
            {
                score = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Score)));
            }
        }


        public Game(double scr)
        {
            Score = scr;
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion
    }
}
