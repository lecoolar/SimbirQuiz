using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using SimbirQuiz.Infrastructure;
using SimbirQuiz.Models;
using SimbirQuiz.Views;

namespace SimbirQuiz.ViewModels
{
	class QuizRunnerViewModel : ViewModelBase
	{
		private Quiz _currentQuiz;
		private int _countTask;
		private int _countScore;
		private ITestTask _currentTask;
		private DispatcherTimer _timer;
		private WrapPanel _picturesWrap;
		private MediaElement _mediaPlayer;
		private Slider _volumeSlider;
		private bool _isPlaying = true;

		private void GiveAnswer()
		{
			_countScore += CurrentTask.GetResult();
			if (_countTask + 1 == CurrentQuiz.Tasks.Count)
				new ResultWindow(_countScore, CurrentQuiz).Show();
			else
				SetCurrentTask(++CountTask);
		}

		private void SetCurrentTask(int countTask)
		{
			CurrentTask = CurrentQuiz.Tasks[countTask];
			CurrentTask.ShuffleAnswers();
		}

		private void TimeStart()
		{
			_timer = new DispatcherTimer();
			_timer.Tick += new EventHandler(TimerTick);
			_timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
			_timer.Start();
		}

		private void TimerTick(object sender, EventArgs e)
		{
			if (CurrentQuiz.TimeLimitOn)
			{
				CurrentQuiz.TimeLimit -= new TimeSpan(0, 0, 0, 0, 1000);
				if (CurrentQuiz.TimeLimit == TimeSpan.Zero)
				{
					_timer.Stop();
					new ResultWindow(_countScore, CurrentQuiz).Show();
				}
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
		private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			_mediaPlayer.Volume = _volumeSlider.Value;
		}
		private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
		{
			RestartMedia();
		}

		public QuizRunnerViewModel(Quiz currentQuiz, List<object> formsToInteract)
		{
			CurrentQuiz = currentQuiz;
			CountTask = 0;
			_countScore = 0;
			SetCurrentTask(CountTask);
			_picturesWrap = formsToInteract[0] as WrapPanel;
			_mediaPlayer = formsToInteract[1] as MediaElement;
			_volumeSlider = formsToInteract[2] as Slider;
			//_tasksListBox = formsToInteract[3] as ListBox;
			CloseCommand = new Command(obj => CloseAction());
			MinimizeCommand = new Command(obj => MinimizeAction());
			MaximizeCommand = new Command(obj => MaximizeAction());
			GiveAnswerCommand = new Command(obj => GiveAnswer());
			PauseStartMediaCommand = new Command(obj => PauseStartMedia());
			RestartMediaCommand = new Command(obj => RestartMedia());
			TimeStart();
		}

		public Command MinimizeCommand { get; set; }
		public Action MinimizeAction { get; set; }
		public Command MaximizeCommand { get; set; }
		public Action MaximizeAction { get; set; }
		public Command CloseCommand { get; set; }
		public Action CloseAction { get; set; }
		public Command PauseStartMediaCommand { get; set; }
		public Command RestartMediaCommand { get; set; }
		public Command GiveAnswerCommand { get; set; }

		public Quiz CurrentQuiz
		{
			get => _currentQuiz;
			set
			{
				_currentQuiz = value;
				OnPropertyChanged("CurrentQuiz");
			}
		}

		public ITestTask CurrentTask
		{
			get => _currentTask;
			set
			{
				_currentTask = value;
				OnPropertyChanged("CurrentTask");
			}
		}

		public int CountTask
		{
			get => _countTask;
			set
			{
				_countTask = value;
				OnPropertyChanged("CountTask");
			}
		}
	}
}
