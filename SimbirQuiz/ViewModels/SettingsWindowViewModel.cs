﻿using System;

using SimbirQuiz.Infrastructure;
using SimbirQuiz.Models;

namespace SimbirQuiz.ViewModels
{
	class SettingsWindowViewModel : ViewModelBase
	{
		private Quiz _currentQuiz;

		public SettingsWindowViewModel(Quiz quiz)
		{
			_currentQuiz = quiz;
			CloseCommand = new Command(obj => CloseAction());
		}

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
	}
}
