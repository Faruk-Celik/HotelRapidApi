namespace HotelRapidApi.Models
{
    public class HotelViewModel
    {

        
        public Datum [] data { get; set; }
      

        public class Datum
        {
            public string dest_id { get; set; }
            public string search_type { get; set; }
            public int hotels { get; set; }
         
        }

    }
}
