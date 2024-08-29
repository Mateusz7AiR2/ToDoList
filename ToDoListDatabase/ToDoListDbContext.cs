using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ToDoListDatabase.Classes;

namespace ToDoListDatabase
{
    public interface IModelSource
    {
        void Initialize();
        IEnumerable<WorkTask> GetWorkTasks();
        OperationResult AddTask(WorkTask task);
        OperationResult RemoveTask(WorkTask task);
        OperationResult StoreTask(WorkTask task);
    }

    public class OperationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ToDoListDbContext : DbContext
    {
        public DbSet<WorkTask> WorkTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Filename={Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "SQLToDo.sqlite")}");
        }
    }

    public class DbModelSource : IModelSource
    {
        public void Initialize()
        {
            try
            {
                using (var context = new ToDoListDbContext())
                {
                    context.Database.EnsureCreated();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error initializing database", ex);
            }

        }

        public IEnumerable<WorkTask> GetWorkTasks()
        {
            List<WorkTask> workTasks = new List<WorkTask>();
            try
            {
                using (var context = new ToDoListDbContext())
                {
                    workTasks.AddRange(context.WorkTasks);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving work tasks", ex);
            }
            return workTasks;
        }

        public OperationResult AddTask(WorkTask task)
        {
            try
            {
                using (var context = new ToDoListDbContext())
                {
                    context.WorkTasks.Add(task);
                    context.SaveChanges();
                }
                return null;
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Database update error: " + dbEx.Message
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "General error: " + ex.Message                  
                };
            }
        }     

        public OperationResult RemoveTask(WorkTask task)
        {
            try
            {
                using (var context = new ToDoListDbContext())
                {
                    context.WorkTasks.Remove(task);
                    context.SaveChanges();
                }
                return null;
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Database update error: " + dbEx.Message
                  
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "General error: " + ex.Message                   
                };

            }
        }

        public OperationResult StoreTask(WorkTask task)
        {
            try
            {
                using (var context = new ToDoListDbContext())
                {
                    context.Entry(task).State = EntityState.Modified;
                    context.SaveChanges();
                }
                return null;
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Database update error: " + dbEx.Message
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "General error: " + ex.Message
                };
            }
        }
    }
}
