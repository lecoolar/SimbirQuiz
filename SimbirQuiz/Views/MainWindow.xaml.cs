using System.Windows;

using SimbirQuiz.Infrastructure;
using SimbirQuiz.ViewModels;

namespace SimbirQuiz.Views
{
	
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			MainWindowViewModel vm = new MainWindowViewModel(new DefaultDialogService(), new JsonFileService());
			if (vm.CloseAction == null)
				vm.CloseAction = new System.Action(Close);
			DataContext = vm;
		}

		private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => DragMove();
	}
}
