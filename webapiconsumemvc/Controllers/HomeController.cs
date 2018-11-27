using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using webapiconsumemvc.Models;

namespace webapiconsumemvc.Controllers
{
    public class HomeController : Controller
    {
        //index actionmethod
        public ActionResult Index()
        {
            return View();
        }
        //about actionmethod
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //Getmembers actionmethod2
        public ActionResult Getmembers()
        {
            IEnumerable<Productviewmodel> products = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:13312/api/");
                var responseTask = client.GetAsync("products");
                responseTask.Wait();
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Productviewmodel>>();
                    readTask.Wait();

                    products = readTask.Result;
                }
                else
                {
                    //Error response received   
                    products = Enumerable.Empty<Productviewmodel>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(products);

        }
        public ActionResult create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult create(Productviewmodel product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:13312/api/products");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<Productviewmodel>("products", product);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(product);
        }


        public ActionResult Edit(int id)
        {
            Productviewmodel product = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:13312/api/");
                //HTTP GET
                var responseTask = client.GetAsync("products?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Productviewmodel>();
                    readTask.Wait();

                    product = readTask.Result;
                }
            }
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Productviewmodel product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:13312/api/");

             

                //HTTP POST
                var putTask = client.PutAsJsonAsync<Productviewmodel>("products?id=" + product.productno.ToString(),product);
                putTask.Wait();

               // var result = putTask.Result;
                if(putTask.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
              
                //if (result.IsSuccessStatusCode)
                //{

                //    return RedirectToAction("Index");
                //}
            }
            return View(product);
        }
    }
}

