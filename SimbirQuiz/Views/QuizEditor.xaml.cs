using System.Collections.Generic;
using System.Windows;

using SimbirQuiz.Infrastructure;
using SimbirQuiz.ViewModels;

namespace SimbirQuiz.Views
{
	
	public partial class QuizEditor : Window
	{
		public QuizEditor()
		{
			InitializeComponent();
			List<object> formsToInteract = new List<object>();
			formsToInteract.Add(PicturesWrap);
			formsToInteract.Add(MediaPlayer);
			formsToInteract.Add(VolumeSlider);
			QuizEditorViewModel vm = new QuizEditorViewModel(new DefaultDialogService(), new JsonFileService(), formsToInteract);
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
