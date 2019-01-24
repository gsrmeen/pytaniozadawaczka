using System.Windows;
namespace Pytaniozadawaczka
{
    public partial class ShufflerWindow : Window
    {
		public int HowMany { get; set; }
        public ShufflerWindow()
        {
            InitializeComponent();
			lblHowMany.Text = "How many questions to shuffle? Max: " + MainWindow.mMainCollectionCount;
        }

		private void OKClick(object sender, RoutedEventArgs e)
		{
			bool b = int.TryParse(txtInput.Text, out int input);
			if(!b || input < 0 || input > MainWindow.mMainCollectionCount)
			{
				MessageBox.Show("Incorrect Value");
			}
			else
			{
				HowMany = input;
				this.Close();
			}
		}
	}
}
