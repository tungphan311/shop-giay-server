using shop_giay_server.models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server._Repository;
using Microsoft.Extensions.Logging;
using shop_giay_server.Dtos;

namespace shop_giay_server._Controllers
{
    public class ImportController : GeneralController<Import, ImportDTO>
    {
        public ImportController(IAsyncRepository<Import> repo, ILogger<ImportController> logger)
            : base(repo, logger)
        { }
    }
}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using shop_giay_server.data;
//using shop_giay_server.models;
//using shop_giay_server._Services;
//using shop_giay_server._Repository;
//using shop_giay_server._Controllers;
//using Microsoft.Extensions.Logging;

//namespace shop_giay_server
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ImportController: ControllerBase
//    // public class ImportController: 
//    {
//        private readonly IAsyncRepository<Import> _importRepo;
//        private readonly IAsyncRepository<ImportDetail> _importDetailRepo;

//        public ImportController(IAsyncRepository<Import> importRepo, IAsyncRepository<ImportDetail> importDetailRepo)
//            // : base(importRepo)
//        {
//            _importRepo = importRepo;
//            _importDetailRepo = importDetailRepo;
//        }

//       // GET: api/import
//        //[HttpGet]
//        //public async Task<IActionResult> GetAll()
//        //{
//        //    var items = await _importRepo.GetAll();
//        //    var res = new Response<Import>(new List<Import>(items));
//        //    return Ok(res);
//        //}



//        // PUT: api/Cart/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for
//        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutCart(int id, Import item)
//        {
//            if (id != item.Id)
//            {
//                return BadRequest();
//            }

//            if (!Exists(id))
//            {
//                return NotFound();
//            }

//            var resultItem = await _importRepo.Update(item);

//            if (resultItem == null)
//            {
//                return BadRequest();
//            }

//            return Ok(new
//            {
//                code = "OK",
//                msg = "Updated.",
//                data = item,
//                total = 1
//            });
//        }

//        // POST: api/Cart
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for
//        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
//        [HttpPost]
//        public async Task<IActionResult> Post(Import item)
//        {
//            var result = await _importRepo.Add(item);
//            if (result == null)
//            {
//                return BadRequest();
//            }

//            return Ok(new
//            {
//                code = "OK",
//                msg = "Created.",
//                data = item,
//                total = 1
//            });
//        }

//        // DELETE: api/Cart/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var result = await _importRepo.Remove(id);
//            if (!result)
//            {
//                return BadRequest(new
//                {
//                    code = "ERROR",
//                    msg = $"Cannot delete item id: {id}",
//                    data = new EmptyResult(),
//                    total = 0
//                });
//            }

//            return Ok(new
//            {
//                code = "OK",
//                msg = "Deleted.",
//                data = new EmptyResult(),
//                total = 0
//            });
//        }

//        private bool Exists(int id)
//        {
//            return _importRepo.GetById(id) != null;
//        }
//    }
//}
