using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;
using SuFood.ViewModel;

namespace SuFood.Areas.BackStage.Controllers
{
    [Area("BackStage")]
    public class FreeChoicePlansManagementController : Controller
    {
        private readonly SuFoodDBContext _context;

        public FreeChoicePlansManagementController(SuFoodDBContext context)
        {
            _context = context;
        }

        //GET: /BackStage/FreeChoicePlansManagement/GetPlansAndProducts 取得方案與方案內的商品 !!有三張表的寫法
        public Object GetPlansAndProducts()
        {
            return _context.ProductsOfPlans.GroupBy(p => p.PlanId).Select(group => new
            {
                plans = group.Select(p => new VmFreeChoicePlans
                {
                    PlanId = p.Plan.PlanId,
                    PlanName = p.Plan.PlanName,
                    PlanDescription = p.Plan.PlanDescription,
                    PlanPrice = p.Plan.PlanPrice,
                    PlanTotalCount = p.Plan.PlanTotalCount,
                    PlanCanChoiceCount = p.Plan.PlanCanChoiceCount,
                    PlanStatus = p.Plan.PlanStatus,
                }).SingleOrDefault(),
                Products = group.Select(p => new
                {
                    ProductId = p.Product.ProductId,
                    ProductName = p.Product.ProductName,
                    Price = p.Product.Price,
                })
            });
        }

        //GET: /BackStage/FreeChoicePlansManagement/GetAllProducts 取得所有商品
        [HttpGet]
        public async Task<IEnumerable<object>> GetAllProducts()
        {
            return _context.Products.Select(pro => new
            {
                ProductId = pro.ProductId,
                ProductName = pro.ProductName,
                Price = pro.Price,
            });
        }

        

        //DELETE: /FreeChoicePlansManagement/DeleteProductsOfPlansAndPlans/{PlanId} 刪除方案中的產品(單筆) ProductsOfPlans和FreeChoicePlans
        [HttpDelete]
        public async Task<string> DeleteProductsOfPlansAndPlans(int PlanId)
        {
            var childTable = _context.ProductsOfPlans.Where(pop => pop.PlanId == PlanId).Select(pop =>　pop);
            _context.ProductsOfPlans.RemoveRange(childTable);
            await _context.SaveChangesAsync();

            var FatherTable = _context.FreeChoicePlans.Where(fcp => fcp.PlanId == PlanId).Select(fcp => fcp).SingleOrDefault();
            
            if(FatherTable != null)
            {
                _context.FreeChoicePlans.Remove(FatherTable);
                _context.SaveChanges();
                return "刪除成功";
            }
            return "刪除失敗";
        }

        //CREATE: /BackStage/FreeChoicePlansManagement/CreatePlans 新增方案與方案中的產品。
        [HttpPost]
        public async Task<string> CreatePlans([FromBody] VmFreeChoicePlans vmParameters)
        {
            FreeChoicePlans fcp = new FreeChoicePlans
            {
                PlanId= vmParameters.PlanId,
                PlanName= vmParameters.PlanName,
                PlanDescription= vmParameters.PlanDescription,
                PlanPrice= vmParameters.PlanPrice,
                PlanCanChoiceCount= vmParameters.PlanCanChoiceCount,
                PlanTotalCount= vmParameters.PlanTotalCount,
                PlanStatus= vmParameters.PlanStatus,
                ProductsOfPlans= vmParameters.ProductsOfPlans,
            };

            _context.FreeChoicePlans.Add(fcp);
            await _context.SaveChangesAsync();
            return "新增方案成功";
        }

        //Edit: /BackStage/FreeChoicePlansManagement/EditPlans 新增方案與方案中的產品。
        [HttpPost]
        public async Task<string> EditPlans([FromBody] VmFreeChoicePlans vmParameters)
        {
            if(vmParameters == null)
            {
                return "修改失敗";
            }

            //修改主資料表內容
            FreeChoicePlans fcp = new FreeChoicePlans
            {
                PlanId = vmParameters.PlanId,
                PlanName = vmParameters.PlanName,
                PlanDescription = vmParameters.PlanDescription,
                PlanPrice = vmParameters.PlanPrice,
                PlanCanChoiceCount = vmParameters.PlanCanChoiceCount,
                PlanTotalCount = vmParameters.PlanTotalCount,
                PlanStatus = vmParameters.PlanStatus,
                //ProductsOfPlans = vmParameters.ProductsOfPlans,
            };
            _context.FreeChoicePlans.Update(fcp);

            //這裡使用條件判斷，修改ProductsOfPlans
            // (1)添加清單中的資料，資料庫沒有該筆資料時，資料存入資料庫。
            var pop = vmParameters.ProductsOfPlans;
            foreach (var product in pop)
            {
                // 檢查是否已存在相同的記錄，不存在的內容儲存到資料庫
                if (! _context.ProductsOfPlans.Any(pop => pop.PlanId == vmParameters.PlanId && pop.ProductId == product.ProductId))
                {
                    ProductsOfPlans data = new ProductsOfPlans
                    {
                        PlanId = vmParameters.PlanId,
                        ProductId = product.ProductId,
                    };
                    _context.ProductsOfPlans.Add(data);
                }
            }
            //(2)添加清單中的資料，資料庫有清單中沒有的資料時，刪除資料庫的該資料
            var tableOfPOP = _context.ProductsOfPlans.Where(pop => pop.PlanId == vmParameters.PlanId);
            foreach (var product in tableOfPOP)
            {
                //product.ProductId
                if (!vmParameters.ProductsOfPlans.Any(pop => pop.ProductId == product.ProductId))
                {
                    var removeItem = tableOfPOP.Where(p => p.ProductId == product.ProductId);
                    _context.ProductsOfPlans.RemoveRange(removeItem);
                }
            }

            await _context.SaveChangesAsync();
            return "修改方案成功";
        }

        //新增ProdcutOfPlans資料表
        [HttpPost]
        public async Task<string> CreateProductsOfPlan([FromBody] List<VmProductsOfPlans> vmParameters)
        {
            foreach (var vmParameter in vmParameters)
            {
                if (!_context.FreeChoicePlans.Any(p => p.PlanId == vmParameter.PlanId))
                {
                    return "不存在該方案";
                }

                // 檢查是否已存在相同的記錄
                if (_context.ProductsOfPlans.Any(pop => pop.PlanId == vmParameter.PlanId && pop.ProductId == vmParameter.ProductId))
                {
                    return $"重複的主鍵：PlanId = {vmParameter.PlanId}, ProductId = {vmParameter.ProductId}";
                }

                ProductsOfPlans pop = new ProductsOfPlans
                {
                    ProductId = vmParameter.ProductId,
                    PlanId = vmParameter.PlanId,
                };

                _context.ProductsOfPlans.Add(pop);
            }

            await _context.SaveChangesAsync();

            return "新增成功";
        }

        //DELETE: /FreeChoicePlansManagement/DeleteProductsOfPlans 刪除方案中的產品(單筆) ProductsOfPlans
        [HttpDelete]
        public async Task<string> DeleteProductsOfPlans(int PlanId, int ProductId)
        {
            if (_context.ProductsOfPlans == null)
            {
                return "資料不存在";
            }

            var product = await _context.ProductsOfPlans.FindAsync(PlanId, ProductId);
            if (product != null)
            {
                _context.ProductsOfPlans.Remove(product);
            }
            await _context.SaveChangesAsync();
            return "刪除成功";
        }

        private bool ProductsOfPlansExists(int id)
        {
            return (_context.ProductsOfPlans?.Any(e => e.PlanId == id)).GetValueOrDefault();
        }
    }
}
