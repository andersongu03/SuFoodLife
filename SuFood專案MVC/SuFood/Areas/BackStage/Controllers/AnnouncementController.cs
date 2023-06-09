using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Areas.BackStage.Controllers
{
    public class AnnouncementController : Controller
    {
		private readonly SuFoodDBContext _context;

		public AnnouncementController(SuFoodDBContext context)
		{
			_context = context;
		}
		// GET: BackStage/Announcements
		public async Task<IActionResult> Index()
		{
			return View();

		}
		public IActionResult Announcement()
		{
			return View();
		}

		[HttpGet]
		public async Task<IEnumerable<VMAnnouncement>> GetAnnouncement()
		{
			return _context.Announcement.Select(VMA => new VMAnnouncement
			{
				AnnouncementId = VMA.AnnouncementId,
				AnnouncementContent = VMA.AnnouncementContent,
				AnnouncementStatus = VMA.AnnouncementStatus,
				AnnouncementStartDate = VMA.AnnouncementStartDate,
				AnnouncementImage = VMA.AnnouncementImage,
				AnnouncementType = VMA.AnnouncementType
			});
		}
		[HttpGet]
		public ActionResult<IEnumerable<Announcement>> GetFilteredAnnouncement(string keyword, string type)
		{
			List<Announcement> filteredAnnouncements = _context.Announcement
		   .Where(a => a.AnnouncementType.Equals(type) && (string.IsNullOrEmpty(keyword) || a.AnnouncementContent.Contains(keyword)))
		   .ToList();

			return filteredAnnouncements;
		}
		// GET: BackStage/Announcements/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Announcement == null)
			{
				return NotFound();
			}

			var announcement = await _context.Announcement
				.FirstOrDefaultAsync(m => m.AnnouncementId == id);
			if (announcement == null)
			{
				return NotFound();
			}

			return View(announcement);
		}

		[HttpPost]
		public async Task<string> Create(VMAnnouncement VMA, IFormFile AnnouncementImage)
		{
			if (ModelState.IsValid)
			{
				if (AnnouncementImage != null)
				{
					byte[] data = null;
					using (BinaryReader br = new BinaryReader(AnnouncementImage.OpenReadStream()))
					{
						data = br.ReadBytes((int)AnnouncementImage.Length);
						VMA.AnnouncementImage = data;
					}

				}
				_context.Announcement.Add(new Models.Announcement()
				{
					AnnouncementId = VMA.AnnouncementId,
					AnnouncementContent = VMA.AnnouncementContent,
					AnnouncementImage = VMA.AnnouncementImage,
					AnnouncementStatus = VMA.AnnouncementStatus,
					AnnouncementType = VMA.AnnouncementType,
					AnnouncementStartDate = DateTime.Now,
					//填入建立者
				});
				await _context.SaveChangesAsync();
				return "新增成功";
			}
			return "新增失敗";
		}

		[HttpPost]
		public async Task<string> Edit(VMAnnouncement VMA, IFormFile AnnouncementImage)
		{
			if (AnnouncementImage != null)
			{
				{
					byte[] data = null;
					using (BinaryReader br = new BinaryReader(AnnouncementImage.OpenReadStream()))
					{
						data = br.ReadBytes((int)AnnouncementImage.Length);
						VMA.AnnouncementImage = data;
					}
				}
				var Edititem = await _context.Announcement.FindAsync(VMA.AnnouncementId);
				if (Edititem != null)
				{
					Edititem.AnnouncementContent = VMA.AnnouncementContent;
					Edititem.AnnouncementImage = VMA.AnnouncementImage;
					Edititem.AnnouncementStatus = VMA.AnnouncementStatus;
					Edititem.AnnouncementType = VMA.AnnouncementType;
					Edititem.AnnouncementStartDate = DateTime.Now;
					//填入建立者
				};
				await _context.SaveChangesAsync();
				return "修改成功";
			}
			return "修改失敗";
		}

		[HttpDelete]
		public async Task<string> DeleteAnnouncement(int id)
		{
			if (_context.Announcement == null)
			{
				return "刪除失敗";
			}
			var announcement = await _context.Announcement.FindAsync(id);
			if (announcement != null)
			{
				_context.Announcement.Remove(announcement);
			}

			await _context.SaveChangesAsync();
			return "刪除成功";
		}

		private bool AnnouncementExists(int id)
		{
			return (_context.Announcement?.Any(e => e.AnnouncementId == id)).GetValueOrDefault();
		}
	}
}
