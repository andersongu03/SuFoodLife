using Microsoft.AspNetCore.Mvc;
using SuFood.Models;

namespace SuFood.Areas.BackStage.Controllers
{
    [Area("BackStage")]
    public class AnalysisManagementController : Controller
    {
        private readonly SuFoodDBContext _context;

        public AnalysisManagementController(SuFoodDBContext context)
        {
            _context = context;
        }
    }
}
