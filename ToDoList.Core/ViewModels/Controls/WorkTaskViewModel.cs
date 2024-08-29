using System.Windows.Controls;
using System.Windows.Input;
using ToDoList.Core.Helpers;
using ToDoListDatabase.Classes;

namespace ToDoList.Core
{
    public class WorkTaskViewModel : BaseViewModel
    {
        public WorkTaskViewModel(WorkTask workTask)
        {
            this.workTask = workTask;
            ChangeState = new RelayCommand(ChangeStateTask);
        }
        public WorkTaskViewModel() { }

        private WorkTask workTask;

        public int Id => workTask.Id;
        public string Title
        {
            get => workTask.Title;
            set
            {
                if (workTask.Title != value)
                {
                    workTask.Title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        public string Description 
        { 
            get => workTask.Description;
            set
            {
                if (workTask.Description != value)
                {
                    workTask.Description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
        public DateTime CreatedDate 
        { 
            get => workTask.CreatedDate;
            set
            {
                if (workTask.CreatedDate != value)
                {
                    workTask.CreatedDate = value;
                    OnPropertyChanged(nameof(CreatedDate));
                }

            }
        }
      
        public bool IsNotified 
        { 
            get => workTask.IsNotified;
            set
            {
                if (workTask.IsNotified != value)
                {
                    workTask.IsNotified = value;
                    OnPropertyChanged(nameof(IsNotified));
                }
            }
        }

        public bool IsCompleted
        {
            get => workTask.IsCompleted;
            set
            {
                if (workTask.IsCompleted != value)
                {
                    WorkTask.IsCompleted = value;
                    OnPropertyChanged((nameof(IsCompleted)));
                }
            }
        }
        public bool IsSelected { get; set; }
        public string CreatedTime => CreatedDate.ToString("hh:mm tt");



        public ICommand ChangeState {  get; set; }

        private void ChangeStateTask()
        {
            DatabaseLocator.ModelSource.StoreTask(workTask);
        }
        public WorkTask WorkTask => workTask;
    }
}
