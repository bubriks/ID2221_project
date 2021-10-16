using DIntensiveFE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DIntensiveFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HttpClient client;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            client = new HttpClient();
        }

        public async Task<IActionResult> IndexAsync()
        {
            string filename = HttpContext.Request.Query["filename"].ToString();
            var retobj = await GetData();
            if (!String.IsNullOrEmpty(filename))
            {
                return View(retobj[filename]);
            }
            return View(retobj);
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


        public async Task<Dictionary<string, blob>> GetData()
        {
            Dictionary<string, blob> keyValuePairs = new Dictionary<string, blob>();
            string resultContentWeath = "";

            HttpResponseMessage weatherresponse = await client.GetAsync("https://dataintproj-default-rtdb.firebaseio.com/database.json");

            if (weatherresponse.IsSuccessStatusCode)
            {
                resultContentWeath = await weatherresponse.Content.ReadAsStringAsync();
            }

            var json = JObject.Parse(resultContentWeath);
            var feat = json?["blobs"] ?? "";


            foreach (dynamic entry in feat)
            {
                string name = entry.Name;
                dynamic value = entry.Value;

                
                keyValuePairs.Add(name, value.ToObject<blob>());
            }



            return keyValuePairs;


        }
    }
}
