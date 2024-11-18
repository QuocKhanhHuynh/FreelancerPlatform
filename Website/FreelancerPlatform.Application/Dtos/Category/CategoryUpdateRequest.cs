using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Category
{
    public class CategoryUpdateRequest
    {
        public int CategoriesId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
