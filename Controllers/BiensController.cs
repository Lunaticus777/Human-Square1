using Human_Evolution.Data;
using Human_Evolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Human_Evolution.Controllers
{
    public class BiensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BiensController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
    string location = null,
    string type = null,
    int? piecesMin = null,
    decimal? prixMin = null,
    decimal? prixMax = null,
    decimal? surfMin = null,
    decimal? surfMax = null,
    string sort = null)
        {
            try
            {
                var query = _context.Biens
                    .Where(b => b.Visible)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(location))
                    query = query.Where(b => b.Ville.Contains(location) || b.Quartier.Contains(location));
                if (!string.IsNullOrWhiteSpace(type))
                    query = query.Where(b => b.Type == type);
                if (piecesMin.HasValue)
                    query = query.Where(b => b.NbPieces >= piecesMin);
                if (prixMin.HasValue)
                    query = query.Where(b => b.Prix >= prixMin);
                if (prixMax.HasValue)
                    query = query.Where(b => b.Prix <= prixMax);
                if (surfMin.HasValue)
                    query = query.Where(b => b.Surface >= surfMin);
                if (surfMax.HasValue)
                    query = query.Where(b => b.Surface <= surfMax);

                query = sort switch
                {
                    "asc" => query.OrderBy(b => b.Prix),
                    "desc" => query.OrderByDescending(b => b.Prix),
                    _ => query.OrderByDescending(b => b.DateAjout)
                };

                var biens = await query.ToListAsync();
                return View("~/Views/Biens/Index.cshtml", biens);
            }
            catch
            {
                return View("~/Views/Biens/Index.cshtml", new List<Human_Evolution.Models.Bien>());
            }
        }

        public async Task<IActionResult> Detail(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return NotFound();

            var bien = await _context.Biens
                .FirstOrDefaultAsync(b => b.Slug == slug && b.Visible);

            if (bien == null) return NotFound();

            return View("~/Views/Biens/Detail.cshtml", bien);
        }
        public IActionResult T3Joane()
        {
            return View("~/Views/Biens/T3Joane.cshtml");
        }
    }
}