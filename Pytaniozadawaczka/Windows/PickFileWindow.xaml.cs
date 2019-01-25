using Microsoft.Win32;
using Pytaniozadawaczka.Models;
using System;
using System.IO;
using System.Windows;

namespace Pytaniozadawaczka.Windows
{
    /// <summary>
    /// Interaction logic for PickFileWindow.xaml
    /// </summary>
    public partial class PickFileWindow : Window
    {
        private string mDefaultXmlPath;
        private bool mClosedCorrectly = false;
        private PickFileWindow()
        {
            InitializeComponent();
        }


        private void OKClick(object sender, RoutedEventArgs e)
        {
            if(ValidFile())
            {
                mClosedCorrectly = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect file");
            }
        }

        private bool ValidFile()
        {
            if (!File.Exists(txtInput.Text)) return false;
            try
            {
                QuestionCollection qc = new QuestionCollection(txtInput.Text);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void ChangePathClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML Files (*.xml)|*.xml";
            dlg.InitialDirectory = new FileInfo(mDefaultXmlPath).Directory.Name;
            
            if(dlg.ShowDialog().Value == true)
            {
                string path = dlg.FileName;
                txtInput.Text = path;
            }
        }

        public static string Display(string defaultPath)
        {
            PickFileWindow window = new PickFileWindow();
            window.mDefaultXmlPath = defaultPath;
            window.txtInput.Text = new FileInfo(window.mDefaultXmlPath).FullName;
            window.ShowDialog();
            return window.mClosedCorrectly? window.txtInput.Text : null;
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            mClosedCorrectly = false;
            this.Close();
        }
    }
}
