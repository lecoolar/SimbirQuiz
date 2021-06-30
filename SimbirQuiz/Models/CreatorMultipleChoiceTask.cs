namespace SimbirQuiz.Models
{
	public class CreatorMultipleChoiceTask : ICreatorTask
	{
		public ITestTask CreateTask() => new MultipleChoiceTask();
	}
}
