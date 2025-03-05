using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileEditor.Data;
using ProfileEditor.Helpers;
using ProfileEditor.Models;
using ProfileEditor.Services.ImageStore;
using ProfileEditor.Services.Persons;
using System.IO;

namespace ProfileEditor.Controllers {
    [Authorize]
    public class PersonController : Controller {
        private readonly IPersonsService _personsService;
        private readonly IImageStoreService _imageStore;

        public PersonController(IPersonsService personsService, IImageStoreService imageStore) {
            this._personsService = personsService;
            this._imageStore = imageStore;
        }

        // GET: Person
        public async Task<IActionResult> Index() {
            var persons = await _personsService.GetPersonsAsync();
            return View(persons ?? new List<PersonVm>());
        }

        // GET: Person/Details/{id}
        public async Task<IActionResult> Details(Guid? id) {
            try {
                var person = await _personsService.GetPersonAsync(Guard.NotNull(id, nameof(id)));
                return View(Guard.NotNull(person, nameof(person)));
            }
            catch (ArgumentNullException) {
                return NotFound();
            }
        }

        // GET: Person/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("FirstName,LastName,EmailAddress,PhoneNumber,DateOfBirth,Gender,ProfilePicture")] PersonVm personVm) {
            if (ModelState.IsValid) {
                var updated = await _personsService.CreatePersonAsync(personVm);
                if (personVm.ProfilePicture != null) {
                    using var memoryStream = new MemoryStream();
                    await personVm.ProfilePicture.CopyToAsync(memoryStream);
                    await _imageStore.UploadImage(new ProfileImage(updated.Id, memoryStream.ToArray(), personVm.ProfilePicture.ContentType));
                }

                return RedirectToAction(nameof(Index));
            }
            return View(personVm);
        }

        // GET: Person/Edit/{id}
        public async Task<IActionResult> Edit(Guid? id) {
            try {
                var person = await _personsService.GetPersonAsync(Guard.NotNull(id, nameof(id)));
                return View(Guard.NotNull(person, nameof(person)));
            }
            catch (ArgumentNullException) {
                return NotFound();
            }
        }

        // POST: Person/Edit/{id}
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("Id,FirstName,LastName,EmailAddress,PhoneNumber,DateOfBirth,Gender,ProfilePicture")] PersonVm personVm) {
            if (id != personVm.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                await _personsService.UpdatePersonAsync(personVm);
                return RedirectToAction(nameof(Index));
            }
            return View(personVm);
        }

        // GET: Person/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id) {
            try {
                var person = await _personsService.GetPersonAsync(Guard.NotNull(id, nameof(id)));
                return View(Guard.NotNull(person, nameof(person)));
            }
            catch (ArgumentNullException) {
                return NotFound();
            }
        }

        // POST: Person/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id) {
            await _personsService.DeletePersonAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Person/ProfilePicture/{id}
        public async Task<IActionResult> ProfilePicture(Guid? id) {
            try {
                var image = await _imageStore.FetchImage(Guard.NotNull(id, nameof(id)));
                var safeImage = Guard.NotNull(image, nameof(image));
                return File(safeImage.ImageData, safeImage.ContentType);
            } catch {
                return File("~/images/default-profile.png", "image/png");
            }
        }

        // POST: Person/UploadPicture/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPicture(Guid? id, ImageUploadVm? imageUpload) {
            try {
                var safeId = Guard.NotNull(id, nameof(id));
                var safeData = Guard.NotNull(imageUpload?.Image, nameof(imageUpload));

                using var memoryStream = new MemoryStream();
                await safeData.CopyToAsync(memoryStream);
                var profileImage = new ProfileImage(safeId, memoryStream.ToArray(), safeData.ContentType);
                
                await _imageStore.UploadImage(profileImage);
                return RedirectToAction(nameof(Edit), new { id });
            }
            catch {
                return NotFound();
            }
        }

        // POST: Person/DeletePicture/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePicture(Guid? id) {
            await _imageStore.DeleteImage(Guard.NotNull(id, nameof(id)));
            return RedirectToAction(nameof(Edit), new {id});
        }
    }
}
