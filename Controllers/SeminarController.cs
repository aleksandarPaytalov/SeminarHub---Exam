using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeminarHub.Contracts;
using SeminarHub.Models;
using System.Security.Claims;

namespace SeminarHub.Controllers
{
    [Authorize]
    public class SeminarController : Controller
    {
        private readonly ISeminarService _service;

        public SeminarController(ISeminarService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var seminars = await _service.GetAllSeminarsAsync();
            
            return View(seminars);
        }
        
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories =  await _service.GetAllCategoriesAsync();
            var model = new AddNewSeminarViewModel()
            {
                Categories = categories
            };

            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(AddNewSeminarViewModel model)
        {
            model.Categories = await _service.GetAllCategoriesAsync();
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = GetUserId();
            await _service.AddNewSeminarAsync(model, userId);
            

            return RedirectToAction(nameof(All));
        }
        
        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();
            var model = await _service.GetAllSeminarsFromCollectionAsync(userId);
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(All));
            }

            var userId = GetUserId();
            bool isAdded = false;
            var boolChecker = await _service.AddToCollectionAsync(id, userId, isAdded);

            if (boolChecker)
            {
                return RedirectToAction(nameof(Joined));
            }
            else
            {
                return RedirectToAction(nameof(All));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            string userId = GetUserId();
            await _service.RemoveFromCollectionAsync(id, userId);

            return RedirectToAction(nameof(Joined));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _service.GetSeminarForEditAsync(id);

            if (model == null)
            {
                return RedirectToAction(nameof(All));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddNewSeminarViewModel model, int id)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _service.EditSeminarAsync(model, id);
            

            return RedirectToAction(nameof(All));
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {

            var model = await _service.GetSeminarDetailsAsync(id);
            if (model == null)
            {
                return RedirectToAction(nameof(All));
            }

            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            var model = await _service.GetSeminarForDeletingAsync(id);

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetUserId();
            await _service.DeleteSeminarByIdAsync(id, userId);
            
            return RedirectToAction(nameof(All));
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        
    }
}
