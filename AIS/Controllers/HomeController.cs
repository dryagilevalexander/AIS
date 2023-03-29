using AIS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using Microsoft.AspNetCore.Hosting.Server;
using System.Web;
using AIS.ViewModels;
using System.Reflection.Metadata.Ecma335;
using System.IO;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using AIS.Services;
using System.Threading.Tasks;
using System.Net;
using System.Security.Principal;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static Npgsql.PostgresTypes.PostgresCompositeType;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace AIS.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}