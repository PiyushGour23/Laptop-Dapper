using Laptop.Interface;
using Laptop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.IO;

namespace Laptop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IWebHostEnvironment environment)
        {
            _webHostEnvironment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        [HttpPut("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file, string productcode)
        {
            APIResponse response = new APIResponse();
            try
            {
                string Filepath = GetFilePath(productcode);
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }
                string imagepath = Filepath + "\\" + productcode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
                using (FileStream stream = System.IO.File.Create(imagepath)) 
                {
                    await file.CopyToAsync(stream);
                    response.ResponseCode = 200;
                    response.Result = "Pass"; 
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(response);
        }

        [HttpPut("MultipleUploadImage")]
        public async Task<IActionResult> MultipleUploadImage(IFormFileCollection filecollection, string productcode)
        {
            APIResponse response = new APIResponse();
            int passcount = 0;
            int errorcount = 0;
            try
            {
                string Filepath = GetFilePath(productcode);
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }

                foreach (var file in filecollection)
                {
                    string imagepath = Filepath + "\\" + file.FileName;
                    if (System.IO.File.Exists(imagepath))
                    {
                        System.IO.File.Delete(imagepath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagepath))
                    {
                        await file.CopyToAsync(stream);
                        passcount++;
                    }
                }
            }
            catch (Exception ex)
            {
                errorcount++;
                response.Error = ex.Message;
            }
            response.ResponseCode = 200;
            response.Result = passcount + " Uploaded files " + errorcount + " Failed to upload ";
            return Ok(response);
        }

        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(string productcode)
        {
            string ImageURL = string.Empty;
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilePath(productcode);
                string imagepath = Filepath + "\\" + productcode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    ImageURL = hosturl + "/Upload/product/" + productcode + "/" + productcode + ".png";
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return Ok(ImageURL);
        }



        [HttpGet("GetMultipleImage")]
        public async Task<IActionResult> GetMultipleImage(string productcode)
        {
            List<string> ImageURL = new List<string>();
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilePath(productcode);

                if(System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string fileName = fileInfo.Name;
                        string imagepath = Filepath + "\\" + fileName;
                        if (System.IO.File.Exists(imagepath))
                        {
                            string _imageURL = hosturl + "/Upload/product/" + productcode + "/" + fileName;
                            ImageURL.Add(_imageURL);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(ImageURL);
        }


        [HttpGet("DownloadImage")]
        public async Task<IActionResult> DownloadImage(string productcode)
        {
            try
            {
                string Filepath = GetFilePath(productcode);
                string imagepath = Filepath + "\\" + productcode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    MemoryStream stream = new MemoryStream();
                    using (FileStream fileStream = new FileStream(imagepath, FileMode.Open))
                    {
                        await fileStream.CopyToAsync(stream);
                    }
                    stream.Position = 0;                   
                    return File(stream, "image/png", productcode + ".png");     // (stream, type jpg/png,image, filename)
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }


        [HttpDelete("Remove")]
        public async Task<IActionResult> RemoveImage(string productcode)
        {
            //string ImageURL = string.Empty;
            //string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilePath(productcode);
                string imagepath = Filepath + "\\" + productcode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                    return Ok("pass");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpDelete("MultipleImageRemove")]
        public async Task<IActionResult> MultipleImageRemove(string productcode)
        {
            List<string> ImageURL = new List<string>();
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilePath(productcode);

                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        fileInfo.Delete();
                    }
                    return Ok("pass");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(ImageURL);
        }

        [NonAction]
        private string GetFilePath(string productcode) 
        {
            return _webHostEnvironment.WebRootPath + "\\Upload\\product\\" + productcode;
        }
    }
}
