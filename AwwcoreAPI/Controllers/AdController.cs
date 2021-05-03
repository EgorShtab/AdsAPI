using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using AwwcoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace AwwcoreAPI.Controllers
{
    public class AdController : Controller
    {
        protected ApplicationContext db;
        public AdController(ApplicationContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Get(int page=0, string sortBy=null)
        {
            IEnumerable<Ad> AdList = db.Ads.Include(pl => pl.PhotoLinks);
            if (AdList.Count() == 0) return NoContent();
            if (sortBy != null)
            {
                switch (sortBy){
                    case "dateDescending": AdList = AdList.OrderByDescending(keySelector: ad => ad.Date); break;
                    case "priceDescending": AdList = AdList.OrderByDescending(keySelector: ad => ad.Price); break;
                    case "date": AdList = AdList.OrderBy(keySelector: ad => ad.Date); break;
                    case "price": AdList = AdList.OrderBy(keySelector: ad => ad.Price); break;
                    default: break;
                }
            }
            return Ok(AdList.Select(ad => new
            {
                ad.Name,
                PhotoLink = ad.PhotoLinks.FirstOrDefault().Link,
                ad.Price
            }).ToList().Skip(page*10).Take(10));
        }
        [HttpGet]
        public IActionResult Details(int id, bool fields=false)
        {
            Ad ConcreteAd = db.Ads.Include(pl => pl.PhotoLinks).Where(ad=>ad.Id==id).SingleOrDefault();
            if (ConcreteAd == null) return NoContent();
            object AdToReturn=fields? new { ConcreteAd.Name, ConcreteAd.Price, PhotoLinks = ConcreteAd.PhotoLinks.Select(pl=>new{
                pl.Link
            }), ConcreteAd.Description }:
                new { ConcreteAd.Name, ConcreteAd.Price, PhotoLinks = ConcreteAd.PhotoLinks.FirstOrDefault().Link };
            return Ok(AdToReturn);
        }
        [HttpPost]
        public IActionResult Post([FromBody]JObject data)
        {
            List<string> PhotoLinks = new List<string>();
            Ad ad = new Ad();
            try
           {
                JToken PhotoLinksJ = data["PhotoLinks"];
                var JArray = PhotoLinksJ.Children();

                ad.Name = (string)data["Name"].ToString();
                ad.Description = data["Description"].ToString();
                ad.Price = (decimal)data["Price"];
                ad.Date = DateTime.Now;

                foreach(var photoLinkJ in JArray)
                {
                    PhotoLinks.Add((string)photoLinkJ);
                }
            }
            catch{
                throw new Exception("Error while JSON parsing occurred");
            }
            if (ModelState.IsValid&&PhotoLinks.Count<=3&&PhotoLinks.Count>0)
            {
                db.Ads.Add(ad);
                db.SaveChanges();
                int adId = ad.Id;

                foreach(var photoLink in PhotoLinks)
                {
                    PhotoLink link = new PhotoLink();
                    link.Link = photoLink;
                    link.AdId = adId;
                    db.PhotoLinks.Add(link);
                    db.SaveChanges();
                }

                return Ok(ad.Id);
            }
            else return BadRequest();
        }
    }
}
