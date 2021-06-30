using SimbirQuiz.Models;

namespace SimbirQuiz.Infrastructure
{
	public interface IFileService
	{
		Quiz Open(string filename);
		void Save(string filename, Quiz savingQuiz);
	}
}
