using BugTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker
{
    public class HomeController : Controller
    {

        public ViewResult Index()
        {
            var viewModel = new MainDashboardViewModel();

            return View(viewModel);
        }
    }
}
