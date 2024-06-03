using DotkonBlog.Areas.Identity.Data;
using DotkonBlog.Data;
using DotkonBlog.Gateway;
using DotkonBlog.Hubs;
using DotkonBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DotkonBlog.Controllers
{
    public class PostController : Controller
    {
        private readonly ILogger<PostController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DotkonBlogDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        private PostGateway postGateway;
        public PostController(ILogger<PostController> logger, 
                              UserManager<ApplicationUser> userManagaer, 
                              DotkonBlogDbContext context,
                              IHubContext<NotificationHub> hubContext) 
        {
            _logger = logger;
            _userManager = userManagaer;
            _context = context;
            _hubContext = hubContext;
            postGateway = new PostGateway(_context, _hubContext);
        }        
        public async Task<IActionResult> Index()
        {
            if (ViewData["UserId"] == null)
            {
                ViewData["UserId"] = _userManager.GetUserId(this.User);
            }

            @ViewData["NotificationCount"] = 0;

            IEnumerable<PostModel>? posts = null;

            try
            {
                posts = await postGateway.ListPosts();
            }
            catch(Exception ex)
            {                
                return NotFound(ex.Message);
            }
            
            return View(posts);
        }

        [Authorize]
        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ViewData["UserId"] == null)
            {
                ViewData["UserId"] = _userManager.GetUserId(this.User);
            }

            var post = await postGateway.GetPost(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [Authorize]
        // GET: Posts/Create
        public IActionResult Create()
        {
            if (ViewData["UserId"] == null)
            {
                ViewData["UserId"] = _userManager.GetUserId(this.User);
            }

            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,UserId,CreatedDate")] PostModel post)
        {

            var result = await postGateway.InsertPost(post);

            // Notificação via WebSocket
            await postGateway.NotifyNewPost(post);
            Thread.Sleep(2000);

            return RedirectToAction(nameof(Index));
            
        }

        [Authorize]
        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ViewData["UserId"] == null)
            {
                ViewData["UserId"] = _userManager.GetUserId(this.User);
            }
            
            var post = await postGateway.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,UserId,CreatedDate")] PostModel post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            try
            {
                var result = await postGateway.EditPost(post);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!postGateway.PostExists(post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await postGateway.GetPost(id);
            
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {            
            var post = await postGateway.GetPost(id);
            var result = await postGateway.DeletePost(post);
            
            return RedirectToAction(nameof(Index));
        }                

    }
}