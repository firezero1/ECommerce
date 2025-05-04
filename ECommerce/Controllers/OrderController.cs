using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly ProductService _productService;

        public OrderController(OrderService orderService, ProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var products = await _productService.GetAllProductsAsync();
            var viewModel = new OrderViewModel
            {
                OrderDetails = new List<OrderDetailViewModel>
                {
                    new OrderDetailViewModel() // 預設一筆空的訂單明細
                }
            };

            ViewBag.Products = products.Select(p => new SelectListItem
            {
                Value = p.ProductId.ToString(),
                Text = p.ProductName
            }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel viewModel)
        {
            await _orderService.AddOrderWithDetailsAsync(viewModel);
            // 使用 TempData 傳遞成功訊息
            TempData["SuccessMessage"] = "訂單已成功新增！";

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var orderWithDetails = await _orderService.GetOrderWithDetailsAsync(id);
            if (orderWithDetails == null)
            {
                return NotFound();
            }

            var products = await _productService.GetAllProductsAsync();
            ViewBag.Products = products.Select(p => new SelectListItem
            {
                Value = p.ProductId.ToString(),
                Text = p.ProductName
            }).ToList();

            // 狀態選項列表
            ViewBag.StatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "A", Text = "新建訂單" },
                new SelectListItem { Value = "B", Text = "揀貨" },
                new SelectListItem { Value = "C", Text = "揀貨完畢" },
                new SelectListItem { Value = "D", Text = "已出貨" },
                new SelectListItem { Value = "E", Text = "取消" }
            };

            orderWithDetails.StatusDescription = GetStatusDescription(orderWithDetails.Status);


            return View(orderWithDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderViewModel viewModel)
        {

            await _orderService.UpdateOrderWithDetailsAsync(viewModel);

            TempData["SuccessMessage"] = "訂單已成功更新！";
            return RedirectToAction("Query");
        }

        [HttpGet]
        public async Task<IActionResult> Query(string customerName, string orderNo, DateTime? startDate, DateTime? endDate)
        {
            ViewData["CustomerName"] = customerName;
            ViewData["OrderNo"] = orderNo;
            ViewData["StartDate"] = startDate;
            ViewData["EndDate"] = endDate;

            var orders = await _orderService.QueryOrders(customerName, orderNo, startDate, endDate);
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> ViewOrder(int id)
        {
            var orderWithDetails = await _orderService.GetOrderWithDetailsAsync(id);
            if (orderWithDetails == null)
            {
                return NotFound();
            }

            var products = await _productService.GetAllProductsAsync();
            ViewBag.Products = products.Select(p => new SelectListItem
            {
                Value = p.ProductId.ToString(),
                Text = p.ProductName
            }).ToList();

            orderWithDetails.StatusDescription = GetStatusDescription(orderWithDetails.Status);

            return View(orderWithDetails);
        }



        // 狀態代碼轉換為中文描述
        private string GetStatusDescription(string status)
        {
            return status switch
            {
                "A" => "新建訂單",
                "B" => "揀貨",
                "C" => "揀貨完畢",
                "D" => "已出貨",
                "E" => "取消",
                _ => "未知狀態"
            };
        }

    }
}