using System.ComponentModel.DataAnnotations;

namespace Human_Evolution.Models
{
    public class Bien
    {
        public int Id { get; set; }

        [Required]
        public string Titre { get; set; }

        // Appartement | Maison | Terrain | Commercial
        [Required]
        public string Type { get; set; }

        [Required]
        public string Ville { get; set; }

        public string Quartier { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Prix { get; set; }

        public decimal? Surface { get; set; }      // m²

        public int? NbPieces { get; set; }

        public int? NbSdb { get; set; }

        public string Etage { get; set; }

        public int? AnneeConstruction { get; set; }

        // Disponible | Réservé | Vendu
        public string Statut { get; set; } = "Disponible";

        public string Reference { get; set; }      // ex: HS-001

        // Chemin image principale ex: /images/biens/appart-chiado.jpg
        public string ImagePrincipale { get; set; }

        // Images supplémentaires (carousel)
        public List<string> Images { get; set; } = new();

        // Caractéristiques (piscine, terrasse, jardin, garage, vue_mer)
        public List<string> Caracteristiques { get; set; } = new();

        // Neuf | Rénové | Ancien | Sur plan
        public string Etat { get; set; }

        // true = visible sur le site public
        public bool Visible { get; set; } = true;

        public DateTime DateAjout { get; set; } = DateTime.UtcNow;

        public string Slug { get; set; }           // URL: appart-t2-chiado-lisbonne
    }
}
