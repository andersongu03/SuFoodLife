﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SuFood.Models
{
    public partial class Announcement
    {
        public int AnnouncementId { get; set; }
        public string AnnouncementContent { get; set; }
        public bool? AnnouncementStatus { get; set; }
        public DateTime AnnouncementStartDate { get; set; }
        public DateTime? AnnouncementEndDate { get; set; }
        public byte[] AnnouncementImage { get; set; }
        public string AnnouncementType { get; set; }
        public string AnnouncementCreater { get; set; }
    }
}