using System.Collections.Generic;
using System.Windows;

using SimbirQuiz.Models;
using SimbirQuiz.ViewModels;

namespace SimbirQuiz.Views
{
	/// <summary>
	/// Логика взаимодействия для QuizRunner.xaml
	/// </summary>
	public partial class QuizRunner : Window
	{
		public QuizRunner(Quiz quiz)
		{
			InitializeComponent();
			List<object> formsToInteract = new List<object>();
			formsToInteract.Add(Images);
			formsToInteract.Add(Player);
			formsToInteract.Add(VolumeSlider);
			QuizRunnerViewModel vm = new QuizRunnerViewModel(quiz, formsToInteract);
			if (vm.CloseAction == null)
				vm.CloseAction = new System.Action(Close);
			if (vm.MinimizeAction == null)
				vm.MinimizeAction = new System.Action(() => WindowState = WindowState.Minimized);
			if (vm.MaximizeAction == null)
				vm.MaximizeAction = new System.Action(() => { if (WindowState == WindowState.Normal) WindowState = WindowState.Maximized; else WindowState = WindowState.Normal; });
			DataContext = vm;
		}

		private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();
	}
}
