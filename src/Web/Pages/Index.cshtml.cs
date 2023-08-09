using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Data;
using Web.Models;

namespace Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _db;

    [BindProperty]
    public string FullUrl { get; set; }

    public Link? Link { get; set; }
    public string ComputedUrl { get; set; }

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
        bool isUri = Uri.TryCreate(FullUrl, UriKind.Absolute, out var result)
            && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);

        if (!isUri)
        {
            ModelState.AddModelError("InvalidInput", "Please enter a valid URI");

            return Page();
        }

        var linkInDb = _db.Links.SingleOrDefault(x => x.FullUrl == FullUrl);

        if (linkInDb is not null)
        {
            Link = linkInDb;
            ComputedUrl = baseUrl + Link.Slug;

            return Page();
        }

        Link = Link.Create(FullUrl);
        ComputedUrl = baseUrl + Link.Slug;

        await _db.Links.AddAsync(Link);
        await _db.SaveChangesAsync();

        return Page();
    }
}
