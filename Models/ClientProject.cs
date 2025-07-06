namespace Human_Evolution.Models
{
    public class ClientProject
    {
        public int Id { get; set; }
        public string TitreProjet { get; set; }
        public string ClientNom { get; set; }
        public string Localisation { get; set; }
        public string Description { get; set; }
        public string TypeProjet { get; set; } // Enum: Construction, Réhabilitation...
        public string EtatAvancement { get; set; } // Enum: Idée, Étude, Permis, Chantier, Livré
        public decimal BudgetPrevisionnel { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime? DateLivraisonPrevue { get; set; }
        public List<string> Photos { get; set; }
        public List<string> Documents { get; set; }
        public string NoteInterne { get; set; }
        public bool StatutPublic { get; set; }
    }

}
