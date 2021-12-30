using IrasBlog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IrasBlog.Data
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogContext _context;
        private readonly ILogger<BlogRepository> _logger;

        public BlogRepository(BlogContext context, ILogger<BlogRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<BlogPost> GetAllBlogs()
        {
            try
            {
                _logger.LogInformation("GetAllBlogs was called");
                return _context.BlogPosts;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get all blogs: {ex}");
                return null;
            }
        }

        public BlogPost GetBlogById(int id)
        {
            try
            {
                _logger.LogInformation("GetBlogById was called");
                return _context.BlogPosts.FirstOrDefault(b => b.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get blog by id: {ex}");
                return null;
            }
        }

        public BlogPost GetBlogBySlug(string slug)
        {
            try
            {
                _logger.LogInformation("GetBlogBySlug was called");
                return _context.BlogPosts.FirstOrDefault(b => b.Slug == slug);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get blog by slug: {ex}");
                return null;
            }
        }               

        public void AddEntity(object model)
        {
            _context.Add(model);
        }

        public bool SaveAll()
        {
            try
            {
                _logger.LogInformation("SaveAll was called");
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save to DB: {ex}");
                return false;
            }
        }

        public bool SaveChanges(object model)
        {
            try
            {
                _logger.LogInformation("SaveChanges (to model) was called");
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save changes to DB: {ex}");
                return false;
            }
            return true;
        }
    }
}
