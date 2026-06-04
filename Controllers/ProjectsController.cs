using Human_Evolution.Models;
using Microsoft.AspNetCore.Mvc;

namespace Human_Evolution.Controllers
{
    public class ProjectsController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ActiveTab = "clients";

            var projetsClients = GetClientProjects();

            return View("Index", projetsClients);
        }

        public IActionResult Disponibles()
        {
            ViewBag.ActiveTab = "disponibles";

            var projetsDisponibles = GetAvailableProjects();

            return View("Index", projetsDisponibles);
        }

        // =========================
        // PROJETS CLIENTS
        // =========================

        private List<Project> GetClientProjects()
        {
            return new List<Project>
            {
                new Project
                {
                    Title = "7 Villas à Esposende",
                    Description = "Projet en bord de mer",
                    ImageUrl = "/images/project01b.png",
                    ModalId = "modalProjet1"
                },

                new Project
                {
                    Title = "10 Villas à Albufeira",
                    Description = "Projet dans le sud",
                    ImageUrl = "/images/project02i.jpg",
                    ModalId = "modalProjet2"
                },

                new Project
                {
                    Title = "Villa à Barcelos",
                    Description = "Projet individuel",
                    ImageUrl = "/images/barcelos.jpeg",
                    ModalId = "modalProjet3"
                },

                new Project
                {
                    Title = "6 Villas à Viseu",
                    Description = "Projet de montagne",
                    ImageUrl = "/images/project04.jpg",
                    ModalId = "modalProjet5"
                },

                new Project
                {
                    Title = "Projet immobilier touristique – Douro",
                    Description = "Projet de réhabilitation",
                    ImageUrl = "/images/project06.jpg",
                    ModalId = "modalProjet6"
                },

                new Project
                {
                    Title = "Projet touristique Lomba – Gondomar",
                    Description = "Projet touristique avec PIP approuvé",
                    ImageUrl = "/images/lomba-01.png",
                    ModalId = "modalLomba"
                },

                new Project
                {
                    Title = "15 appartements à Joane",
                    Description = "Projet résidentiel moderne",
                    ImageUrl = "/images/joane.jpeg",
                    ModalId = "modalProjet7"
                },

                new Project
                {
                    Title = "15 villas à Porto",
                    Description = "Projet haut standing à Gaia",
                    ImageUrl = "/images/gaia.jpg",
                    ModalId = "modalProjet8"
                }
            };
        }

        // =========================
        // PROJETS DISPONIBLES
        // =========================

        private List<Project> GetAvailableProjects()
        {
            return new List<Project>
            {
                new Project
                {
                    Title = "Ruine à réhabiliter – Douro",
                    Description = "Opportunité d’investissement",
                    ImageUrl = "/images/project06.jpg",
                    ModalId = "modalProjet6"
                },

                new Project
                {
                    Title = "Projet touristique Lomba – Gondomar",
                    Description = "Projet avec PIP approuvé",
                    ImageUrl = "/images/lomba-01.png",
                    ModalId = "modalLomba"
                }

            };
        }
    }
}