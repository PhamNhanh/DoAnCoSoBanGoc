﻿using System.ComponentModel.DataAnnotations;

namespace WEBTimViec.Models
{
    public class UngTuyen
    {
        [Key]
        public int UngTuyen_id { get; set; }
        public string? url_CV { get; set; }
        public string? ThuGioiThieu { get; set; }
        public DateTime? ThoiGianUngTuyen { get; set; }

/*        public int? applicationUserid { get; set; }*/
        public ApplicationUser? applicationUser { get; set; }
        public int? application_Userid {  get; set; }
        public BaiTuyenDung? BaiTuyenDung { get; set; }
        public int? BaiTuyenDungid { get; set; }
    }
}
