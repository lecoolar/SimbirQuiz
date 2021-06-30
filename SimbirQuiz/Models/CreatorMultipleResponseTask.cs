namespace SimbirQuiz.Models
{
	public class CreatorMultipleResponseTask : ICreatorTask
	{
		public ITestTask CreateTask() => new MultipleResponseTask();
	}
}
