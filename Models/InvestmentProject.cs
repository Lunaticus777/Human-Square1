namespace Human_Evolution.Models
{
    public class InvestmentProject
    {
        public int Id { get; set; }
        public string TitreProjet { get; set; }
        public string Localisation { get; set; }
        public string ObjectifInvestissement { get; set; } // Enum: Revente, Location, Hébergement
        public string Description { get; set; }
        public decimal BudgetTotal { get; set; }
        public decimal TicketMinimum { get; set; }
        public double RentabiliteCible { get; set; } // %
        public int DelaiPrevisionnelMois { get; set; }
        public string Risques { get; set; }
        public bool VisibilitePublic { get; set; }
        public string FichePDF { get; set; }
        public List<string> Images { get; set; }
        public string Statut { get; set; } // Enum: Ouvert, Réservé, Fermé
    }

}
