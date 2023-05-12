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
using AIS.ViewModels.EmployersViewModels;
using AIS.ErrorManager;
using System.Net;

namespace AIS.Controllers
{
    [Authorize(Roles = "admin, employee")]
    public class EmployersController : Controller
    {

        private IPartnerService _partnerService;
        private IEmployeeService _employeeService;

        public EmployersController(IPartnerService partnerService,
                                 IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            _partnerService = partnerService;
        }
        #region [Employeers]
        public async Task<IActionResult> CreateEmployee(int id)
        {
            IEnumerable<Partner> partners = await _partnerService.GetPartners();
            CreateEmployeeViewModel model = new CreateEmployeeViewModel()
            {
                PartnerId = id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _employeeService.CreateEmployee(model);
            return RedirectToAction("EditPartnerOrganization", "Partners", new { id = model.PartnerId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            Employee? employee = await _employeeService.GetEmployee(id);
            if (employee == null) throw new AisException("Сотрудник не найден", HttpStatusCode.BadRequest);
            await _employeeService.DeleteEmployee(id);
            return RedirectToAction("EditPartnerOrganization", "Partners", new { id = employee.PartnerId.Value });        
        }

        public async Task<IActionResult> EditEmployee(int id)
        {
            Employee? employee = await _employeeService.GetEmployee(id);
            if (employee == null) throw new AisException("Сотрудник не найден", HttpStatusCode.BadRequest);
            EditEmployeeViewModel model = new EditEmployeeViewModel()
            {
                Id = employee.Id,   
                Name = employee.Name,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Address = employee.Address,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                PartnerId = employee.PartnerId.Value
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditEmployee(EditEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _employeeService.EditEmployee(model);
            return RedirectToAction("EditPartnerOrganization", "Partners", new { id = model.PartnerId });         
        }
        #endregion
    }
}
