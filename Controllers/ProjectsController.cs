using Human_Evolution.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Human_Evolution.Controllers
{
    public class ProjectsController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ActiveTab = "clients";

            var projetsClients = new List<Project>
            {
                new Project { Title = "7 Villas à Esposende", ImageUrl = "/images/project01b.png", Description = "Projet en bord de mer", ModalId = "modalProjet1" },
                new Project { Title = "10 Villas à Albufeira", ImageUrl = "/images/project02.png", Description = "Projet dans le sud", ModalId = "modalProjet2" },
                new Project { Title = "Villa à Barcelos", ImageUrl = "/images/project03.png", Description = "Projet individuel", ModalId = "modalProjet3" },
                new Project { Title = "6 Villas à Viseu", ImageUrl = "/images/project04.jpg", Description = "Projet de montagne", ModalId = "modalProjet5" },
                new Project { Title = "Projet immobilier touristique – Douro", ImageUrl = "/images/project06.jpg", Description = "Projet de réhabilitation", ModalId = "modalProjet6" }
            };

            return View("Index", projetsClients); // Utilise la même vue que Index
        }

        public IActionResult Disponibles()
        {
            ViewBag.ActiveTab = "disponibles";

            var projetsDispo = new List<Project>
            {
                new Project { Title = "Ruine à réhabiliter – Douro", ImageUrl = "/images/project06.jpg", Description = "Opportunité d’investissement", ModalId = "modalProjet6" },
                new Project { Title = "Terrains à bâtir – Braga", ImageUrl = "/images/project07.jpg", Description = "Projet prêt à développer", ModalId = "modalProjet7" }
            };

            return View(projetsDispo); // Affiche la vue Disponibles.cshtml
        }
    }
}
