﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using SimbirQuiz.Infrastructure;

namespace SimbirQuiz.Models
{
	public class MultipleResponseTask : ITestTask
	{
		private string _question = "Введите вопрос...";
		private int _point = 1;
		private bool _swapAnswer;
		private ObservableCollection<MultipleAnswer> _answers;
		private string _media;
		//private string _audio;
		private ObservableCollection<string> _images = new ObservableCollection<string>();

		public MultipleResponseTask() => _answers = new ObservableCollection<MultipleAnswer>();

		public string Question { get => _question; set => _question = value; }
		public int Point { get => _point; set => _point = value; }
		public bool SwapAnswer { get => _swapAnswer; set => _swapAnswer = value; }
		public ObservableCollection<MultipleAnswer> Answers { get => _answers; }

		public string Media { get => _media; set => _media = value; }
		//public string Audio { get => _audio; set => _audio = value; }
		public ObservableCollection<string> Images { get => _images; set => _images = value; }

		public void AddNewAnswer() => _answers.Add(new MultipleAnswer());
		public void RemoveAnswer(Answer answer) => _answers.Remove(answer as MultipleAnswer);
		public int GetResult()
		{
			int countRightAnswers = (from ans in Answers where ans.UserChoice && ans.IsRight select ans).Count();
			return countRightAnswers * Point;
		}

		public int GetMaxPointsPerTask()
		{
			int maxPointsPerTask = 0;
			foreach (var answer in Answers)
				if (answer.IsRight)
					maxPointsPerTask += Point;
			return maxPointsPerTask;
		}

		public void ShuffleAnswers()
		{
			if (SwapAnswer)
			{
				Random rng = new Random();
				rng.Shuffle(Answers);
			}
		}
	}
}
