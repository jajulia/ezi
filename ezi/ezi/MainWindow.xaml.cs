using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                    knowledge.UpdateData(DocumentsTextBox.Text, "");
                    DocumentsListView.ItemsSource = knowledge.documents;
                    RefreshView(new object[] { DocumentsListView.ItemsSource });
                }
                else if (button.Name == "KeywordsButton")
                {
                    knowledge.documents.Clear();
                    DocumentsListView.ItemsSource = knowledge.documents;
                    DocumentsTextBox.Text = "";
                    KeywordsTextBox.Text = filename;
                    knowledge.UpdateData("", KeywordsTextBox.Text);
                    KeywordsListView.ItemsSource = knowledge.keywords;
                    DocumentsButton.IsEnabled = true;
                    RefreshView(new object[] { DocumentsListView.ItemsSource, KeywordsListView.ItemsSource });
                }
            }
        }

        private void RefreshView(object[] itemSources)
        {
            foreach (object o in itemSources)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(o);
                view.Refresh();
            }
        }

        private void ProcessButton_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if (QueryTextBox.Text.Length > 0 && DocumentsTextBox.Text.Length > 0 && KeywordsTextBox.Text.Length > 0)
                {
                    knowledge.UpdateData("", KeywordsTextBox.Text);
                    knowledge.setParam(AlfaTextBox.Text, BetaTextBox.Text, GammaTextBox.Text);
                    knowledge.Calculate(QueryTextBox.Text);
                    //knowledge.documents.OrderBy(x => x.result);
                    ResultListView.ItemsSource = knowledge.documents.OrderByDescending(x => x.result); //knowledge.documents;
                    RefreshView(new object[] { DocumentsListView.ItemsSource, KeywordsListView.ItemsSource, ResultListView.ItemsSource });
                    knowledge.UpdateData(DocumentsTextBox.Text, "");

                }
                else
                {
                    throw new Exception("Brak wszystkich danych. Nalezy podac dwa pliki oraz zapytanie.");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }



    }
}
