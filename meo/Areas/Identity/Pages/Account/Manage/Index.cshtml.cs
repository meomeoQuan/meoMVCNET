// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using meo.DataAccess.Repository;
using meo.DataAccess.Repository.IRepository;
using meo.Models;
using meo.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace meo.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
     

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string? userImage { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var userImage = _unitOfWork.ApplicationUser.Get(u => u.Email == user.Email).userImage;
            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
                ,
                userImage = userImage
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile ? file)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            var applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Email == user.Email);

            // Handle ImageUrl validation
            if (file == null && string.IsNullOrEmpty(applicationUser.userImage))
            {
                ModelState.AddModelError("applicationUser.userImage", "Please upload an image.");
            }

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\user");
                    //---------------------------------------------------------------------

                    if (!string.IsNullOrEmpty(applicationUser.userImage))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, applicationUser.userImage.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    //----------------------------------------------------------------------

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                     applicationUser.userImage = @"\images\product\" + fileName;
                _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();

            }


                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (Input.PhoneNumber != phoneNumber)
                {
                    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                    if (!setPhoneResult.Succeeded)
                    {
                        StatusMessage = "Unexpected error when trying to set phone number.";
                        return RedirectToPage();
                    }
                }

                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Your profile has been updated";
                return RedirectToPage();
            }



        }
    }

