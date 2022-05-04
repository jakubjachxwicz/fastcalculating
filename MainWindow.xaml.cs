using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace fastcalculating
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int first, second, operation, expectedResult;
        private enum operations
        {
            add, sub, mul, div
        };
        
        private int time = 15;
        private int score = 0;
        private DispatcherTimer timer;
        
        public MainWindow()
        {
            InitializeComponent();

            generateEquation();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timerTick;
            timer.Start();
        }

        private void dataField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                int result;

                if (int.TryParse(dataField.Text, out result))
                {
                    if (result == expectedResult)
                    {
                        commentLabel.Content = "Dobrze!";
                        commentLabel.Foreground = Brushes.Green;
                        score++;
                        scoreLabel.Content = "Wynik: " + score.ToString();
                        
                        if (time < 15) time += 2;
                        timeBar.Value = time;
                        dataField.Text = string.Empty;

                        generateEquation();
                    }

                    else
                    {
                        commentLabel.Content = "Niepoprawny wynik";
                        commentLabel.Foreground = Brushes.Red;

                        time -= 2;
                        timeBar.Value = time;
                        dataField.Text = string.Empty;
                    }
                }
            }
        }

        private void dataField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void timerTick(object sender, EventArgs e)
        {
            if (time > 0)
            {
                time--;

                timeBar.Value = time;
            }

            else
            {
                MessageBoxResult result = MessageBox.Show("Twój winik: " + score + ". Czy chcesz zagrać ponownie?", "Koniec gry", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    time = 15;
                    timeBar.Value = 15;
                    score = 0;
                    scoreLabel.Content = string.Empty;
                    commentLabel.Content = string.Empty;
                    dataField.Text = string.Empty;
                }

                else
                {
                    System.Windows.Application.Current.Shutdown();
                }
            }
        }


        private void generateEquation()
        {
            Random random = new Random();
            string op = "";

            int diff = 0;
            if (score > 10 && score <= 20) diff = 4;
            else if (score > 20 && score <= 30) diff = 8;
            else if (score > 30) diff = 12;

            switch (operation = random.Next(0, 4))
            {
                case (int)operations.add:
                    {
                        first = random.Next(3 + ((2 * diff) - 3), 10 + ((2 * diff) - 2));
                        second = random.Next(3 + ((2 * diff) - 3), 10 + ((2 * diff) - 2));

                        expectedResult = first + second;
                        op = " + ";
                    }
                    break;
                case (int)operations.sub:
                    {
                        first = random.Next(3 + ((2 * diff) - 3), 10 + ((2 * diff) - 2));
                        second = random.Next(3 + ((2 * diff) - 3), 10 + ((2 * diff) - 2));

                        while (second > first) second = random.Next(0, 9);

                        expectedResult = first - second;
                        op = " - ";
                    }
                    break;
                case (int)operations.mul:
                    {
                        first = random.Next(0 + (diff / 2), 10 + (diff / 2));
                        second = random.Next(0 + (diff / 2), 10 + (diff / 2));

                        expectedResult = first * second;
                        op = " * ";
                    }
                    break;
                case (int)operations.div:
                    {
                        int[] composites = {4, 6, 8, 9, 10, 12, 14, 15, 16, 18, 20, 21, 22, 24, 25, 26, 27, 28, 30, 32};
                        
                        first = composites[random.Next(0, 8 + diff)];
                        second = random.Next(2, 32);

                        while (first % second != 0) second = random.Next(2, 20);

                        expectedResult = first / second;
                        op = " / ";
                    }
                    break;
            }


            equationLabel.Content = first.ToString() + op + second.ToString();
        }
    }
}