using System.Windows;

using SimbirQuiz.Models;
using SimbirQuiz.ViewModels;

namespace SimbirQuiz.Views
{
	
	public partial class ResultWindow : Window
	{
		public ResultWindow(int userScore, Quiz quiz)
		{
			InitializeComponent();
			ResultWindowViewModel vm = new ResultWindowViewModel(userScore, quiz);
			if (vm.CloseAction == null)
				vm.CloseAction = new System.Action(Close);
			DataContext = vm;
		}

		private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();
	}
}
