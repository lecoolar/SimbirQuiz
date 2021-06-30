using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using SimbirQuiz.Infrastructure;
using SimbirQuiz.Models;
using SimbirQuiz.Views;

namespace SimbirQuiz.ViewModels
{
	class QuizEditorViewModel : ViewModelBase
	{
		private IFileService _fileService;
		private IDialogService _dialogService;

		private Quiz _currentQuiz;
		private ITestTask _selectedTask;
		private GradePoint _selectedGradePoint;
		private Answer _selectedAnswer;
		private WrapPanel _picturesWrap;
		private MediaElement _mediaPlayer;
		private Slider _volumeSlider;
		private bool _isPlaying = true;

		private void CreateQuiz() => CurrentQuiz = new Quiz("newQuiz");

		public void OpenQuiz()
		{
			try
			{
				if (_dialogService.OpenFileDialog() == true)
					CurrentQuiz = _fileService.Open(_dialogService.FilePath);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void SaveQuiz()
		{
			try
			{
				if (_dialogService.SaveFileDialog(CurrentQuiz.Name) == true)
					_fileService.Save(_dialogService.FilePath, CurrentQuiz);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void CreateAndAddTask(ICreatorTask creatorTask)
		{
			if (creatorTask != null)
			{
				int indexOfNewTask = CurrentQuiz.Tasks.Count;
				CurrentQuiz.Tasks.Add(creatorTask.CreateTask());
				SelectedTask = CurrentQuiz.Tasks[indexOfNewTask];
			}
		}

		private void Close()
		{
			SaveQuiz();
			CloseAction();
		}

		private void ChangeSourceImage(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			string[] paths = null;
			if ((paths = OpenImageFile()) != null)
			{
				((Image)button.DataContext).Source = new BitmapImage(new Uri(paths[0]));
			}
		}

		private void DeletePicture(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;			
			_picturesWrap.Children.Remove((WrapPanel)button.DataContext);
		}

		private Button CreateDeletePictureButton(object pictureWrap)
		{
			Button deleteButton = new Button();			
			deleteButton.Height = 20;
			deleteButton.Width = 30;
			MaterialDesignThemes.Wpf.PackIcon icon = new MaterialDesignThemes.Wpf.PackIcon();
			icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Delete;
			deleteButton.DataContext = pictureWrap as WrapPanel;
			deleteButton.Click += DeletePicture;
			deleteButton.Content = icon;			
			deleteButton.Style = new Style();
			
			return deleteButton;
		}

		private Button CreateEditSourceButton(object image)
		{
			Button editButton = new Button();
			editButton.Height = 20;
			editButton.Width = 30;
			editButton.Margin = new Thickness(0);
			editButton.Content = "Изменить";			
			editButton.DataContext = image as Image;
			editButton.Click += ChangeSourceImage;
			return editButton;
		}

		private StackPanel CreatePicturePanel()
		{
			StackPanel picture = new StackPanel();			
			picture.MaxWidth = 100;
			picture.MaxHeight = 100;
			Image image = new Image();			
			image.Height = 65;
			image.Width = 65;	
			picture.Children.Add(image);
			StackPanel buttons = new StackPanel();
			buttons.Height = 20;
			buttons.Width = 70;
			buttons.Orientation = Orientation.Horizontal;
			buttons.Children.Add(CreateEditSourceButton(picture.Children[0]));
			buttons.Children.Add(CreateDeletePictureButton(picture));
			picture.Children.Add(buttons);	
			return picture;
		}

		private void AddPicture_Click()		
        {
			string[] paths = null;
			if ((paths = OpenImageFile()) != null)
			{	
				foreach (var path in paths)
				{
					_picturesWrap.Children.Add(CreatePicturePanel());                    
                    var image = ((StackPanel)_picturesWrap.Children[_picturesWrap.Children.Count - 1]).Children[0] as Image;
                    image.Source = new BitmapImage(new Uri(path));
                }
			}
		}

		private string[] OpenImageFile()
        {
			string[] paths = null;
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.InitialDirectory = Environment.CurrentDirectory;
			openFile.Filter = "Image Files |*.jpeg; *.png";
			openFile.Multiselect = true;
			openFile.RestoreDirectory = true;
			if (openFile.ShowDialog() == true)
			{
				paths = openFile.FileNames;
			}
			return paths;
		}

		private string OpenMediaFile()
		{
			string puth = "";
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = Environment.CurrentDirectory;
			openFileDialog.Filter = "Videos Files |*.mp4; *.mpeg; *.avi| Audio Files |*.mp3";//форматы видео и аудио
			openFileDialog.RestoreDirectory = true;
			if (openFileDialog.ShowDialog() == true)
			{
				puth = openFileDialog.FileName;			
			}
			return puth;
		}
		private void AddMedia_Click()
		{
			string puth = "";
			if ((puth = OpenMediaFile()) != "")
			{
				_mediaPlayer.Source = new Uri(puth);
				_mediaPlayer.Play();
				_volumeSlider.ValueChanged += VolumeSlider_ValueChanged;
				_volumeSlider.Value = 0.1;
                _mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            }
		}

		private void PauseStartMedia()
		{
			if (_isPlaying)
			{
				_mediaPlayer.Pause();
				_isPlaying = false;
			}
			else
			{
				_mediaPlayer.Play();
				_isPlaying = true;
			}
		}

		private void RestartMedia()
		{
			_mediaPlayer.Stop();
			_mediaPlayer.Play();
		}

		private void DeleteMedia()
		{
			_mediaPlayer.Close();
			_mediaPlayer.Source = null;
		}

		private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_mediaPlayer.Volume = _volumeSlider.Value;
		}
		private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
		{
			RestartMedia();
		}
		public QuizEditorViewModel(IDialogService dialogService, IFileService fileService, List<object> formsToIteract)
		{
			_dialogService = dialogService;
			_fileService = fileService;
			_picturesWrap = formsToIteract[0] as WrapPanel;
			_mediaPlayer = formsToIteract[1] as MediaElement;
			_volumeSlider = formsToIteract[2] as Slider;
			NewCommand = new Command(obj => CreateQuiz());
			OpenCommand = new Command(obj => OpenQuiz());
			SaveCommand = new Command(obj => SaveQuiz(), obj => CurrentQuiz != null);
			EditCommand = new Command(obj => new SettingsWindow(CurrentQuiz).Show(), obj => CurrentQuiz != null);
			AddTaskCommand = new Command(obj => CreateAndAddTask(obj as ICreatorTask), obj => CurrentQuiz != null);
			RemoveTaskCommand = new Command(obj => CurrentQuiz.Tasks.Remove(SelectedTask), obj => SelectedTask != null);
			AddGradePointCommand = new Command(obj => CurrentQuiz.AddNewGradePoint(), obj => CurrentQuiz != null);
			RemoveGradePointCommand = new Command(obj => CurrentQuiz.GradePoints.Remove(SelectedGradePoint), obj => SelectedGradePoint != null);
			AddAnswerCommand = new Command(obj => SelectedTask.AddNewAnswer(), obj => SelectedTask != null);
			RemoveAnswerCommand = new Command(obj => SelectedTask.RemoveAnswer(SelectedAnswer), obj => SelectedAnswer != null);
			CloseCommand = new Command(obj => Close());
			MinimizeCommand = new Command(obj => MinimizeAction());
			MaximizeCommand = new Command(obj => MaximizeAction());
			AddPictureCommand = new Command(obj => AddPicture_Click());
			AddVideoAudio = new Command(obj => AddMedia_Click());
			PauseStartMediaCommand = new Command(obj => PauseStartMedia());
			RestartMediaCommand = new Command(obj => RestartMedia());
			DeleteMediaCommand = new Command(obj => DeleteMedia());
		}

		public Command NewCommand { get; set; }
		public Command OpenCommand { get; set; }
		public Command SaveCommand { get; set; }
		public Command EditCommand { get; set; }
		public Command AddTaskCommand { get; set; }
		public Command RemoveTaskCommand { get; set; }
		public Command AddGradePointCommand { get; set; }
		public Command RemoveGradePointCommand { get; set; }
		public Command AddAnswerCommand { get; set; }
		public Command RemoveAnswerCommand { get; set; }
		public Command AddPictureCommand { get; set; }
		public Command AddVideoAudio { get; set; }
		public Command PauseStartMediaCommand { get; set; }//
		public Command RestartMediaCommand { get; set; }
		public Command DeleteMediaCommand { get; set; }
		public Command MinimizeCommand { get; set; }
		public Action MinimizeAction { get; set; }
		public Command MaximizeCommand { get; set; }
		public Action MaximizeAction { get; set; }
		public Command CloseCommand { get; set; }
		public Action CloseAction { get; set; }

		public Quiz CurrentQuiz
		{
			get => _currentQuiz;
			set
			{
				_currentQuiz = value;
				OnPropertyChanged("CurrentQuiz");
			}
		}

		public WrapPanel PicturesWrap
        {
			get => _picturesWrap;
            set
            {
				_picturesWrap = value;
				//OnPropertyChanged("PicturesWrap");
            }
        }

		public ITestTask SelectedTask
		{
			get => _selectedTask;
			set
			{
				_selectedTask = value;
				OnPropertyChanged("SelectedTask");
			}
		}

		public GradePoint SelectedGradePoint
		{
			get => _selectedGradePoint;
			set
			{
				_selectedGradePoint = value;
				OnPropertyChanged("SelectedGradePoint");
			}
		}

		public Answer SelectedAnswer
		{
			get => _selectedAnswer;
			set
			{
				_selectedAnswer = value;
				OnPropertyChanged("SelectedAnswer");
			}
		}
	}
}
