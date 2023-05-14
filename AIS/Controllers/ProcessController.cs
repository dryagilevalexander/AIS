using AIS.Models;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AIS.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using AspNetCore;
using Microsoft.AspNetCore.Mvc;
using static AIS.Controllers.ProcessController;
using DocumentFormat.OpenXml.Presentation;
using System.Linq;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations;
using AIS.ViewModels.DocumentsViewModels;
using AIS.ViewModels.ProcessViewModels;
using AIS.ViewModels.EmployersViewModels;

namespace AIS.Controllers
{
    [Authorize(Roles = "admin, employee")]
    public class ProcessController : Controller
    {
        private ILetterService _letterService;

        public ProcessController(ILetterService letterService)
        {
            _letterService = letterService;
        }

        #region [Letters]
        public async Task<IActionResult> Letters()
        {
            return View(await _letterService.GetAllLettersEagerLoading());
        }

        public async Task<IActionResult> CreateLetter()
        {
            CreateLetterViewModel model = new CreateLetterViewModel();
            await model.Fill(_letterService);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLetter(CreateLetterViewModel model)
        {
            await _letterService.CreateLetter(model);
            return RedirectToAction("Letters");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLetter(int id)
        {
            await _letterService.DeleteLetter(id);
            return RedirectToAction("Letters");
        }

        public async Task<IActionResult> EditLetter(int id)
        {
            EditLetterViewModel model = new EditLetterViewModel();
            await model.Fill(id, _letterService);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditLetter(Letter letter)
        {
            await _letterService.EditLetter(letter);
            return RedirectToAction("Letters");
        }
        #endregion

        #region[pref]
        public IActionResult Pref()
        {
            return View();
        }
        #endregion

    }
}
