using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Common.Db;

public class TaskEntity
{
    [Key]
    public int TaskId { get; set; }
    [Required]
    [StringLength(30)]
    [Column(TypeName = "nchar (40)")]
    public string TaskName { get; set; }
    [StringLength(150)]
    public string? Description { get; set; }
    public Priority? Priority { get; set; }
    [Column(TypeName = "datetime")]
    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }
    [Column(TypeName = "bit")]
    public bool IsOverdue { get; set; } = false;
    [Column(TypeName = "bit")]
    public bool IsCompleted { get; set; } = false;
}