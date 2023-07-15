using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
                .Where(t => t.DueDate > DateTime.Today && !t.IsCompleted);
        }

        public IActionResult OnPostEdit(int? id)
        {
            return RedirectToPage("Edit", "SetTaskToEdit", new { taskId = id });
        }


        public async Task<IActionResult> OnPostDone(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            TaskEntity? task = await _db.Tasks.SingleOrDefaultAsync(t => t.TaskId == id);

            if (task == null)
                return NotFound();

            task.IsCompleted = true;
            _db.Tasks.Attach(task).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExist(task.TaskId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/UpcomingTasks");
        }

        private bool TaskExist(int id)
        {
            return _db.Tasks.Any(t => t.TaskId == id);
        }
    }
}
