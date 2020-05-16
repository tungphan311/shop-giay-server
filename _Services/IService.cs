using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace shop_giay_server._Services
{
    public interface IService<Entity> where Entity: class
    {
        public Task<Entity> Create(Entity item);

        public Task<Entity> Read(int id);

        public Task<Entity> Update(Entity item);

        public Task<Entity> Delete(int id);

        public bool Exists(int id);

        public Task<IEnumerable<Entity>> GetAll();
    }
}
