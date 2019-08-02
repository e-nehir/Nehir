﻿using goldStore.Areas.Panel.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using goldStore.Models.ViewModel;
using goldStore.Areas.Panel.Models;

namespace goldStore.Controllers
{
    public class ShopController : Controller
    {
        ProductRepository repoProduct = new ProductRepository(new Areas.Panel.Models.goldstoreEntities());
        CategoryRepository repoCategory = new CategoryRepository(new Areas.Panel.Models.goldstoreEntities());
        BrandRepository repoBrand= new BrandRepository(new Areas.Panel.Models.goldstoreEntities());
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Products");
        }
        public PartialViewResult PartialBrands()
        {
            // Tüm markaları partial View'e Gönder
            return PartialView(repoBrand.GetAll());
        }
        public PartialViewResult PartialNewArrivals()
        {
            return PartialView();
        }

        public PartialViewResult PartialPrice()
        {
            return PartialView();
        }

        public PartialViewResult PartialCategory()
        {
            return PartialView(repoCategory.GetAll());
        }

        // ürünleri gösteren method
        public ActionResult Products(int?brandId,int?categoryId,decimal?min,decimal?max, int? page, int? PageSize, int? orderBy)
        {
            ViewBag.orderBy = new List<SelectListItem>() {
                new SelectListItem { Text = "Fiyat", Value = "1", Selected = true },
                new SelectListItem { Text = "İsim", Value = "2" },

            };
            ViewBag.PageSize = new List<SelectListItem>() {
                new SelectListItem { Text = "12", Value = "12", Selected = true },
                new SelectListItem { Text = "9", Value = "9" },
                new SelectListItem { Text = "6", Value = "6" },
                new SelectListItem { Text = "3", Value = "3" },
                new SelectListItem { Text = "2", Value = "2" }
            };

            int _page = page ?? 1;
            int _pageSize = PageSize ?? 6;
            TempData["PageSize"] = _pageSize;
            TempData.Keep("PageSize");

            var result = repoProduct.GetAll();
            // eğer markaya göre bir filtreleme istendiğinde
            if ( brandId!=null)
            {
                result = result.Where(x => x.brandId == brandId).ToList();
            }
            // eğer kategoriye göre bir filtreleme istendiğinde
            else if (categoryId!=null)
            {
                result = result.Where(x => x.categoryId == categoryId).ToList();
            }

            else if (max != null && min != null)
            {
                result = result.Where(x => x.price >= min && x.price <= max).ToList();
               /* TempData["min"] = min;
                TempData.Keep("min");
                TempData["max"] = max;
                TempData.Keep("max");*/
            }
            // order by Fiyat seçilmişse
            else if (orderBy ==1)
            {
                result = result.OrderBy(x => x.price).ToList();
                TempData["orderby"] = 1;
                TempData.Keep("orderby");
            }
            // order by isim seçilmişse
            else if (orderBy ==2)
            {
                result = result.OrderBy(x => x.productName).ToList();
                TempData["orderby"] = 2;
                TempData.Keep("orderby");
            }

            return View(result.ToPagedList(_page,_pageSize));
        }
        
        // ürün detayı
        public ActionResult ProductDetail(int productId)
        {
            return View(repoProduct.Get(productId));
        }
        // product resimleri için
        public ActionResult Thumbnail(int width, int height, int Id, int _imageId)
        {
            byte[] photo = repoProduct.Get(Id).productImage.FirstOrDefault(x => x.imageId == _imageId).image;
            var base64 = Convert.ToBase64String(photo);
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);

            using (var newImage = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(newImage))
            using (var stream = new MemoryStream())
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(image, new Rectangle(0, 0, width, height));
                newImage.Save(stream, ImageFormat.Png);
                return File(stream.ToArray(), "image/png");
            }

        }

        //brand resmi için
        public ActionResult BrandThumbnail(int width, int height, int Id)
        {
            byte[] photo = repoBrand.Get(Id).image;
            var base64 = Convert.ToBase64String(photo);
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);

            using (var newImage = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(newImage))
            using (var stream = new MemoryStream())
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(image, new Rectangle(0, 0, width, height));
                newImage.Save(stream, ImageFormat.Png);
                return File(stream.ToArray(), "image/png");
            }

        }

        //deneme
        [NonAction]
        private int isExistInCard (int id)
        {
            List<BasketItem> card = (List<BasketItem>)Session["card"];
            for (int i = 0; i < card.Count; i++)

                if (card[i].product.productId.Equals(id))
                    return i;
            return -1;
        }
        [HttpPost]
        public List<BasketItem>  AddCard(int productId, int quantity)
        {
            product _product = repoProduct.Get(productId);
            {
                if (Session["card"] == null)
                {
                    List<BasketItem> Card = new List<BasketItem>();
                    Card.Add(new BasketItem()
                    {
                        Id = Guid.NewGuid(),
                        product = _product,
                        quantity = quantity,
                        DateCreated = DateTime.Now

                    });
                    Session["card"] = Card;

                    return (List<BasketItem>)Session["card"];

                }

                else
                {
                    List<BasketItem> card = (List<BasketItem>)Session["card"];
                    int index = isExistInCard(productId);
                    if (index != -1)
                    {
                        card[index].quantity += quantity;
                    }
                    else
                    {
                        card.Add(new BasketItem

                        {
                            product = _product,
                            quantity = quantity,
                            DateCreated = DateTime.Now
                        });
                    }

                    Session["card"] = card;
                   
                }
                return (List<BasketItem>)Session["card"];

            }
        }

            public List<BasketItem> Remove (int productId)
            {
               
                List<BasketItem> card = (List<BasketItem>)Session["card"];
                if(card.Exists(x=>x.product.productId==productId))
                {

                    int index = isExistInCard(productId);
                    card.RemoveAt(index);
                    Session["card"] = card;
                }
            return (List<BasketItem>)Session["card"];


        }

        public ActionResult Basket()
        {
            return View((List<BasketItem>)Session["card"]);

        }



        }

    }
