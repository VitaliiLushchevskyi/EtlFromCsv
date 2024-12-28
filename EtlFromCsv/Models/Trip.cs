namespace EtlFromCsv.Models
{
    public class Trip
    {
        [CsvHelper.Configuration.Attributes.Name("tpep_pickup_datetime")]
        public DateTime TpepPickupDatetime { get; set; }
        [CsvHelper.Configuration.Attributes.Name("tpep_dropoff_datetime")]
        public DateTime TpepDropoffDatetime { get; set; }
        [CsvHelper.Configuration.Attributes.Name("passenger_count")]
        [CsvHelper.Configuration.Attributes.Default(0)]
        public int PassengerCount { get; set; }
        [CsvHelper.Configuration.Attributes.Name("trip_distance")]
        public float TripDistance { get; set; }
        [CsvHelper.Configuration.Attributes.Name("store_and_fwd_flag")]
        public string StoreAndFwdFlag { get; set; }
        [CsvHelper.Configuration.Attributes.Name("PULocationID")]
        public int PULocationID { get; set; }
        [CsvHelper.Configuration.Attributes.Name("DOLocationID")]
        public int DOLocationID { get; set; }
        [CsvHelper.Configuration.Attributes.Name("fare_amount")]
        public decimal FareAmount { get; set; }
        [CsvHelper.Configuration.Attributes.Name("tip_amount")]
        public decimal TipAmount { get; set; }
    }
}
