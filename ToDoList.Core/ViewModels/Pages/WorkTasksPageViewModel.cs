using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ToDoList.Core.Helpers;
using ToDoListDatabase.Classes;

namespace ToDoList.Core
{
    public class AddCommandArgs
    {
        public int hours;
        public int minutes;

        public AddCommandArgs(int hours, int minutes)
        {
            this.hours = hours;
            this.minutes = minutes;
        }
    }
    public class NotificationEventArgs : EventArgs
    {
        public WorkTaskViewModel TodoEvent { get; }

        public NotificationEventArgs(WorkTaskViewModel todoEvent)
        {
            TodoEvent = todoEvent;
        }
    }

    public class WorkTasksPageViewModel : BaseViewModel
    {
        private List<WorkTaskViewModel> workTaskList;
        private DispatcherTimer notificationTimer;
        public event EventHandler<NotificationEventArgs> NotificationRequested;
        public event EventHandler<string> ErrorOccurred;

        public ObservableCollection<WorkTaskViewModel> WorkTaskListLayout { get; set; } = new();
       
        private DateTime selectedDate;
        public DateTime SelectedDate
        { 
            get => selectedDate ;
            set
            {
                if (selectedDate != value)
                {
                    selectedDate = value;
                    OnSelectedDateChanged();
                }
            }
        }
    
        private string newWorkTaskTitle;
        public string NewWorkTaskTitle 
        { 
            get => newWorkTaskTitle;
            set
            {
                if (newWorkTaskTitle != value)
                {
                    newWorkTaskTitle = value;
                    OnPropertyChanged(nameof(NewWorkTaskTitle));
                }
            }
        }

        private string newWorkTaskDescription;
        public string NewWorkTaskDescription 
        {
            get => newWorkTaskDescription;
            set
            {
                if (newWorkTaskDescription != value)
                {
                    newWorkTaskDescription = value;
                    OnPropertyChanged(nameof(NewWorkTaskDescription));
                }
            }
        }
        public ICommand AddNewTaskCommand { get; set; }
        public ICommand DeleteSelectedTasksCommand {  get; set; }


        public WorkTasksPageViewModel()
        {
            AddNewTaskCommand = new RelayCommand(AddNewTask);
            DeleteSelectedTasksCommand = new RelayCommand(DeleteSelectedTasks);

            selectedDate = DateTime.Today;

            StartNotificationTimer();
            AcquireWorkTasks();
        }

        private void StartNotificationTimer()
        {
            notificationTimer = new DispatcherTimer();
            notificationTimer.Interval = TimeSpan.FromSeconds(1); 
            notificationTimer.Tick += CheckForUpcomingEvents;
            notificationTimer.Start();
        }

        private void CheckForUpcomingEvents(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            foreach (var todoTask in workTaskList)
            {
                var targetTask = workTaskList.FirstOrDefault(x => x.Id == todoTask.Id);
                if (targetTask == null)
                    continue;
                double minutesDifference = (todoTask.CreatedDate - now).TotalMinutes;

                if (!targetTask.IsNotified && minutesDifference <= 60 && minutesDifference > 0)
                {
                    targetTask.IsNotified = true;
                    var resultStore = DatabaseLocator.ModelSource.StoreTask(targetTask.WorkTask);
                    if (resultStore != null)
                    {
                        OnErrorOccurred(resultStore.ErrorMessage);
                    }
                    OnNotificationRequested(todoTask);
                }
            }
        }

        protected virtual void OnNotificationRequested(WorkTaskViewModel todoEvent)
        {
            NotificationRequested?.Invoke(this, new NotificationEventArgs(todoEvent));
        }

        protected virtual void OnErrorOccurred(string errorMessage)
        {
            ErrorOccurred?.Invoke(this, errorMessage);
        }

        private void Wt_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(WorkTask.IsCompleted) || e.PropertyName == nameof(WorkTask.CreatedDate))
            {
                SortWorkTasks();
            }
        }

        private void AddNewTask(object? param)
        {
            var args = param as AddCommandArgs;

            if (args == null) 
            {
                return;
            }

            var task = new WorkTask
            {
                Title = NewWorkTaskTitle,
                Description = NewWorkTaskDescription,
                CreatedDate = SelectedDate + TimeSpan.FromHours(args.hours) + TimeSpan.FromMinutes(args.minutes),
            };
            var resultAdd = DatabaseLocator.ModelSource.AddTask(task);

            if (resultAdd != null)
            {
                OnErrorOccurred(resultAdd.ErrorMessage);
            }
            AcquireWorkTasks();

            NewWorkTaskTitle = string.Empty;
            NewWorkTaskDescription = string.Empty;
        }

        private void DeleteSelectedTasks()
        {
            var selectedTasks = workTaskList.Where(x =>x.IsSelected).ToList();
            foreach (var task in selectedTasks)
            {    
                var findObject = DatabaseLocator.ModelSource.GetWorkTasks().FirstOrDefault( x=> x.Id == task.Id);
                if (findObject != null)
                {
                    var resultRemove = DatabaseLocator.ModelSource.RemoveTask(findObject);
                    if (resultRemove != null) 
                    { 
                        OnErrorOccurred(resultRemove.ErrorMessage); 
                    }
                }
            }

            AcquireWorkTasks();
        }

        public void AcquireWorkTasks()
        {
            workTaskList = TerminalHelper.GetTasksFromDataBase();

            foreach (var wt in workTaskList)
            {
                wt.PropertyChanged += Wt_PropertyChanged;
            }

            SortWorkTasks();
        }

        public void SortWorkTasks()
        {
            WorkTaskListLayout.Clear();

            foreach(var e in workTaskList
                    .Where(x => x.CreatedDate.Year == SelectedDate.Year &&
                            x.CreatedDate.Month == SelectedDate.Month &&
                            x.CreatedDate.Day == SelectedDate.Day)
                    .OrderBy(x => x.IsCompleted)
                    .ThenBy(x => x.CreatedDate))
            {
                WorkTaskListLayout.Add(e);
            }
        }
        public void OnSelectedDateChanged()
        {
            SortWorkTasks();
        }
    }
}
