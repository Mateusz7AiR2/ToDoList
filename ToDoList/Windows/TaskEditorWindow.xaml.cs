using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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

namespace ToDoList.Windows
{
    public partial class TaskEditorWindow : Window
    {
        public TaskEditorWindow(WorkTaskViewModel task)
        {
            InitializeComponent();
            DataContext = task;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

