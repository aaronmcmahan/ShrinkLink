using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using static System.Net.Mime.MediaTypeNames;

namespace Web.Models;

public class Link : IEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Number")]
    [Column("LinkId")]
    public int Id { get; set; }
    public string Slug { get; set; } = null!;

    [Url]
    public string FullUrl { get; set; } = null!;

    public static Link Create(string fullUri)
    {
        return new Link
        {
            FullUrl = fullUri,
            Slug = NewSlug()
        };
    }

    static string NewSlug()
    {
        Random random = new();
        int length = 6;
        const string chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ0123456789";

        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
