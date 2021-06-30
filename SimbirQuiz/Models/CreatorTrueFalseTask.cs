namespace SimbirQuiz.Models
{
	public class CreatorTrueFalseTask : ICreatorTask
	{
		public ITestTask CreateTask() => new TrueFalseTask();
	}
}
