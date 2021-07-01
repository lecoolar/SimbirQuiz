using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace SimbirQuiz.Infrastructure
{
    /// <summary>
    /// Логика взаимодействия для FileOpen.xaml
    /// </summary>
    public partial class FileOpen : Window
    {
        private string _selectedQuizPath = null;

        public string SelectedQuizPath { get => _selectedQuizPath; }

        public FileOpen()
        {
            InitializeComponent();
            FindFiles();
        }

        private void FindFiles()
        {
            if (Directory.Exists("Quizzes"))
            {
                string[] foundedDirs = Directory.GetDirectories(Path.GetFullPath("Quizzes"));
                List<string> foundedQuizzes = new List<string>();
                if (foundedDirs.Length > 0)
                {
                    foreach (var dir in foundedDirs)
                    {                        
                        var files = Directory.GetFiles(dir, "*.json");
                        if (files.Length > 0)
                        {
                            foundedQuizzes.Add(files[0]);
                        }                        
                    }
                }

                foreach (var item in foundedQuizzes)
                {
                    Label fileName = new Label();
                    fileName.Content = new DirectoryInfo(Path.GetDirectoryName(item)).Name;
                    fileName.Tag = item;
                    fileName.FontSize = 18;
                    fileName.FontFamily = new FontFamily("Arial");
                    fileName.MouseLeftButtonDown += QuizLabel_Click;
                    fileName.MouseEnter += QuizLabel_MouseEnter;
                    fileName.MouseLeave += QuizLabel_MouseLeave;
                    FoundedFiles.Children.Add(fileName);
                }
            }
        }

        private void QuizLabel_MouseEnter(object sender, RoutedEventArgs e)
        {
            (sender as Label).Background = Brushes.MediumPurple;
        }

        private void QuizLabel_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Label).Background = Brushes.Transparent;
        }

        private void QuizLabel_Click(object sender, RoutedEventArgs e)
        {
            _selectedQuizPath = (sender as Label).Tag.ToString();
            this.DialogResult = true;
            this.Close();
        }

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
