using Dedy.Models;
using Dedy.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dedy.Controllers
{
    public class ShowProductsController : Controller
    {
        private readonly ProductService db;
        private int PageSize = 8;
        public ShowProductsController(ProductService context)
        {
            db = context;
        }

        [HttpGet]
        public ViewResult CatalogPamyat(int Page = 1, Filter filt = null)
        {
            var builder = Builders<Product>.Filter;
            var Filter = builder.Eq("Type", "Памятник");
            if(filt != null)
            {


                if(filt.Color_Black || filt.Color_Red || filt.Color_White)
                {
                    if (filt.Color_Black)
                    {
                        if (filt.Color_Red)
                        {
                            if (filt.Color_White)
                            {
                                Filter &= builder.Eq("Color", "Черный") | builder.Eq("Color", "Красный") | builder.Eq("Color", "Белый");
                            }
                            else
                            {
                                Filter &= builder.Eq("Color", "Черный") | builder.Eq("Color", "Красный");
                            }

                        }
                        else
                        {
                            if (filt.Color_White)
                            {
                                Filter &= builder.Eq("Color", "Черный") | builder.Eq("Color", "Белый");
                            }
                            else
                            {
                                Filter &= builder.Eq("Color", "Черный");
                            }
                        }
                    }
                    else
                    {
                        if (filt.Color_Red)
                        {
                            if (filt.Color_White)
                            {
                                Filter &= builder.Eq("Color", "Красный") | builder.Eq("Color", "Белый");
                            }
                            else
                            {
                                Filter &= builder.Eq("Color", "Красный");
                            }

                        }
                        else
                        {
                            if (filt.Color_White)
                            {
                                Filter &= builder.Eq("Color", "Белый");
                            }
                        }
                    }
                }


                





                if (filt.Material != null)
                {
                    Filter &= builder.Eq("Model", filt.Material);
                }
                if (filt.Name != null)
                {
                    Filter &= builder.Regex("Name", filt.Name);
                }
                if (filt.Orientation != null)
                {
                    Filter &= builder.Eq("Orientation", filt.Orientation);
                }
                if (filt.MinPrice != null && filt.MaxPrice == null)
                {
                    Filter &= builder.Gte("Price", filt.MinPrice);
                }
                if (filt.MaxPrice != null && filt.MinPrice == null)
                {
                    Filter &= builder.Lte("Price", filt.MaxPrice);
                }
                if (filt.MaxPrice != null && filt.MinPrice != null)
                {
                    Filter &= builder.Gt("Price", filt.MinPrice) & builder.Lt("Price", filt.MaxPrice);

                }
            }

            ViewData["Action"] = "CatalogPamyat";
            int elements = Convert.ToInt32(db.GetCount(Filter)) - (Page - 1) * PageSize;
            var pageList = elements >= PageSize ? db.GetProducts(Page, PageSize, Filter).ToList() : db.GetProducts(Page, elements, Filter).ToList();
            return View("Catalog", new ListProductView(pageList,
                new PagingInfo
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItems = Convert.ToInt32(db.GetCount(Filter))
                }, filt));
        }

        [HttpPost]
        public async Task<IActionResult> CatalogPamyat(Filter filt, string returnId, string icq)
        {
            if(returnId == null)
            {
                return RedirectToAction("CatalogPamyat", filt);
            }
            else
            {
                if(icq == "77")
                {
                    await db.Remove(returnId);
                    return RedirectToAction("CatalogPamyat", filt);
                }
                else
                {
                    var item = db.GetProduct(returnId);
                    return View("ShowAdditionalInformation", item.Result);
                }
               
            }
            
        }




        [HttpGet]
            public ViewResult CatalogOgrad(int Page = 1, Filter filt = null)
        {
            var builder = Builders<Product>.Filter;
            var Filter = builder.Eq("Type", "Благоустройство");
            if (filt != null)
            {
                if (filt.Name != null)
                {
                    Filter &= builder.Regex("Name", filt.Name);
                }
                if (filt.MinPrice != null && filt.MaxPrice == null)
                {
                    Filter &= builder.Gte("Price", filt.MinPrice);
                }
                if (filt.MaxPrice != null && filt.MinPrice == null)
                {
                    Filter &= builder.Lte("Price", filt.MaxPrice);
                }
                if (filt.MaxPrice != null && filt.MinPrice != null)
                {
                    Filter &= builder.Gt("Price", filt.MinPrice) & builder.Lt("Price", filt.MaxPrice);

                }
            }

            ViewData["Action"] = "CatalogOgrad";
            int elements = Convert.ToInt32(db.GetCount(Filter)) - (Page - 1) * PageSize;
            var pageList = elements >= PageSize ? db.GetProducts(Page, PageSize, Filter).ToList() : db.GetProducts(Page, elements, Filter).ToList();
            return View("Catalog", new ListProductView(pageList,
                new PagingInfo
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItems = Convert.ToInt32(db.GetCount(Filter))
                },filt));
        }

        [HttpPost]
        public IActionResult CatalogOgrad(Filter filt = null)
        {
            return RedirectToAction("CatalogOgrad", filt);
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
