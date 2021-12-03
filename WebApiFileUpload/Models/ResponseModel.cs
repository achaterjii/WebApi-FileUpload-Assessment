using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiFileUpload.Models
{
    public class ResponseModel
    {
        public bool fileValid { get; set; }
        public List<string> invalidLines { get; set; }
    }
}
