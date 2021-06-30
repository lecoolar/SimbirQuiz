using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using SimbirQuiz.Infrastructure;

namespace SimbirQuiz.Models
{
	public class TrueFalseTask : ITestTask
	{
		private string _question = "Введите вопрос...";
		private int _point = 1;
		private bool _swapAnswer;
		private ObservableCollection<TrueFalseAnswer> _answers;
		private List<string> _videos;
		private List<string> _audios;
		private ObservableCollection<string> _images;

		public TrueFalseTask() => _answers = new ObservableCollection<TrueFalseAnswer>();

		public string Question { get => _question; set => _question = value; }
		public int Point { get => _point; set => _point = value; }
		public bool SwapAnswer { get => _swapAnswer; set => _swapAnswer = value; }
		public ObservableCollection<TrueFalseAnswer> Answers { get => _answers; }

		public List<string> Videos { get => _videos; set => _videos = value; }
		public List<string> Audios { get => _audios; set => _audios = value; }
		public ObservableCollection<string> Images { get => _images; set => _images = value; }

		public void AddNewAnswer() => _answers.Add(new TrueFalseAnswer());
		public void RemoveAnswer(Answer answer) => _answers.Remove(answer as TrueFalseAnswer);
		public int GetResult()
		{
			int countRightAnswers = (from ans in Answers where ans.UserChoice && ans.IsTrue select ans).Count();
			return countRightAnswers * Point;
		}

		public int GetMaxPointsPerTask()
		{
			int maxPointsPerTask = 0;
			foreach (var answer in Answers)
				if (answer.IsTrue)
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
