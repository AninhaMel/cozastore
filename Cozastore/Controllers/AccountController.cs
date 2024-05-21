using System.Net.Mail;
using Cozastore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;


namespace Cozastore.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(
        ILogger<AccountController> logger,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager
        )
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]

    public IActionResult Login(string returnUrl)
    {
        LoginVM login = new()
        {
            UrlRetorno = returnUrl ?? Url.Content("~/")
        };
        return View(login);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task <IActionResult> Login(LoginVM login)
    {
        if (ModelState.IsValid)
        {
            string userName = login.Email;
            if (IsValidEmail(userName))
            {
                var user = await _userManager.FindByEmailAsync(userName);
                if (user != null)
                    userName = user.UserName;
            }

            var result = await _signInManager.PasswordSignInAsync(
                userName, login.Senha, login.Lembrar, lockoutOnFailure: true
            );

            if (result.Succeeded)
            {
                _logger.LogInformation($"Usuário {userName} acessou o sistema");
                return LocalRedirect(login.UrlRetorno);
            }

            if(result.IsLockedOut)
            {
                _logger.LogWarning($"Usuário {userName} está bloqueado");
                ModelState.AddModelError(string.Empty, "Conta Bloqueada! Aguarde alguns minutos para continuar!");

            }

            
            ModelState.AddModelError(string.Empty, "Usuário e/ou Senha Inválidos!!!");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        PublicClientApplication async async Task<IActionResult> Logout()
        {
            _logger.LogInformation($"Usuário {ClaimTypes.EMail} saiu do sistema!");
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
   
    public IActionResult AcessDenied()
    {
        return View();
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            MailAddress mail = new(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
