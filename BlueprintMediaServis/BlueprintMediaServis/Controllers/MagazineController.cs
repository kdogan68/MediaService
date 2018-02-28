using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlueprintMediaServis.Models;
using System.IO;
using System.Web.UI.WebControls;

namespace BlueprintMediaServis.Controllers
{
    public class MagazineController : Controller
    {
        public ActionResult Index()
        {
            BlueprintMediaServisEntities1 BMSentity = new BlueprintMediaServisEntities1();

            return View(Tuple.Create<Magazine, IEnumerable<Magazine>>(new Magazine(), BMSentity.Magazines.ToList()));
        }

        [HttpPost]
        public ActionResult Insert(Magazine magazineEntity, HttpPostedFileBase image, HttpPostedFileBase pdf)
        {
            string imagePath = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(image.FileName));
            string pdfPath = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(pdf.FileName));

            using (BlueprintMediaServisEntities1 entities = new BlueprintMediaServisEntities1())
            {
                magazineEntity.imageFile = ConvertByte(imagePath, image);
                magazineEntity.pdfFile = ConvertByte(pdfPath, pdf);
                magazineEntity.updateTime = DateTime.Now;
                magazineEntity.createTime = DateTime.Now;
        
                entities.Magazines.Add(magazineEntity);
                entities.SaveChanges();
            }

           return  Redirect(Request.UrlReferrer.ToString());
        }

        
        public ActionResult DisplayPDF(int id)
        {
            var cont = new BlueprintMediaServisEntities1();
            var query = cont.Magazines.ToList();
            var result = query.Where(m => m.id == id).ToList();
            //var query = from magazine in BMSentity.Magazines where (magazine.id == id) select magazine.pdfFile;
            byte[] byteArray = result.Select(m => m.pdfFile).First();
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

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


        private Byte[] ConvertByte(string filePath, HttpPostedFileBase f)
        {                    
            string filename = Path.GetFileName(filePath);
            string ext = Path.GetExtension(filename);
            string contenttype = String.Empty;
            
            //Set the contenttype based on File Extension

            switch (ext)
            {                              
                case ".jpg":
                    contenttype = "image/jpg";
                    break;

                case ".png":
                    contenttype = "image/png";
                    break;

                case ".pdf":
                    contenttype = "application/pdf";
                    break;
            }

            if (contenttype != String.Empty)
            {
                Stream fs = f.InputStream;
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                return bytes;
            }

            return null;
        }

      



    }
}