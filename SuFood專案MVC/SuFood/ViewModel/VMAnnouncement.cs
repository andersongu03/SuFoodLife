namespace SuFood.ViewModel
{
    public class VMAnnouncement
    {
        public int AnnouncementId { get; set; }
        public string AnnouncementContent { get; set; }
        public bool? AnnouncementStatus { get; set; }
        public DateTime AnnouncementStartDate { get; set; }
        public byte[]? AnnouncementImage { get; set; }
        public string? AnnouncementType { get; set; }

		public string? AnnouncementCreater { get; set; }
	}
}
