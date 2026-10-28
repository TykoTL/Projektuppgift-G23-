
using Microsoft.AspNetCore.Mvc;
using SeventWebApplication.Models;
using SeventWebApplication.Models.Booking;
using SeventWebApplication.Models.VehicleNS;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace SeventWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // TESTDATA: Lägg till dummybilen.
            FleetManager.instance.SeedTestVehicle();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        //[HttpPost]
        public IActionResult Search(string query)
        {
            // GAMMAL KOD – kommenterad eftersom FindVehiclesFromRequest()
            // inte finns i FleetManager.

            /*
            var result = FleetManager.instance.FindVehiclesFromRequest(Request);

            var searchResults = result
                .Select(vehicle => new VehicleSearchResult(vehicle))
                .ToList();

            return Json(searchResults);
            */

            // NY KOD – använder FindVehicles(string query) i FleetManager.
            //var result = FleetManager.instance.FindVehicles(query);
            //
            //var searchResults = result
            //    .Select(vehicle => new VehicleSearchResult(vehicle))
            //    .ToList();
            //
            //return Json(searchResults);


            // NY NY KOD - använder Request objektet istället så kan man få ut specifika sub?querys från Queryn enklare (se FindVehiclesFromRequest)

            //var result = VehicleAssignmentService.AssignVehiclesFromQuery( Request.Query )._vehicles;
            var result = FleetManager.instance.FindVehiclesFromQuery( Request.Query );

            return Json(result);
        }

    }
}
