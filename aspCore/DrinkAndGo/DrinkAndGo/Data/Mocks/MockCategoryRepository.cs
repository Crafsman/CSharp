using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Data.Mocks
{
    public class MockCategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> Categories
        {
            get
            {
                return new List<Category>
                {
                    new Category{ CategoryName = "Alcoholic", Description = "All alcohlic drinks"},
                    new Category{ CategoryName="Non-alcoholic", Description="All non-alcoholic drinks"}
                };
            }
        }
    }
}
