using System.Text;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace mastermind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StringBuilder sb = new StringBuilder();
        Random rnd = new Random();

        SolidColorBrush[] colorSelection = [Brushes.Red, Brushes.Blue, Brushes.Green, Brushes.White, Brushes.Yellow, Brushes.Orange];
        string[] colorSelectionString = ["Red", "Blue", "Green", "White", "Yellow", "Orange"];
        int[] colorsRandom = new int[4];
        string[,] highscoresList = new string[15, 3];
        List<string> userList = new List<string>();

        int attempts = 0;
        int maxattempts = 10;
        int score = 100;
        bool isPlaying = false;
        string username = "";

        DispatcherTimer timer = new DispatcherTimer();
        DateTime startTime;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PickColors()
        {
            for (int i = 0; i < colorsRandom.Length; i++)
            {
                colorsRandom[i] = rnd.Next(1, 6);
            }

            debugTextBox.Text = $"Oplossing: ({colorSelectionString[colorsRandom[0]]}, {colorSelectionString[colorsRandom[1]]}, {colorSelectionString[colorsRandom[2]]}, {colorSelectionString[colorsRandom[3]]})";
        }

        private void ComboBoxItemsInit()
        {
            color1ComboBox.ItemsSource = colorSelectionString;
            color2ComboBox.ItemsSource = colorSelectionString;
            color3ComboBox.ItemsSource = colorSelectionString;
            color4ComboBox.ItemsSource = colorSelectionString;
        }

        private void ComboBox_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox.Name == "color1ComboBox")
            {
                color1Ellipse.Fill = colorSelection[comboBox.SelectedIndex];
            }
            else if (comboBox.Name == "color2ComboBox")
            {
                color2Ellipse.Fill = colorSelection[comboBox.SelectedIndex];
            }
            else if (comboBox.Name == "color3ComboBox")
            {
                color3Ellipse.Fill = colorSelection[comboBox.SelectedIndex];
            }
            else if (comboBox.Name == "color4ComboBox")
            {
                color4Ellipse.Fill = colorSelection[comboBox.SelectedIndex];
            }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                bool check1 = CheckComboBox(color1Ellipse, color1ComboBox, 0);
                bool check2 = CheckComboBox(color2Ellipse, color2ComboBox, 1);
                bool check3 = CheckComboBox(color3Ellipse, color3ComboBox, 2);
                bool check4 = CheckComboBox(color4Ellipse, color4ComboBox, 3);
                UpdateAttempts();
                StartCountdown();
                scoreLabel.Content = $"Current score: {score}";

                if (check1 && check2 && check3 && check4)
                {
                    EndGame(true);
                }
            }
            
        }

        private bool CheckComboBox(Ellipse elipse, ComboBox combobox, int number)
        {
            elipse.Stroke = Brushes.Black;
            elipse.StrokeThickness = 1;

            if (combobox.SelectedIndex == colorsRandom[number])
            {
                elipse.Stroke = Brushes.DarkRed;
                elipse.StrokeThickness = 5;
                return true;
            }
            else if (colorsRandom.Contains(combobox.SelectedIndex))
            {
                elipse.Stroke = Brushes.Wheat;
                elipse.StrokeThickness = 5;
                score -= 1;
                return false;
            }
            else
            {
                score -= 2;
                return false;
            }
        }

        private void UpdateAttempts()
        {
            attempts += 1;
            this.Title = $"Poging {attempts}";

            if (attempts >= maxattempts)
            {
                isPlaying = false;
                EndGame(false);
            }
        }

        private void mastermind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                ToggleDebug();
            }
        }

        /// <summary>
        /// De ToggleDebug functie togglet tussen het wel en niet
        /// tonen van een textbox met daarin de willekeurig gegenereerde oplossing
        /// </summary>
        private void ToggleDebug()
        {
            if (debugTextBox.Visibility == Visibility.Hidden)
            {
                debugTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                debugTextBox.Visibility = Visibility.Hidden;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - startTime;
            timerLabel.Content = elapsed.ToString(@"ss");

            if (elapsed.TotalSeconds > 11)
            {
                StopCountdown();
            }
        }

        /// <summary>
        /// De StartCountdown functie start een secondenteller die
        /// zal tellen vanaf 0 en door zal lopen tot 10
        /// </summary>
        private void StartCountdown()
        {
            if (isPlaying)
            {
                timerLabel.Content = "00";
                timer.Tick += Timer_Tick;
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Start();
                startTime = DateTime.Now;
            }
        }

        /// <summary>
        /// <para>De StopCountdown functie stopt de secondenteller en
        /// zal deze opnieuw laten starten.</para>
        /// <para> Na het stoppen van de voorgaande teller zal ook
        /// de UpdateAttempts functie aangeroepen worden.</para>
        /// </summary>
        private void StopCountdown()
        {
            timer.Stop();
            UpdateAttempts();
            StartCountdown();
        }

        private void NewGame()
        {
            color1Ellipse.Stroke = Brushes.Black;
            color1Ellipse.StrokeThickness = 1;
            color1Ellipse.Fill = Brushes.Transparent;
            color2Ellipse.Stroke = Brushes.Black;
            color2Ellipse.StrokeThickness = 1;
            color2Ellipse.Fill = Brushes.Transparent;
            color3Ellipse.Stroke = Brushes.Black;
            color3Ellipse.StrokeThickness = 1;
            color3Ellipse.Fill = Brushes.Transparent;
            color4Ellipse.Stroke = Brushes.Black;
            color4Ellipse.StrokeThickness = 1;
            color4Ellipse.Fill = Brushes.Transparent;

            attempts = 0;
            isPlaying = true;

            PickColors();
            UpdateAttempts();
            StartCountdown();

            score = 100;
            scoreLabel.Content = $"Current score: {score}";
        }

        private void EndGame(bool hasWon)
        {
            isPlaying = false;
            timer.Stop();

            if (hasWon)
            {
                MessageBoxResult Result = MessageBox.Show($"Code is gekraakt in {attempts} pogingen.", "WINNER", MessageBoxButton.OK);
                for (int i = 0; i < highscoresList.GetLength(0); i++)
                {
                    if (highscoresList[i, 0] == null) 
                    {
                        highscoresList[i, 0] = username;
                        highscoresList[i, 1] = attempts.ToString();
                        highscoresList[i, 2] = score.ToString();
                        break;
                    }
                }
                gameUIGrid.Visibility = Visibility.Hidden;
                usernameUIGrid.Visibility = Visibility.Visible;
            }
            else if (!hasWon)
            {
                MessageBoxResult Result = MessageBox.Show($"You failed! De correcte code was {colorSelectionString[colorsRandom[0]]}, {colorSelectionString[colorsRandom[1]]}, {colorSelectionString[colorsRandom[2]]}, {colorSelectionString[colorsRandom[3]]}.", "FAILED", MessageBoxButton.OK);
            }
        }

        private void Mastermind_Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isPlaying)
            {
                var result = MessageBox.Show("Wilt u het spel vroegtijdig beëindigen?", $"Poging {attempts}/10", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
            
        }

        private void MnuNew_Click(object sender, System.EventArgs e)
        {
            settingsUIGrid.Visibility = Visibility.Hidden;
            gameUIGrid.Visibility = Visibility.Hidden;
            usernameUIGrid.Visibility = Visibility.Visible;
        }

        private void MnuHighscores_Click(object sender, System.EventArgs e)
        {
            for (int i = 0; i < highscoresList.GetLength(0); i++)
            {
                if (highscoresList[i, 0] != null)
                {
                    sb.AppendLine($"{highscoresList[i, 0]} - {highscoresList[i, 1]} pogingen - {highscoresList[i, 2]}/100");
                }
            }
            MessageBox.Show(sb.ToString(), "Mastermind highscores", MessageBoxButton.OK);
            sb.Clear();
        }

        private void MnuExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void MnuSettings_Click(object sender, System.EventArgs e)
        {
            if (!isPlaying)
            {
                gameUIGrid.Visibility = Visibility.Hidden;
                usernameUIGrid.Visibility = Visibility.Hidden;
                settingsUIGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("De instellingen kunnen tijdens het spelen niet geopend worden!", "Let op!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            string tempUsername = usernameTextBox.Text;
            if (tempUsername.Length > 0)
            {
                username = usernameTextBox.Text;
                userList.Add(tempUsername);
                MessageBoxResult result = MessageBox.Show($"Speler werd toegevoegd, wil je nog een speler toevoegen?", "Extra spelers toevoegen?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    usernameUIGrid.Visibility = Visibility.Hidden;
                    gameUIGrid.Visibility = Visibility.Visible;
                    ComboBoxItemsInit();
                    NewGame();
                }
            }
            else
            {
                MessageBox.Show("Er werd geen username ingegeven!", "Geen geldige input", MessageBoxButton.OK);
            }

        }

        private void AmountAttemptsButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = int.TryParse(amountAttemptsTextBox.Text, out int tempint);
            if (result)
            {
                if (tempint > 3 && tempint < 20)
                {
                    maxattempts = tempint;
                    MessageBox.Show($"De hoeveelheid pogingen werd aangepast naar {maxattempts}", "Instellingen", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show($"De hoeveelheid pogingen moet tussen 3 en 20 liggen.", "Instellingen", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show($"De ingevoerde waarde moet een getal zijn!", "Instellingen", MessageBoxButton.OK);
            }
        }
    }
}