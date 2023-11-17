using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Export;
using Stimulsoft.Report;
using Stimulsoft.Base;
using System.Drawing;

namespace StimulSoftReport.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            var stream = new MemoryStream();
            var setting = new StiPdfExportSettings
            {
                EmbeddedFonts = true,
                UseUnicode = true,
                StandardPdfFonts = true,

            };
            StiReport report = new StiReport();
            report.Load(Directory.GetCurrentDirectory() + @"\ReportFile\Report.mrt");
            byte[] file;
            //Stream st = new FileStream("C:\\Users\\USER\\Desktop\\1.jpeg", FileMode.Open);
            System.Drawing.Image imag = System.Drawing.Image.FromFile(Directory.GetCurrentDirectory() + @"\Images\chiken.png");
            StiImage im = new StiImage();
            im.Image = imag;
            //report["Image"] = im.Image;
            report.Dictionary.Variables.Add("Image", im.Image);

            var Test1 = new
            {
                Number = 1,
                Name = "محمد حسین ضرابی",
                Image = imageToByteArray(System.Drawing.Image.FromFile(Directory.GetCurrentDirectory() + @"\Images\mhz.jpg"))
            };
            List<Test2> test2 = new List<Test2>();

            test2.Add(new Test2 { Name = "محمد", Family = "رضایی" });
            test2.Add(new Test2 { Name = "میثم", Family = "فرهانی" });


            //report.Dictionary.Variables.Add("Image", imag);
            report.RegData("Test1", Test1);
            report.RegData("Test2", test2);
            report.Render();           
            report.ExportDocument(StiExportFormat.Pdf, stream, setting);
            byte[] bytesInStream = stream.ToArray();
            stream.Close();
            Response.Clear();
            using (var outputFileStream = new FileStream("C:\\Users\\USER\\Desktop\\Report.pdf", FileMode.Create))
            {
                outputFileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
            return Ok();
        }
        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, imageIn.RawFormat);
            return ms.ToArray();
        }
    }
    public class Test2
    {
        public string Name { get; set; }
        public string Family { get; set; }
    }
}
