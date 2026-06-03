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

        // Liste statique des biens — fonctionne sans base de données
        private static List<Bien> GetBiensStatiques() => new List<Bien>
        {
            new Bien {
                Id = 1,
                Titre = "Appartement T3 avec terrasse panoramique",
                Type = "Appartement",
                Ville = "Famalicão",
                Quartier = "Joane",
                Prix = 285000,
                Surface = 130,
                NbPieces = 3,
                NbSdb = 2,
                Reference = "HS-T3-JOANE",
                Statut = "Disponible",
                Visible = true,
                ImagePrincipale = "/images/Sala_comum_dia.png",
                Description = "Appartement T3 neuf avec grande terrasse panoramique et vue sur les collines du Norte. Finitions premium, cuisine équipée, 2 parkings.",
                Etat = "Neuf",
                Slug = "t3-joane-famalicao",
                DateAjout = new DateTime(2026, 1, 1)
            }
            // Ajoutez vos prochains biens ici en copiant le bloc ci-dessus
        };

        // GET /Biens
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
            // On essaie d'abord la base de données
            // Si elle est vide ou indisponible, on utilise les données statiques
            List<Bien> biens;

            try
            {
                var query = _context.Biens
                    .Where(b => b.Visible)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(location))
                    query = query.Where(b =>
                        b.Ville.Contains(location) ||
                        b.Quartier.Contains(location));

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

                biens = await query.ToListAsync();

                // Si la base est vide, on utilise les données statiques
                if (!biens.Any())
                    biens = GetBiensStatiques();
            }
            catch
            {
                // Si la base est indisponible (Railway), données statiques
                biens = GetBiensStatiques();
            }

            // Filtres côté mémoire si on utilise les données statiques
            if (!string.IsNullOrWhiteSpace(location))
                biens = biens.Where(b =>
                    b.Ville.Contains(location, StringComparison.OrdinalIgnoreCase) ||
                    b.Quartier.Contains(location, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(type))
                biens = biens.Where(b => b.Type == type).ToList();

            if (piecesMin.HasValue)
                biens = biens.Where(b => b.NbPieces >= piecesMin).ToList();

            if (prixMin.HasValue)
                biens = biens.Where(b => b.Prix >= prixMin).ToList();

            if (prixMax.HasValue)
                biens = biens.Where(b => b.Prix <= prixMax).ToList();

            biens = sort switch
            {
                "asc" => biens.OrderBy(b => b.Prix).ToList(),
                "desc" => biens.OrderByDescending(b => b.Prix).ToList(),
                _ => biens.OrderByDescending(b => b.DateAjout).ToList()
            };

            return View("~/Views/Biens/Index.cshtml", biens);
        }

        // GET /Biens/Detail/t3-joane-famalicao
        public async Task<IActionResult> Detail(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return NotFound();

            Bien bien = null;

            try
            {
                bien = await _context.Biens
                    .FirstOrDefaultAsync(b => b.Slug == slug && b.Visible);
            }
            catch { }

            // Si pas en base, cherche dans les statiques
            if (bien == null)
                bien = GetBiensStatiques().FirstOrDefault(b => b.Slug == slug);

            if (bien == null) return NotFound();

            return View("~/Views/Biens/Detail.cshtml", bien);
        }

        // GET /Biens/T3Joane
        public IActionResult T3Joane()
        {
            return View("~/Views/Biens/T3Joane.cshtml");
        }
    }
}
