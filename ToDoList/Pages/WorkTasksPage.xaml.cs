using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using ToDoList.Core;
using ToDoList.Core.Helpers;

namespace ToDoList
{
    public partial class WorkTasksPage : Page
    {
        public WorkTasksPage()
        {
            InitializeComponent();
            DataContext = new WorkTasksPageViewModel();           
            (DataContext as WorkTasksPageViewModel).NotificationRequested += ViewModel_NotificationRequested;
            (DataContext as WorkTasksPageViewModel).ErrorOccurred += ViewModel_ErrorOccurred;

            myCalendar.SelectedDatesChanged += MyCalendar_SelectedDateChanged;
            myCalendar.DisplayDateChanged += MyCalendar_DisplayDateChanged;
            myCalendar.Loaded += MyCalendar_Loaded;

        }

        private void ViewModel_NotificationRequested(object sender, NotificationEventArgs e)
        {
            MessageBox.Show(
                $"Reminder: {e.TodoEvent.Description} is happening at {e.TodoEvent.CreatedDate}",
                "Information about task " + e.TodoEvent.Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        private void ViewModel_ErrorOccurred(object sender, string e)
        {
            MessageBox.Show(e, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void MyCalendar_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            HighlightTaskDates();
        }

        private void MyCalendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            HighlightTaskDates();
        }

        private void MyCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            HighlightTaskDates();
        }

        private void HighlightTaskDates()
        {
            var taskDates = TerminalHelper.GetDaysWithTasks();

            foreach (var dayButton in FindVisualChildren<CalendarDayButton>(myCalendar))
            {
                if (dayButton.DataContext is DateTime day && taskDates.Contains(day.Date))
                {
                    if (day.Date < DateTime.Now.Date)
                        dayButton.Background = Brushes.DarkRed;
                    else
                        dayButton.Background = Brushes.LightGreen;
                }
                else
                    dayButton.Background = null;
            }
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button b && b.DataContext is WorkTasksPageViewModel wtpvm)
            {
                if (string.IsNullOrEmpty(wtpvm.NewWorkTaskTitle) || string.IsNullOrEmpty(wtpvm.NewWorkTaskDescription))
                    MessageBox.Show("Incorrect Title or Description");
                else
                {
                    var nextWindow = new TimePickerWindow(wtpvm);
                    nextWindow.ShowDialog();
                }
            }
        }
    }
}
