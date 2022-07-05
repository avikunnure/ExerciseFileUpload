using ExerciseFileUploadUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedProjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseFileUploadUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HttpClient _httpClient;
        public HomeController(ILogger<HomeController> logger,
            IHttpClientFactory factory)
        {
            _logger = logger;
            _httpClient = factory.CreateClient("MyAPI");
        }

        public async Task<IActionResult> Index()
        {
            List<SharedProjects.MyDocuments> documents=new List<SharedProjects.MyDocuments>();
            var requestmessage = new HttpRequestMessage(method: HttpMethod.Get, "api/Documents");

          
            var result = await _httpClient.SendAsync(requestmessage);

            if (result.IsSuccessStatusCode)
            {
                var RES = await result.Content.ReadAsStringAsync();
                 documents=JsonConvert.DeserializeObject<List<SharedProjects.MyDocuments>>(RES);
                return View(documents);
            }
            else
            {
                var RES = await result.Content.ReadAsStringAsync();
                ModelState.AddModelError("", RES);
            }
            return View(documents);
        }
        [HttpGet]
        public IActionResult Create()
        {
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
        [HttpPost]
        public async Task<IActionResult> Create(UploadDocument uploadDocument)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SharedProjects.MyDocuments document = new SharedProjects.MyDocuments();
                    using var memorystram = new System.IO.MemoryStream();
                    uploadDocument.formFile.CopyTo(memorystram);
                    document.Content = memorystram.ToArray();
                    memorystram.Close();
                    document.Base64stringFile = Convert.ToBase64String(document.Content);
                    document.FileName = uploadDocument.formFile.FileName;

                    var stringcontent = new StringContent(
                        Newtonsoft.Json.JsonConvert.SerializeObject(document)
                        , UnicodeEncoding.UTF8, "application/json");
                    var requestmessage = new HttpRequestMessage(method: HttpMethod.Post, "api/Documents");

                    requestmessage.Content = stringcontent;
                    var result = await _httpClient.SendAsync(requestmessage);
                    
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["MessageForUser"] = $" Your File '{uploadDocument.formFile.FileName}' added Successfully";
                    }
                    else
                    {
                        var RES = await result.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", RES);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Create", uploadDocument);
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateMultiple(MultiUploadDocument uploadDocument)
        {
            try
            {
                if (ModelState.IsValid)
                {
                     MultiMyDocuments myDocuments = new MultiMyDocuments();
                    foreach (var item in uploadDocument.formFile)
                    {
                        SharedProjects.MyDocuments document = new SharedProjects.MyDocuments();
                        using var memorystram = new System.IO.MemoryStream();
                        item.CopyTo(memorystram);
                        document.Content = memorystram.ToArray();
                        memorystram.Close();
                        document.Base64stringFile = Convert.ToBase64String(document.Content);
                        document.FileName = item.FileName;
                        myDocuments.Documents.Add(document);
                    }
                    

                    var stringcontent = new StringContent(
                        Newtonsoft.Json.JsonConvert.SerializeObject(myDocuments)
                        , UnicodeEncoding.UTF8, "application/json");
                    var requestmessage = new HttpRequestMessage(method: HttpMethod.Post, "api/Documents/PostList");

                    requestmessage.Content = stringcontent;
                    var result = await _httpClient.SendAsync(requestmessage);

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["MessageForUser"] = $" Your '{uploadDocument.formFile.Count}' Files added Successfully";
                    }
                    else
                    {
                        var RES = await result.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", RES);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Create", uploadDocument);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
