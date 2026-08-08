using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Mvc.Models;

public class TicketCreateEditViewModel
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Priority { get; set; }
    [Required]
    public string Status { get; set; }
    [Required]
    public string RaisedBy { get; set; }
    [Required]
    public DateTime CreatedDate { get; set; }

}
