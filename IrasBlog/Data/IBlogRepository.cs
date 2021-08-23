using IrasBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrasBlog.Data
{
    public interface IBlogRepository
    {
        IEnumerable<BlogPost> GetAllBlogs();
        BlogPost GetBlogById(int id);
        BlogPost GetBlogBySlug(string slug);

        bool SaveAll();
        void AddEntity(object model);
        bool SaveChanges(object model);
    }
}
