namespace RasperryBI.TollAggregatorService.Entities
{
    public class Toll
    {
        public int TollId { get; set; }

        public string VehicleModel { get; set; }

        public string VehicleNo { get; set; }

        public string TollImageUrl { get; set; }

        public string CitizenName { get; set; }

        public string TollLocation { get; set; }

        public bool Processed { get; set; }
    }
}