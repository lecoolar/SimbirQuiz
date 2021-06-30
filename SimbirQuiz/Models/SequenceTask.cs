using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using SimbirQuiz.Infrastructure;

namespace SimbirQuiz.Models
{
	public class SequenceTask : ITestTask
	{
		private string _question = "Введите вопрос...";
		private int _point = 1;
		private ObservableCollection<SequenceAnswer> _answers;
		private List<string> _videos;
		private List<string> _audios;
		private ObservableCollection<string> _images;

		public SequenceTask() => _answers = new ObservableCollection<SequenceAnswer>();

		public string Question { get => _question; set => _question = value; }
		public int Point { get => _point; set => _point = value; }
		public ObservableCollection<SequenceAnswer> Answers { get => _answers; }

		public List<string> Videos { get => _videos; set => _videos = value; }
		public List<string> Audios { get => _audios; set => _audios = value; }
		public ObservableCollection<string> Images { get => _images; set => _images = value; }

		public void AddNewAnswer() => _answers.Add(new SequenceAnswer());
		public void RemoveAnswer(Answer answer) => _answers.Remove(answer as SequenceAnswer);
		public int GetResult()
		{
			int countRightAnswers = (from ans in Answers where ans.SequenceNumber == ans.UserSequenceNumber select ans).Count();
			return countRightAnswers * Point;
		}
		public int GetMaxPointsPerTask() => Answers.Count * Point;

		public void ShuffleAnswers()
		{
			Random rng = new Random();
			rng.Shuffle(Answers);
		}
	}
}
