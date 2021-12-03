using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApiFileUpload.Models;

namespace WebApiFileUpload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        public IActionResult Get()
        {            
            return Ok("File Upload API running..");
        }

        [HttpPost]
        [Route("upload")]
        public IActionResult Upload(IFormFile file)
        {
            ResponseModel response = new ResponseModel();
            response.fileValid = true;
            response.invalidLines = new List<string>();

            try
            {
                using (StreamReader txtFile = new StreamReader(file.OpenReadStream()))
                {
                    int counter = 0;
                    string line = "";
                    while ((line = txtFile.ReadLine()) != null)
                    {
                        string invalidFName = "";
                        string invalidAccNo = "";
                        var firstName = line.Split(' ')[0];
                        if (!Regex.IsMatch(firstName, @"^[A-Z]+[a-zA-Z]*$"))
                        {
                            response.fileValid = false;
                            invalidFName = firstName;
                        }
                        var accNo = line.Split(' ')[1];
                        if (!Regex.IsMatch(accNo, @"^[3|4]\d{6}$") && !Regex.IsMatch(accNo, @"^[3|4]\d{6}[p]$"))
                        {
                            invalidAccNo = accNo;
                            response.fileValid = false;
                        }
                        int lineNo = counter + 1;
                        if (invalidFName != "" && invalidAccNo == "")
                        {
                            response.invalidLines.Add("Account name-not valid for line " + lineNo + " '" + firstName + " " + accNo + "'");
                        }
                        else if (invalidFName == "" && invalidAccNo != "")
                        {
                            response.invalidLines.Add("Account number-not valid for line " + lineNo + " '" + firstName + " " + accNo + "'");
                        }
                        else if (invalidFName != "" && invalidAccNo != "")
                        {
                            response.invalidLines.Add("Account name,account number-not valid for line " + lineNo + " '" + firstName + " " + accNo + "'");
                        }
                        counter++;
                    }
                    txtFile.Close();
                }
                return Ok(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                if (file.ContentType != "text/plain")
                {
                    return BadRequest("Please Upload A Text File");
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }       
    }
}
