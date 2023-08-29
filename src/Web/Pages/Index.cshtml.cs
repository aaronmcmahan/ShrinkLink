using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Data;
using Web.Models;

namespace Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _db;
    private readonly IHashids _hashids;

    [BindProperty]
    public string FullUrl { get; set; }

    public Link? Link { get; set; }
    public string ComputedUrl { get; set; }

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext db, IHashids hashids)
    {
        _logger = logger;
        _db = db;
        _hashids = hashids;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        bool isUrl = Uri.TryCreate(FullUrl, UriKind.Absolute, out var result)
            && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);

        if (!isUrl)
        {
            ModelState.AddModelError("InvalidInput", "Please enter a valid URI");

            return Page();
        }

        string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
        var linkInDb = _db.Links.SingleOrDefault(x => x.FullUrl == FullUrl);

        if (linkInDb is not null)
        {
            Link = linkInDb;
            ComputedUrl = baseUrl + _hashids.Encode(linkInDb.Id);

            return Page();
        }

        Link = new Link(FullUrl);
        await _db.Links.AddAsync(Link);
        await _db.SaveChangesAsync();
        ComputedUrl = baseUrl + _hashids.Encode(Link.Id);

        return Page();
    }
}
