using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.AspNetCore.Session;
using NuGet.Protocol;
using System.Drawing;
using WebPlatform.Models;
using WebPlatform.Models.Enum;
using WebPlatform.Services;
using ZXing.QrCode;

namespace WebPlatform.Controllers
{
    public class ModulesController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ICLogger _logger;
        public ModulesController(ICLogger logger, IWebHostEnvironment environment)
        {
            _environment = environment;
            _logger = logger;
            _logger.System("Initialized Modules Controller");
        }
        // GET: ModulesController1
        public ActionResult Index()
        {
            Byte[] byteArray;
            var width = 250; // width of the Qr Code   
            var height = 250; // height of the Qr Code   
            var margin = 0;
            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = margin
                }
            };
            var pixelData = qrCodeWriter.Write("http://192.168.41.121:8008/pair?webPlatform=KAFKA&appType=MOBILE_APP");

            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference   
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB   
            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                using (var ms = new MemoryStream())
                {
                    var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    try
                    {
                        // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image   
                        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                    }
                    finally
                    {
                        bitmap.UnlockBits(bitmapData);
                    }
                    // save to stream as PNG   
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byteArray = ms.ToArray();
                    ViewBag.QrCodeUri = byteArray;


                }
            }
            return View("Index",new List<Connection>());
        }
        // Add Port 

       
        public async Task<IActionResult> AddModule(string port, string appType, List<Connection> conns)
        {
            AppType _appType;
            int Port;
            if (!int.TryParse(port, out Port))
            {
                return BadRequest("Provided Port is Not Supported");
            }
            if (!Enum.TryParse<AppType>(appType.Split("-")[0], true, out _appType))
            {
                return BadRequest("Provided AppType is Not Supported");
            }
            HttpContext.Response.Cookies.Append(_appType.ToString(), Port.ToString());

            conns.Add(new Connection() { AppType = _appType, Port = Port });
            return PartialView("_Modules", conns);
        }


        // GET: ModulesController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ModulesController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ModulesController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ModulesController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ModulesController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ModulesController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ModulesController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
