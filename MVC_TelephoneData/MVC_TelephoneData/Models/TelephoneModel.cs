using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace MVC_TelephoneData.Models
{
    public class TelephoneModel
    {
        [Key]
        public int idworker { get; set; }
        public string firstname { get; set; }
        public string secondname { get; set; }
        public string patronymic { get; set; }
        public string orgname { get; set; }
        public string telephone { get; set; }
        public string longtelephone { get; set; }
        public string postname { get; set; }
        public string code { get; set; }
        
    }
}