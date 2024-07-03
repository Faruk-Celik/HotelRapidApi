using HotelRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Drawing.Text;
using System.Net.Http;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;

namespace HotelRapidApi.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IHttpClientFactory _httpclientFactory;

        public DefaultController ( IHttpClientFactory httpclientFactory )
        {
            _httpclientFactory = httpclientFactory;
        }
        public IActionResult Index ()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> GetLocation ( PostLocationViewModel postLocationViewModel )
        {
            var client = _httpclientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/searchDestination?query=" + postLocationViewModel.LocationName),
                Headers =
    {
        { "X-RapidAPI-Key", "039e9bbb90msh4c02765019d343fp1fcbc3jsn36d89a02d0fd" },
        { "X-RapidAPI-Host", "booking-com15.p.rapidapi.com" },
    },
            };

            using (var response = await client.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
                    return StatusCode((int)response.StatusCode, errorMessage);
                }
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var LocationData = JsonConvert.DeserializeObject<HotelViewModel>(body);
                int? destId = Convert.ToInt32(LocationData.data [0].dest_id);
                var postLocationWithId = new PostLocationViewModel
                {
                    LocationName = postLocationViewModel.LocationName,
                    dest_id = destId,
                    arrival_date = postLocationViewModel.arrival_date,
                    departure_date = postLocationViewModel.departure_date,

                };

                return RedirectToAction("HotelList", "Default", postLocationWithId);
            }

        }
        public async Task<IActionResult> HotelList ( PostLocationViewModel Location )
        {
            var arrivalDate = Location.arrival_date.ToString("yyyy-MM-dd");
            var departureDate = Location.departure_date.ToString("yyyy-MM-dd");

            var client = _httpclientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?dest_id=" + Location.dest_id + "&search_type=CITY&arrival_date=" + arrivalDate + "&departure_date=" + departureDate + "&adults=1&children_age=0%2C17&room_qty=1&page_number=1&units=metric&temperature_unit=c&languagecode=en-us&currency_code=EUR"),
                Headers =
        {
        { "x-rapidapi-key", "039e9bbb90msh4c02765019d343fp1fcbc3jsn36d89a02d0fd" },
        { "x-rapidapi-host", "booking-com15.p.rapidapi.com" },
        },
            };
            using (var response = await client.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
                    return StatusCode((int)response.StatusCode, errorMessage);
                }
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<HotelLocationViewModel>(body);
                if (value.data.hotels != null)
                {
                    return View(value.data.hotels.ToList());
                }

                return View();



            }
        }
        public async Task<IActionResult> HotelDetail ( string hotel_id, string arrival_date, string departure_date )
        {
            var hotelId = hotel_id;
            var arrivalDate = arrival_date;
            var departureDate = departure_date;
            var client = _httpclientFactory.CreateClient();
            var request = new HttpRequestMessage

            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/getHotelDetails?hotel_id=" + hotelId + "&arrival_date=" + arrivalDate + "&departure_date=" + departureDate + "&adults=1&children_age=1%2C17&room_qty=1&units=metric&temperature_unit=c&languagecode=en-us&currency_code=EUR"),
                Headers =
    {
        { "x-rapidapi-key", "039e9bbb90msh4c02765019d343fp1fcbc3jsn36d89a02d0fd" },
        { "x-rapidapi-host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
                    return StatusCode((int)response.StatusCode, errorMessage);
                }
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<HotelDetailViewModel>(body);
                if (value.data != null)
                {
                    return View(value.data);
                }
            }
            return View();

        }

    }
}
