namespace SimbirQuiz.Infrastructure
{
	public interface IDialogService
	{
		string FilePath { get; set; }

		bool OpenFileDialog();
		bool SaveFileDialog(string fileName);
	}
}
