namespace HotelRapidApi.Models
{
    public class PostLocationViewModel
    {
        public string LocationName { get; set; }
        public int? dest_id { get; set; }
        public DateTime arrival_date { get; set; }
        public DateTime departure_date { get; set; }
        public string? search_type { get; set; }

    }
}
