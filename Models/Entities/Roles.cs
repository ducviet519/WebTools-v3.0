using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models
{
    public class Roles
    {

        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; }

        public string Description { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [EnumDataType(typeof(Status))]
        public Status StatusDropList { get; set; }
    }
    public enum Status
    {
        [Display(Name = "Đang sử dụng")] DangSuDung = 1,
        [Display(Name = "Ngưng sử dụng")] NgungSuDung = 0,
    }
}
