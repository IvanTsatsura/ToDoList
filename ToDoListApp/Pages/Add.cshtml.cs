using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoList.Common.Db;

namespace ToDoListApp.Pages
{
    public class AddModel : PageModel
    {
        private ToDoListContext _db;
        [BindProperty]
        public TaskEntity? TaskToAdd { get; set; }

        public AddModel(ToDoListContext context)
        {
            _db = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if ((TaskToAdd != null) && ModelState.IsValid)
            {
                if (TaskToAdd.DueDate is null)
                    TaskToAdd.DueDate = DateTime.Now;
                if (TaskToAdd.Priority is null)
                    TaskToAdd.Priority = 0;
                _db.Add(TaskToAdd);
                _db.SaveChanges();
                return Page();
            }
            else
            {
                return Page();
            }
        }
    }
}
