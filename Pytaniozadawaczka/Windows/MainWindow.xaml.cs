using Pytaniozadawaczka.Models;
using Pytaniozadawaczka.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;


namespace Pytaniozadawaczka
{

    public partial class MainWindow : Window
    {

        private QuestionCollection mMainCollection;
        public static int mMainCollectionCount;
        private QuestionCollection mCurrentCollection;
        private int mCurrentQuestion = 0;
        private int mMaxQuestions;
        private int mScore;
        private bool oneWrong;
        private DispatcherTimer mMainTimer = new DispatcherTimer();
        private DateTime mTimerStart;

        public MainWindow()
        {
            InitializeComponent();
            string xmlPath = PickFileWindow.Display(@"../../Exams/databases_exam.xml");
            if (xmlPath == null)
            {
                this.Close();
                return;
            }

            mMainCollection = new QuestionCollection(xmlPath);
            mMainCollectionCount = mMainCollection.Count;
            RestartGame();
        }

        public void RestartGame()
        {
            stpAnswers.Children.Clear();
            lblQuestion.Text = "";
            lblTimer.Content = "";


            if (this.IsVisible)
            {
                double result = (double)mScore / (double)mMaxQuestions;
                result *= 100;
                string message =
                    "Congratulations!" + Environment.NewLine +
                    "You earned " + mScore + "/" + mMaxQuestions + " points!" + Environment.NewLine +
                    "That makes " + (int)result + "%" + Environment.NewLine +
                    "You managed to finish in just " + new DateTime((DateTime.Now - mTimerStart).Ticks).ToString("HH:mm:ss") + "!";
                MessageBox.Show(message);
            }


            ShufflerWindow w1 = new ShufflerWindow();
            w1.ShowDialog();


            mMainTimer.Interval = new TimeSpan(0, 0, 1);
            mMainTimer.Start();
            mTimerStart = DateTime.Now;
            mMainTimer.Tick += (s, ea) =>
            {
                TimeSpan diff = DateTime.Now - mTimerStart;
                string formatted = new DateTime(diff.Ticks).ToString("HH:mm:ss");
                lblTimer.Content = formatted;
            };

            mCurrentQuestion = 0;
            mScore = 0;
            mMaxQuestions = w1.HowMany;
            mCurrentCollection = mMainCollection.RandomizeCollection(mMaxQuestions);
            LoadQuestion();
        }


        private void LoadQuestion()
        {
            UpdatePanels();
            if (mCurrentQuestion == mMaxQuestions)
            {
                mMainTimer.Stop();
                RestartGame();
                return;
            }

            Question q = mCurrentCollection.Questions[mCurrentQuestion];
            lblQuestion.Text = q.Value;
            stpAnswers.Children.Clear();
            for (int i = 0; i < q.Answers.Count; i++)
            {
                Answer a = q.Answers[i];

                CheckBox cb = new CheckBox()
                {
                    Content = new TextBlock()
                    {
                        Padding = new Thickness(3),
                        TextAlignment = TextAlignment.Justify,
                        TextWrapping = TextWrapping.Wrap,
                        Text = a.Value,
                        Margin = new Thickness(5, -8, 0, 0)
                    },
                    IsChecked = false,
                    Margin = new Thickness(30, 8, 8, 8),
                    FontSize = 16,
                };

                stpAnswers.Children.Add(cb);
            }
        }

        private void NextQuestion(object sender, RoutedEventArgs e)
        {
            (bool canProceed, bool[] correctAnswers) = VerifyAnswer();

            if (canProceed)
            {
                oneWrong = false;
                mScore++;
                mCurrentQuestion++;
                LoadQuestion();
            }
            else
            {
                if (!oneWrong)
                {
                    oneWrong = true;
                    mScore--;
                }
                MarkCorrectAnswers(correctAnswers);
            }

        }


        public (bool, bool[] correctAnswer) VerifyAnswer()
        {
            Question q = mCurrentCollection.Questions[mCurrentQuestion];
            bool[] userInput = new bool[q.Answers.Count];
            for (int i = 0; i < userInput.Length; i++)
            {
                CheckBox cb = stpAnswers.Children[i] as CheckBox;
                userInput[i] = cb.IsChecked.Value;
            }

            return q.VerifyAnswer(userInput);
        }


        public void MarkCorrectAnswers(bool[] correctAnswers)
        {
            for (int i = 0; i < stpAnswers.Children.Count; i++)
            {
                CheckBox cb = stpAnswers.Children[i] as CheckBox;
                if (!correctAnswers[i])
                {
                    TextBlock tb = cb.Content as TextBlock;
                    if (mCurrentCollection.Questions[mCurrentQuestion].Answers[i].Correct)
                        tb.Background = Brushes.LightGreen;
                    else
                        tb.Background = Brushes.PaleVioletRed;
                }
            }
        }

        private void UpdatePanels()
        {
            lblCurrent.Content = "Question " + mCurrentQuestion + " / " + mMaxQuestions;
            lblScore.Content = "Score " + mScore + " / " + mCurrentQuestion;
        }
    }
}
