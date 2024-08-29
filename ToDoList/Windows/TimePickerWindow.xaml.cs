using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToDoList.Core;
using ToDoList.Core.Helpers;

namespace ToDoList
{
    public partial class TimePickerWindow : Window
    {
        private WorkTasksPageViewModel _viewModel;
        public int SelectedHour { get; private set; }
        public int SelectedMinute { get; private set; }
        public List<int> Hours { get; } = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        public List<int> Minutes { get; } = new List<int> { 0, 15, 30, 45 };

        public TimePickerWindow(WorkTasksPageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = this; 
            _viewModel = viewModel;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedHour = (int)HourComboBox.SelectedItem;
            SelectedMinute = (int)MinuteComboBox.SelectedItem;

            _viewModel.AddNewTaskCommand.Execute(new AddCommandArgs(SelectedHour, SelectedMinute));

            DialogResult = true;
            Close();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close(); 
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowHeight = this.Height;
            double windowWidth = this.Width;
            double newLeft = (screenWidth - windowWidth);
            double newTop = (screenHeight - windowHeight) / 2;

            this.Left = newLeft;
            this.Top = newTop - 15;
        }
    }
}
