﻿namespace SimbirQuiz.Models
{
	public class CreatorSequenceTask : ICreatorTask
	{
		public ITestTask CreateTask() => new SequenceTask();
	}
}
