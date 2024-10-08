﻿using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class CategoryRepository : Repository<Category> , ICategoryRepository
    {
        private ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var categorydb = _context.Categories.FirstOrDefault(x=>x.Id== category.Id);
            if (categorydb != null)
            {
                categorydb.Name = category.Name;
                categorydb.DisplayOrder = category.DisplayOrder;
            }
        }
    }
}
