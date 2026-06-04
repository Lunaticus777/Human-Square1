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

        // ─────────────────────────────────────────────
        //  CATALOGUE STATIQUE — fonctionne sans base
        //  Ajoutez vos biens ici
        // ─────────────────────────────────────────────
        public static List<Bien> GetBiensStatiques() => new List<Bien>
        {
            new Bien {
                Id = 1,
                Titre = "Terracos de Joane  T0, T2, T3 neufs",
                Type = "Appartement",
                Ville = "Famalicao",
                Quartier = "Joane",
                Prix = 113520,
                Surface = 144,
                NbPieces = 3,
                NbSdb = 2,
                Reference = "HS-JOANE-2024",
                Statut = "Disponible",
                Visible = true,
                ImagePrincipale = "/images/joane.jpeg",
                Description = "Programme residentiel neuf — 15 lots T0, T2 et T3 avec terrasse panoramique. Certification A, parking inclus. A partir de 113 520 EUR.",
                Etat = "Neuf",
                Slug = "terracos-de-joane",
                DateAjout = new DateTime(2024, 6, 1)
            },

            new Bien {
                Id = 2,
                Titre = "Projet touristique Lomba - Douro",
                Type = "Terrain",
                Ville = "Gondomar", Quartier = "Lomba",
                Prix = 690000,
                Surface = 5115, NbPieces = 16, NbSdb = 0,
                Reference = "HS-LOMBA-2024",
                Statut = "Disponible", Visible = true,
                ImagePrincipale = "/images/lomba-01.png",
                Description = "Terrain 5115m2 avec PIP approuve - Complexe touristique 16 unites + restaurant panoramique au bord du Douro. 30 min de Porto.",
                Etat = "Neuf", Slug = "lomba-gondomar",
                DateAjout = new DateTime(2024, 3, 1)
            }
            // Ajoutez vos prochains biens ici

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
            List<Bien> biens;

            try
            {
                var query = _context.Biens.Where(b => b.Visible).AsQueryable();

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

                biens = await query.OrderByDescending(b => b.DateAjout).ToListAsync();

                if (!biens.Any())
                    biens = GetBiensStatiques();
            }
            catch
            {
                biens = GetBiensStatiques();
            }

            return View("~/Views/Biens/Index.cshtml", biens);
        }

        // GET /Biens/TerracosDeJoane
        public IActionResult TerracosDeJoane()
        {
            return View("~/Views/Biens/T3Joane.cshtml");
        }

        // GET /Biens/Detail/{slug}
        public async Task<IActionResult> Detail(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return NotFound();

            // Redirection directe pour les slugs connus
            if (slug == "terracos-de-joane")
                return RedirectToAction("TerracosDeJoane");

            Bien bien = null;
            try
            {
                bien = await _context.Biens
                    .FirstOrDefaultAsync(b => b.Slug == slug && b.Visible);
            }
            catch { }

            if (bien == null)
                bien = GetBiensStatiques().FirstOrDefault(b => b.Slug == slug);

            if (bien == null) return NotFound();

            return View("~/Views/Biens/Detail.cshtml", bien);
        }

        // Ancienne route conservée pour compatibilité
        public IActionResult T3Joane()
        {
            return RedirectToAction("TerracosDeJoane");
        }
        public IActionResult Lomba()
        {
            return View("~/Views/Biens/Lomba.cshtml");
        }
    }
}
