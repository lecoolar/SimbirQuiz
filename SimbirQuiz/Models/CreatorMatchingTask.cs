namespace SimbirQuiz.Models
{
	public class CreatorMatchingTask : ICreatorTask
	{
		public ITestTask CreateTask() => new MatchingTask();
	}
}
