using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AIS.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using AIS.ViewModels.TasksViewModels;
using AIS.ErrorManager;
using System.Net;

namespace AIS.Controllers
{
    [Authorize(Roles = "admin, employee")]
    public class EnclosuresController : Controller
    {
        private IEnclosureService _enclosureService;

        public EnclosuresController(IEnclosureService enclosureService)
        {
            _enclosureService = enclosureService;
        }
        #region [MyTasks]
        
        [HttpPost]
        public async Task<IActionResult> DeleteMyEnclosure(int id)
        {
            await _enclosureService.DeleteMyEnclosure(id);
            return new EmptyResult();
        }
        #endregion
    }
}
