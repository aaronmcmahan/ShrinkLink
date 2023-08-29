using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models;

public class Link
{
    public Link(string fullUrl)
    {
        FullUrl = fullUrl;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Number")]
    [Column("LinkId")]

    public int Id { get; set; }

    [Url]
    public string FullUrl { get; set; } = string.Empty;
}
