using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoList.Common.Db;

namespace ToDoListApp.Pages
{
    public class CompletedTasksModel : PageModel
    {
        private ToDoListContext _db;
        public IEnumerable<TaskEntity>? Tasks { get; set; }

        public CompletedTasksModel(ToDoListContext context)
        {
            _db = context;
        }

        public void OnGet()
        {
            Tasks = _db.Tasks
                .Where(t => t.IsCompleted == true)
                .OrderBy(t => t.DueDate);
        }

        public async Task<IActionResult> OnPostRemove(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            TaskEntity task = await _db.Tasks.SingleOrDefaultAsync(t => t.TaskId == id);

            if (task == null)
                return NotFound();

            _db.Remove(task);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExist(task.TaskId))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToPage("/CompletedTasks");
        }

        private bool TaskExist(int taskId)
        {
            return _db.Tasks.Any(t => t.TaskId == taskId);
        }
    }
}
