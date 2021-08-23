using System;
using IrasBlog.Data;
using IrasBlog.Helpers;
using IrasBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IrasBlog.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ILogger<BlogPostsController> _logger;

        public BlogPostsController(IBlogRepository blogRepository, ILogger<BlogPostsController> logger)
        {
            _blogRepository = blogRepository;
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View(_blogRepository.GetAllBlogs());
        }

        public ActionResult Details(string slug)
        {
            BlogPost blogPost = _blogRepository.GetBlogBySlug(slug);
            if (blogPost == null)
            {
                return NotFound();
            }
            return View(blogPost);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var dateTimeStamp = $"{DateTime.Now.ToString("yyyyMddHHmmss")}";
            BlogPost defaultBlogPost = new BlogPost
            {
                Title = "Test Title " + dateTimeStamp,
                BlogPostBody = "Test Body " + dateTimeStamp,
                Abstract = "My Abstract " + dateTimeStamp
            };
            return View(defaultBlogPost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                blogPost.Slug = blogPost.Title.GenerateSlug();
                blogPost.Created = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                _blogRepository.AddEntity(blogPost);
                _blogRepository.SaveAll();
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string slug)
        {
            BlogPost blogPost = _blogRepository.GetBlogBySlug(slug);
            if (blogPost == null)
            {
                return NotFound();
            }
            return View(blogPost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                if (!_blogRepository.SaveChanges(blogPost))
                {
                    _logger.LogError("Failed to Save Changes");
                    return BadRequest("Failed to Save Changes");
                }
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string slug)
        {
            BlogPost blogPost = _blogRepository.GetBlogBySlug(slug);
            if (blogPost == null)
            {
                return NotFound();
            }
            return View(blogPost);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string slug)
        {
            BlogPost blogPost = _blogRepository.GetBlogBySlug(slug);
            if (blogPost == null)
            {
                return NotFound();
            }
            blogPost.Published = false;
            _blogRepository.SaveAll();
            return RedirectToAction("Index");
        }
    }
}
