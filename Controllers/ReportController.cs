using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop_giay_server._Controllers;
using shop_giay_server.data;
using shop_giay_server.models;

namespace shop_giay_server.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly DataContext _context;
        public ReportController(DataContext context)
        {
            _context = context;
        }

        public bool CompareDate(DateTime date1, DateTime date2)
        {
            return (date1.Year == date2.Year) && (date1.Month == date2.Month) && (date1.Day == date2.Day);
        }

        public async Task<List<Order>> GetOrder(DateTime date)
        {
            var orders = _context.Orders.AsEnumerable().Where(x => CompareDate(x.OrderDate, date) == true).ToList();

            return orders;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ReportDto dto)
        {
            var target = dto.Target;

            if (dto.FromDate == null || dto.ToDate == null || DateTime.Compare(dto.FromDate, dto.ToDate) >= 0)
            {
                return BadRequest(ResponseDTO.BadRequest());
            }

            List<string> categories = new List<string>();
            List<List<Order>> orderList = new List<List<Order>>() { };
            List<BestSale> bests = new List<BestSale>() { };
            List<CustomDate> months = new List<CustomDate>() { };

            var totalDay = (int)(dto.ToDate - dto.FromDate).TotalDays;
            int step = 1;

            if (totalDay > 60)
            {
                step = 30;
            }
            else if (totalDay > 14)
            {
                step = (int)totalDay / 7;
            }

            bool add = true;
            DateTime checkpoint = dto.FromDate;
            for (DateTime date = dto.FromDate; date <= dto.ToDate; date = date.AddDays(1))
            {
                if (step == 30)
                {
                    if (date.Month == checkpoint.Month)
                    {
                        if (add == true)
                        {
                            categories.Add(date.ToString("MMM yyyy"));
                            months.Add(new CustomDate { Month = date.Month, Year = date.Year });
                            add = false;
                            checkpoint = date;
                        }
                    }
                    else
                    {
                        categories.Add(date.ToString("MMM yyyy"));
                        months.Add(new CustomDate { Month = date.Month, Year = date.Year });
                        checkpoint = date;
                    }
                }
                else
                {
                    if ((int)(date - checkpoint).TotalDays == step || date == dto.ToDate || date == dto.FromDate)
                    {
                        categories.Add(date.ToString("dd/MM"));
                        checkpoint = date;
                    }
                    else
                    {
                        categories.Add("");
                    }
                }

                var orders = await GetOrder(date);
                orderList.Add(orders);

                if (orders != null)
                {
                    foreach (var order in orders)
                    {
                        var items = _context.OrderItems.Where(x => x.OrderId == order.Id).ToList();
                        foreach (var item in items)
                        {
                            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == item.StockId);
                            var shoes = await _context.Shoes.FirstOrDefaultAsync(x => x.Id == stock.ShoesId);
                            var shoesImages = _context.ShoesImages.Where(x => x.ShoesId == shoes.Id).ToList();

                            var image = "";
                            if (shoesImages.Count != 0)
                            {
                                image = shoesImages[0].ImagePath;
                            }

                            var id = stock.ShoesId;
                            var bestSale = bests.FirstOrDefault(x => x.Id == id);

                            if (bestSale == null)
                            {
                                var sale = new BestSale
                                {
                                    Id = id,
                                    Name = shoes.Name,
                                    Image = image,
                                    Price = item.Total,
                                    Amount = item.Amount
                                };

                                bests.Add(sale);
                            }
                            else
                            {
                                bestSale.Amount += item.Amount;
                                bestSale.Price += item.Total;
                            }
                        }
                    }
                }
            }

            bests = bests.OrderBy(x => x.Amount).Take(3).ToList();

            if (target == ReportConstants.REVENUE)
            {
                List<int> data = new List<int>();

                if (step != 30)
                {
                    foreach (var orders in orderList)
                    {
                        if (orders == null)
                        {
                            data.Add(0);
                        }
                        else
                        {
                            var total = 0;
                            foreach (var order in orders)
                            {
                                total += (int)order.Total;
                            }
                            data.Add(total);
                        }
                    }
                }
                else
                {
                    foreach (var month in months)
                    {
                        var orders = orderList.FindAll(x => x.Count > 0 && x[0].OrderDate.Month == month.Month && x[0].OrderDate.Year == month.Year).ToList();

                        if (orders.Count == 0)
                        {
                            data.Add(0);
                        }
                        else
                        {
                            var total = 0;
                            foreach (var order in orders)
                            {
                                foreach (var o in order)
                                {
                                    total += (int)o.Total;
                                }
                            }
                            data.Add(total);
                        }
                    }
                }

                var xAxis = new XAxis { categories = categories };
                var yAxis = new YAxis
                {
                    label = new Label { format = "{value} đ", style = new Style { } },
                    title = new Title { text = "Doanh thu", style = new Style { } }
                };

                var series = new Series
                {
                    name = "Doanh thu",
                    type = "spline",
                    data = data,
                    tooltip = new Tooltip { valueSuffix = "đ" }
                };

                var response = new ResponseReportDto
                {
                    xAxis = xAxis,
                    yAxis = yAxis,
                    series = new List<Series>() { series },
                    bestSales = bests
                };

                return Ok(ResponseDTO.Ok(response));
            }
            else if (target == ReportConstants.ORDERS)
            {
                List<int> data = new List<int>();

                if (step != 30)
                {
                    foreach (var orders in orderList)
                    {
                        if (orders == null)
                        {
                            data.Add(0);
                        }
                        else
                        {
                            data.Add(orders.Count);
                        }
                    }
                }
                else
                {
                    foreach (var month in months)
                    {
                        var orders = orderList.FindAll(x => x.Count > 0 && x[0].OrderDate.Month == month.Month && x[0].OrderDate.Year == month.Year).ToList();

                        if (orders.Count == 0)
                        {
                            data.Add(0);
                        }
                        else
                        {
                            var total = 0;
                            foreach (var order in orders)
                            {
                                total += order.Count;
                            }
                            data.Add(total);
                        }
                    }
                }

                var xAxis = new XAxis { categories = categories };
                var yAxis = new YAxis
                {
                    label = new Label { format = "{value} đơn", style = new Style { } },
                    title = new Title { text = "Đơn hàng", style = new Style { } }
                };

                var series = new Series
                {
                    name = "Doanh thu",
                    type = "column",
                    data = data,
                    tooltip = new Tooltip { valueSuffix = " đơn" }
                };

                var response = new ResponseReportDto
                {
                    xAxis = xAxis,
                    yAxis = yAxis,
                    series = new List<Series>() { series },
                    bestSales = bests
                };

                return Ok(ResponseDTO.Ok(response));
            }

            return Ok(ResponseDTO.Ok("Ok"));
        }
    }

    public class CustomDate
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class BestSale
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public int Amount { get; set; }
    }

    static class ReportConstants
    {
        public const int REVENUE = 1;
        public const int ORDERS = 2;
        public const int MEMBERS = 3;
    }

    public class ReportDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Target { get; set; }
    }

    public class ResponseReportDto
    {
        public XAxis xAxis { get; set; }

        public YAxis yAxis { get; set; }

        public List<Series> series { get; set; }

        public List<BestSale> bestSales { get; set; }
    }

    public class XAxis
    {
        public List<string> categories { get; set; }
    }

    public class YAxis
    {
        public Label label { get; set; }
        public Title title { get; set; }
    }

    public class Label
    {
        public string format { get; set; }
        public Style style { get; set; }
    }

    public class Style
    {
        public string color { get; set; } = "#2f7ed8";
    }

    public class Title
    {
        public string text { get; set; }
        public Style style { get; set; }
    }

    public class Series
    {
        public string name { get; set; }
        public string type { get; set; }
        public List<int> data { get; set; }
        public Tooltip tooltip { get; set; }
    }

    public class Tooltip
    {
        public string valueSuffix { get; set; }
    }
}