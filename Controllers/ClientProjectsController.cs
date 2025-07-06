using Human_Evolution.Models;
using Microsoft.AspNetCore.Mvc;


public class ClientProjectsController : Controller
{
    // Temporaire : tu peux connecter à ta base plus tard
    public IActionResult Index()
    {
        var projets = new List<ClientProject>
        {
            new ClientProject
            {
                Id = 1,
                TitreProjet = "6 Villas à Viseu",
                Localisation = "Viseu",
                Description = "Construction de 6 villas modernes avec piscine.",
                EtatAvancement = "Chantier",
                BudgetPrevisionnel = 800000,
                DateDebut = DateTime.Now.AddMonths(-2),
                Photos = new List<string> { "/images/project04.jpg" }
            },
            new ClientProject
            {
                Id = 2,
                TitreProjet = "Ruine à réhabiliter – Douro",
                Localisation = "Douro",
                Description = "Projet de restauration dans un site classé UNESCO.",
                EtatAvancement = "Étude",
                BudgetPrevisionnel = 250000,
                DateDebut = DateTime.Now.AddMonths(-1),
                Photos = new List<string> { "/images/project05.jpg" }
            }
        };

        return View(projets);
    }
}
