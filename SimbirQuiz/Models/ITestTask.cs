using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SimbirQuiz.Models
{
	public interface ITestTask
	{
		string Question { get; set; }
		int Point { get; set; }

		List<string> Videos { get; set; }
		List<string> Audios { get; set; }
		ObservableCollection<string> Images { get; set; }

		//void AddVideo();
		//void AddAudio();
		//void AddImage();

		//void RemoveVideo(string video);
		//void RemoveAudio(string audio);
		//void RemoveImage(string image);

		void AddNewAnswer();

		void RemoveAnswer(Answer answer);
		int GetResult();
		void ShuffleAnswers();
		int GetMaxPointsPerTask();
	}
}
