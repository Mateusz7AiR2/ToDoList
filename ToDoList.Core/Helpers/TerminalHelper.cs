using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListDatabase.Classes;

namespace ToDoList.Core.Helpers
{
    public class TerminalHelper
    {     
        public static List<WorkTaskViewModel> GetTasksFromDataBase()
        {
            List<WorkTaskViewModel> createdWorkTaskList = new List<WorkTaskViewModel>();

            foreach (var task in DatabaseLocator.ModelSource.GetWorkTasks())
            {
                createdWorkTaskList.Add(new WorkTaskViewModel(task));
            }

            return createdWorkTaskList;
        }

        public static List<DateTime> GetDaysWithTasks()
        {
            return  DatabaseLocator.ModelSource.GetWorkTasks()
                .Where (x => !x.IsCompleted)
                .Select(x => x.CreatedDate.Date)
                .Distinct()
                .ToList(); 
        }
    }
}
