using ECommerce.Models;
using ECommerce.Repository;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

public class OrderService
{
    private readonly OrderRepository _orderRepository;
    private readonly OrderDetailRepository _orderDetailRepository;
    private readonly ProductRepository _productRepository;
    private readonly OrderNoSeqRepository _orderNoSeqRepository;

    public OrderService(OrderRepository orderRepository, OrderDetailRepository orderDetailRepository, ProductRepository productRepository, OrderNoSeqRepository orderNoSeqRepository)
    {
        _orderRepository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
        _productRepository = productRepository;
        _orderNoSeqRepository = orderNoSeqRepository;
    }

    public async Task<OrderViewModel> GetOrderWithDetailsAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return null;

        var orderDetails = await _orderDetailRepository.GetByOrderIdAsync(id);

        var orderViewModel = new OrderViewModel
        {
            OrderId = order.OrderId,
            OrderNo = order.OrderNo,
            Customer = order.Customer,
            Status = order.Status,
            CreateDate = order.CreateDate,
            UpdateDate = order.UpdateDate,
            ShipDate = order.ShipDate,
            Remark = order.Remark,
            OrderDetails = orderDetails.Select(od => new OrderDetailViewModel
            {
                OrderDetailId = od.OrderDetailId,
                ProductId = od.ProductId,
                ProductName = _productRepository.GetByIdAsync(od.ProductId).Result?.ProductName,
                Quantity = od.Quantity
            }).ToList()
        };

        return orderViewModel;
    }

    public async Task AddOrderWithDetailsAsync(OrderViewModel orderViewModel)
    {
        // 1. �ͦ��q��s��
        var today = DateTime.Now.ToString("yyyyMMdd");
        var orderNoSeq = await _orderNoSeqRepository.GetOrderNoSeq(today) ?? new OrderNoSeq { OrderDate = today, Seq = 0 };
        orderNoSeq.Seq++;
        var orderNo = $"{today}{orderNoSeq.Seq.ToString("D8")}";

        // 2. �s�W�q��
        var order = new Order
        {
            OrderNo = orderNo,
            Customer = orderViewModel.Customer,
            Status = "A", // �w�]���A�� "�s�حq��"
            CreateDate = DateTime.Now,
            ShipDate = orderViewModel.ShipDate,
            Remark = orderViewModel.Remark
        };
        await _orderRepository.AddAsync(order);

        // 3. ��s OrderNoSeq
        if (orderNoSeq.Seq == 1)
        {
            await _orderNoSeqRepository.AddAsync(orderNoSeq);
        }
        else
        {
            await _orderNoSeqRepository.UpdateAsync(orderNoSeq);
        }

        // 4. �s�W�q�����
        foreach (var detail in orderViewModel.OrderDetails)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = order.OrderId,
                ProductId = detail.ProductId,
                Quantity = detail.Quantity,
                CreateDate = DateTime.Now
            };
            await _orderDetailRepository.AddAsync(orderDetail);
        }
    }



    public async Task UpdateOrderWithDetailsAsync(OrderViewModel orderViewModel)
    {
        // ��s Order
        var order = await _orderRepository.GetByIdAsync(orderViewModel.OrderId);
        if (order == null) throw new Exception("Order not found");

        order.Customer = orderViewModel.Customer;
        order.Status = orderViewModel.Status;
        order.UpdateDate = DateTime.Now;
        order.ShipDate = orderViewModel.ShipDate;
        order.Remark = orderViewModel.Remark;

        await _orderRepository.UpdateAsync(order);

        // ��s OrderDetails
        var existingDetails = await _orderDetailRepository.GetByOrderIdAsync(orderViewModel.OrderId);
        // �R�����s�b������
        var detailsToDelete = existingDetails
            .Where(ed => !orderViewModel.OrderDetails.Any(od => od.OrderDetailId == ed.OrderDetailId))
            .ToList();

        if (detailsToDelete.Any())
        {
            await _orderDetailRepository.DeleteRangeAsync(detailsToDelete.Select(d => d.OrderDetailId).ToList());
        }


        // �s�W�Χ�s����
        foreach (var detailViewModel in orderViewModel.OrderDetails)
        {
            if (detailViewModel.OrderDetailId == 0)
            {
                // �s�W
                var newDetail = new OrderDetail
                {
                    OrderId = orderViewModel.OrderId,
                    ProductId = detailViewModel.ProductId,
                    Quantity = detailViewModel.Quantity,
                    CreateDate = DateTime.Now
                };
                await _orderDetailRepository.AddAsync(newDetail);
            }
            else
            {
                // ��s
                var existingDetail = existingDetails.FirstOrDefault(ed => ed.OrderDetailId == detailViewModel.OrderDetailId);
                if (existingDetail != null)
                {
                    existingDetail.ProductId = detailViewModel.ProductId;
                    existingDetail.Quantity = detailViewModel.Quantity;
                    existingDetail.UpdateDate = DateTime.Now;
                    await _orderDetailRepository.UpdateAsync(existingDetail);
                }
            }
        }
    }


    public async Task<List<Order>>  QueryOrders(string customerName, string orderNo, DateTime? startDate, DateTime? endDate)
    {
        var orders = await _orderRepository.GetOrders(orderNo, customerName, startDate,endDate);

        return orders.ToList();
    }
}
