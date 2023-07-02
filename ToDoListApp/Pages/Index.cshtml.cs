using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using ToDoList.Common.Db;

namespace ToDoListApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private ToDoListContext _db;
        public IEnumerable<TaskEntity>? Tasks { get; set; }
        public IEnumerable<TaskEntity>? OverdueTasks { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ToDoListContext context)
        {
            _logger = logger;
            _db = context;
        }

        public void OnGet()
        {
            foreach(TaskEntity task in _db.Tasks)
            {
                if (task.DueDate < DateTime.UtcNow)
                {
                    task.IsOverdue = true;
                    _db.Tasks.Attach(task).State = EntityState.Modified;
                }
            }

            Tasks = _db.Tasks
                .Where(t => t.IsCompleted == false && t.DueDate == DateTime.Today)
                .OrderByDescending(t => t.Priority);

            OverdueTasks = _db.Tasks
                .OrderBy(t => t.DueDate)
                .ThenBy(t => t.Priority);
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
            catch(DbUpdateConcurrencyException)
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

            return RedirectToPage("/index");
        }

        public IActionResult OnPostDelete(int? id)
        {
            if(!id.HasValue)
            {
                return BadRequest("Need an exist id");
            }

            TaskEntity? task = _db.Tasks.SingleOrDefault(t => t.TaskId == id);

            if (task == null)
            {
                return NotFound($"Task with id:{id} not found");
            }

            _db.Tasks.Remove(task);
            _db.SaveChanges();

            return RedirectToPage("/index");
        }

        public IActionResult OnPostEdit(int? id)
        {
            return RedirectToPage("Edit", "SetTaskToEdit", new { taskId = id });
        }

        private bool TaskExist(int id)
        {
            return _db.Tasks.Any(t => t.TaskId == id);
        }
    }
}