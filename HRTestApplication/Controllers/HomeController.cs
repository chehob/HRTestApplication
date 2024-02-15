using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HRTestApplication.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HRTestApplication.Application;
using HRTestApplication.Application.Common.Interfaces;

namespace HRTestApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHRWork _HRWork;
        private readonly ILogger<HomeController> _logger;        

        public HomeController(ILogger<HomeController> logger, IHRWork HRWork)
        {
            _HRWork = HRWork;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            await _HRWork.DoHRWork();

            return View();
        }
    }
}
