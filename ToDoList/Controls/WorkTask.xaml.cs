using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using ToDoList.Core;
using ToDoList.Core.Helpers;
using ToDoList.Windows;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for WorkTask.xaml
    /// </summary>
    public partial class WorkTask : UserControl
    {
        public WorkTask()
        {
            InitializeComponent();            
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
                HandleDoubleClick();
        }

        private void HandleDoubleClick()
        {
            var nextWindow = new TaskEditorWindow(DataContext as WorkTaskViewModel);
            nextWindow.ShowDialog();        
        }
    }
}
