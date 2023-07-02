using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoList.Common.Db;

namespace ToDoListApp.Pages
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public TaskEntity? EditedTask { get; set; }
        public TaskEntity TaskToEdit { get; set; }
        private ToDoListContext? _db;

        public EditModel(ToDoListContext context)
        {
            _db = context;
        }

       /* public async Task<IActionResult> OnGetAsync(int? taskId)
        {  
            if(taskId is null)
            {
                return NotFound();
            }
               
            TaskEntity tempTask = await _db.Tasks.SingleOrDefaultAsync(t => t.TaskId == taskId);

            if (tempTask == null)
            {
                return NotFound();
            }
            
            TaskToEdit = tempTask;

            return Page();
        }*/

        public async Task<IActionResult> OnPostEditAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            if (EditedTask != null)
            {
                _db.Tasks.Attach(EditedTask).State = EntityState.Modified;
            }

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExist(EditedTask.TaskId))
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

        public async Task<IActionResult> OnGetSetTaskToEditAsync(int? taskId)
        {
            if (taskId is null)
            {
                return NotFound();
            }

            TaskEntity tempTask = await _db.Tasks.SingleOrDefaultAsync(t => t.TaskId == taskId);

            if (tempTask == null)
            {
                return NotFound();
            }

            TaskToEdit = tempTask;

            return Page();
        }

        /*public void OnGetSetTaskToEdit(int taskId)
        {
             TaskToEdit = _db.Tasks.SingleOrDefault(t => t.TaskId == taskId);
        }*/

        private bool TaskExist(int taskId)
        {
            return _db.Tasks.Any(t =>  t.TaskId == taskId);
        }
    }
}
