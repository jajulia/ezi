using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace ezi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Knowledge knowledge;
        public MainWindow()
        {
            knowledge = new Knowledge();
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                Button button = sender as Button;
                if (button.Name == "DocumentsButton")
                {
                    DocumentsTextBox.Text = filename;
                }
                else if (button.Name == "KeywordsButton")
                {
                    KeywordsTextBox.Text = filename;
                }
            }
        }

        private void ProcessButton_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                knowledge.UpdateData(DocumentsTextBox.Text, KeywordsTextBox.Text);
                DocumentsListView.ItemsSource = knowledge.documents;
                KeywordsListView.ItemsSource = knowledge.keywords;
                knowledge.Calculate(Query.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }


    }
}
