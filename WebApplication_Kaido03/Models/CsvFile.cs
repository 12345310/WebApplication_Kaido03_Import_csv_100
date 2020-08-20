using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication_Kaido03.Models
{
    public class CsvFile
    {
        [DisplayName("Parent Child の CSV ファイル")]
        [UploadFile("csv")]
        public HttpPostedFileBase UploadFile { get; set; }

    }
}