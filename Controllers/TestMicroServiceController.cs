using Microsoft.AspNetCore.Mvc;
using MVCApp1.Models;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace MVCApp1.Controllers
{
    public class TestMicroServiceController : Controller
    {
        public async Task<IActionResult>  Index(Int64? Id,string? Name,string? MobileNo)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
           
            response = await client.GetAsync("http://localhost:23892/api/My");
            
            response.EnsureSuccessStatusCode();
            string ResponseBody=await response.Content.ReadAsStringAsync();
            ViewBag.ResponseBody =JsonConvert.DeserializeObject(ResponseBody);
            PersionalInfo detail = new PersionalInfo();
            if (Id != null &&  Id!=0)
            {                
                detail.Rec_Id = (Int64)Id;
                detail.Name = Name;
                detail.MobileNo = MobileNo;
                
            }
                return View(detail);
        }

        public async Task<IActionResult> PostData()
        {
            HttpClient client = new HttpClient();
            var name1=HttpContext.Request.Form["name"] ;
            var mobileno1= HttpContext.Request.Form["mobileNo"];
            var id= HttpContext.Request.Form["Id"];
           
            if (id=="0")
            {
                var PreInfo = new PersionalInfo()
                {
                    Name = name1,
                    MobileNo = mobileno1
                };
                var PerInfoSer = JsonConvert.SerializeObject(PreInfo);
                var ReqContent = new StringContent(PerInfoSer, Encoding.UTF8, "application/json");
                var Res = await client.PostAsync("http://localhost:23892/api/My", ReqContent);
                Res.EnsureSuccessStatusCode();
                var content = await Res.Content.ReadAsStringAsync();
                var CreatedInfo = JsonConvert.DeserializeObject<PersionalInfo>(content);
            }
            else
            {
                var PreInfo = new PersionalInfo()
                {
                    Rec_Id =Convert.ToInt64(id),
                    Name = name1,
                    MobileNo = mobileno1
                };
                var PerInfoSer = JsonConvert.SerializeObject(PreInfo);
                var ReqContent = new StringContent(PerInfoSer, Encoding.UTF8, "application/json");
                var Res = await client.PutAsync("http://localhost:23892/api/My", ReqContent);
                Res.EnsureSuccessStatusCode();
                var content = await Res.Content.ReadAsStringAsync();
                var CreatedInfo = JsonConvert.DeserializeObject<PersionalInfo>(content);
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult>Edit(Int64 id)
        {
            Int64 Id;
            string Name="",MobileNo = "";
            if (id != 0)
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("http://localhost:23892/api/My/GetId/" + id);
                response.EnsureSuccessStatusCode();
                string ResponseBody = await response.Content.ReadAsStringAsync();
                ViewBag.ResponseById = JsonConvert.DeserializeObject(ResponseBody);
                foreach (var Item in ViewBag.ResponseById)
                {
                    Id=Item.rec_Id;
                    Name=Item.name;
                    MobileNo=Item.mobileNo;
                }
            }
            return RedirectToAction("Index", new {Id= id, Name= Name, MobileNo= MobileNo });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Int64 id)
        {
             if (id != 0)
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:23892/api/My?id=" + id);
                response.EnsureSuccessStatusCode();
                string ResponseBody = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject(ResponseBody);
                
            }
            return RedirectToAction("Index");
        }


    }
}
