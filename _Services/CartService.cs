using System;
using shop_giay_server._Services;
using shop_giay_server.models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shop_giay_server.data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace shop_giay_server
{
    public class CartService : IService<Cart>
    {
        private readonly DataContext _context;

        public CartService(DataContext context)
        {
            _context = context;
        }

        public async Task<Cart> Create(Cart item)
        {
            await _context.Carts.AddAsync(item);
            _context.SaveChanges();
            return item;
        }

        public async Task<Cart> Read(int id)
        {
            return await _context.Carts.FindAsync(id);
        }

        public async Task<Cart> Update(Cart item)
        {
            var track = _context.Carts.Attach(item);
            track.State = EntityState.Modified;
            _context.SaveChanges();
            //return ;
            return null;
        }

        public Task<Cart> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Cart>> GetAll()
        {
            return await _context.Carts.ToListAsync();
        }


        //public async Cart Create(Cart item)
        //{
        //    await _context.Carts.Add(item);
        //    _context.SaveChanges();
        //    return item;
        //}

        //public Cart Read(int id)
        //{
        //    return _context.Carts.Find(id);
        //}

        //public Cart Update(Cart itemChanges)
        //{
        //    var item = _context.Carts.Attach(itemChanges);
        //    item.State = EntityState.Modified;
        //    _context.SaveChanges();
        //    return itemChanges;
        //}

        //public Cart Delete(int id)
        //{
        //    var item = _context.Carts.Find(id);
        //    if (item != null)
        //    {
        //        _context.Carts.Remove(item);
        //        _context.SaveChanges();
        //    }
        //    return item;
        //}

        //public bool Exists(int id)
        //{
        //    return _context.Carts.Any(i => i.Id == id);
        //}

        //public IEnumerable<Cart> GetAll()
        //{
        //    return _context.Carts;
        //}

    }
}
