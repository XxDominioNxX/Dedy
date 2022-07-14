using Dedy.Models;
using Dedy.ViewModel;
using Dedy.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dedy.Controllers
{
    public class BDOptionsController : Controller
    {
        private readonly ProductService db;
        
        public BDOptionsController(ProductService context)
        {
            db = context;
        }






        [HttpGet]
        public IActionResult Create(string TypeG)
        {
            if(TypeG == "Памятник")
            {
                ViewData["Type"] = "Памятник";
            }

            if (TypeG == "Благоустройство")
            {
                ViewData["Type"] = "Благоустройство";
            }
            return View(new Product()) ; 
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product p, string Type_return ,IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                p.Type = Type_return;
                await db.Create(p);
                var pr = await db.GetProducts();
                
                if (uploadedFile != null)
                {
                    await db.StoreImage(pr.Last().Id, uploadedFile.OpenReadStream(), uploadedFile.FileName);
                }
                return RedirectToAction("CatalogPamyat","ShowProducts");
            }
            return View("Create", Type_return);
        }


        public async Task<ActionResult> AttachImage(string id)
        {
            Product p = await db.GetProduct(id);
            if (p == null)
                return NotFound();
            return View(p);
        }
        [HttpPost]
        public async Task<ActionResult> AttachImage(string id, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                await db.StoreImage(id, uploadedFile.OpenReadStream(), uploadedFile.FileName);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> GetImage(string id)
        {
            var image = await db.GetImage(id);
            if (image == null)
            {
                return NotFound();
            }
            return File(image, "image/png");
        }


     

    }
}
