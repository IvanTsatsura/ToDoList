using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoList.Common.Db;

namespace ToDoListApp.Pages
{
    public class UpcomingTasksModel : PageModel
    {
        private ToDoListContext _db;
        public IEnumerable<TaskEntity>? UpcomingTasks { get; set; }
        public UpcomingTasksModel(ToDoListContext context)
        {
            _db = context;
        }
        public void OnGet()
        {
            UpcomingTasks = _db.Tasks
                .Where(t => t.DueDate > DateTime.Today);
        }
    }
}
