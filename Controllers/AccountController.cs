using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using My_Final_Project.DTO;
using My_Final_Project.Models;
using My_Final_Project.Reposatory;
using My_Final_Project.Services;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace My_Final_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ICartRepo cartRepo;
        private readonly IwishListRepo wishListRepo;
        private readonly IEmailService emailService;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ICartRepo cartRepo, IwishListRepo wishListRepo, IEmailService emailService, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.cartRepo = cartRepo;
            this.wishListRepo = wishListRepo;
            this.emailService = emailService;
            this.signInManager = signInManager;
        }
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
    {
        new Claim("UserId", user.Id),
        new Claim("role", user.Role),
        new Claim("UserName", user.UserName),
        new Claim("Email", user.Email),
        new Claim("Name", user.Name),
        new Claim("Age", user.Age.ToString()),
        new Claim("Nationality", user.Nationality),
        new Claim("Phone", user.Phone)
    };

            var expDate = DateTime.Now.AddDays(2);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudiance"],
                claims: claims,
                expires: expDate,
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return await Task.FromResult(tokenHandler.WriteToken(token));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registration(RegisterUserDTO registerDTO)
        {
            var EmailExists = userManager.Users.Any(x => x.Email == registerDTO.Email);
            if (EmailExists)
            {
                var error = new
                {
                    ErrorMessage = "Email already exists",
                };
                return Conflict(error);
            }
            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                Name = registerDTO.Name,
                Age = registerDTO.Age,
                Role = registerDTO.Role,
                Nationality = registerDTO.Nationality,
                Phone = registerDTO.Phone
            };
            IdentityResult result = await userManager.CreateAsync(applicationUser, registerDTO.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            if (result.Succeeded)
            {
                await userManager.AddClaimsAsync(applicationUser, new List<Claim>
                {
            new Claim("UserId",applicationUser.Id),
            new Claim("role",registerDTO.Role),
            new Claim("Email", registerDTO.Email),
            new Claim("Name", registerDTO.Name),
            new Claim("Age",registerDTO.Age.ToString() ),
            new Claim("Nationality",registerDTO.Nationality),
            new Claim("Phone",registerDTO.Phone)
                });
                Carts carts = new Carts
                {
                    UserID = applicationUser.Id,
                    TotalPrice = 0,
                    TotalQuantity = 0,
                };
                cartRepo.CreateCart(carts);
                WishList wishlist = new WishList
                {
                    UserID = applicationUser.Id
                };
                wishListRepo.CreateWishList(wishlist);
            }

            //    var token = GenerateJwtToken(applicationUser);
            var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = applicationUser.Email }, Request.Scheme);
            var message = new Message(new string[] { applicationUser.Email }, "Confirmation email link", confirmationLink!);
            emailService.SendEmail(message);

            return StatusCode(StatusCodes.Status200OK,
                      new Models.Response { Status = "Success", Message = $"User created & Email Sent to {applicationUser.Email} SuccessFully" });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDTO credntials)
        {
            var requiredEmail = await userManager.FindByEmailAsync(credntials.Email);


            if (requiredEmail == null)
            {
                var error = new
                {
                    ErrorMessage = "Email or Password is incorrect",
                };
                return Unauthorized(error);
            }
            var LockedUser = await userManager.IsLockedOutAsync(requiredEmail);
            var isAuth = await userManager.CheckPasswordAsync(requiredEmail, credntials.Password);

            if (!isAuth)
            {
                await userManager.AccessFailedAsync(requiredEmail);

                // Check if the user should be locked out
                if (await userManager.IsLockedOutAsync(requiredEmail))
                {
                    var error1 = new
                    {
                        ErrorMessage = "User is locked out. Please try again later.",
                    };
                    return Unauthorized(error1);
                }
                var error = new
                {
                    ErrorMessage = "Email or Password is incorrect",
                };
                return Unauthorized(error);
            }

            await userManager.ResetAccessFailedCountAsync(requiredEmail);
            if (LockedUser)
            {
                var error = new
                {
                    ErrorMessage = "User Locked out for 15 minutes for registering 3 wrong times",
                };
                return Unauthorized(error);
            }
            if (requiredEmail.TwoFactorEnabled)
            {
                //enablig twofactor
                //  await userManager.SetTwoFactorEnabledAsync(requiredEmail, true);


                await signInManager.SignOutAsync();
                await signInManager.PasswordSignInAsync(requiredEmail, credntials.Password, false, true);

                if (requiredEmail == null || !await userManager.CheckPasswordAsync(requiredEmail, credntials.Password))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized,
                    new { Status = "Failed", Message = "Email or Password is incorrect" });


                }
                var twoFactorToken = await userManager.GenerateTwoFactorTokenAsync(requiredEmail, "Email");
                //      var x = HttpUtility.UrlEncode(twoFactorToken);
                //       var signIn = await signInManager.TwoFactorSignInAsync("Email", twoFactorToken, false, false);

                var message = new Message(new string[] { requiredEmail.Email }, "OTP Confirmation", twoFactorToken);
                emailService.SendEmail(message);
                return Ok(new LoginUser2FDTO { Code = twoFactorToken, Username = requiredEmail.UserName, response = new Models.Response { Status = "Success", Message = $"We have sent an OTP to your Email {requiredEmail.Email}" } });

                return StatusCode(StatusCodes.Status200OK,
                    new Models.Response { Status = "Success", Message = $"We have sent an OTP to your Email {requiredEmail.Email}" });

            }


            var expDate = DateTime.Now.AddDays(2);
            var token = await GenerateJwtToken(requiredEmail);
            var Message = "User logged in";
            return Ok(new TokenDTO
            {
                Token = token,
                Exp = expDate,
                Message = Message
            });

        }

        [HttpPost("Logintwo")]
        public async Task<IActionResult> LoginwithOTP(string username)
        {

            //    var x = HttpUtility.UrlDecode(loginUser2FDTO.Code);
            var requiredEmail = await userManager.FindByNameAsync(username);
/*            var signIn = await signInManager.TwoFactorSignInAsync("Email", User.Code, false, false);
*/
            if (requiredEmail == null)
            {
                var error = new
                {
                    ErrorMessage = "email is not found",
                };
                return Unauthorized(error);
            }
            //  var signIn = await signInManager.TwoFactorSignInAsync(requiredEmail.Email, code, false, false);
            

                var expDate = DateTime.Now.AddDays(2);
                var token = await GenerateJwtToken(requiredEmail);
                var Message = "User logged in";
                return Ok(new TokenDTO
                {
                    Token = token,
                    Exp = expDate,
                    Message = Message
                });
        
            return StatusCode(StatusCodes.Status401Unauthorized,
               new { Status = "Failed", Message = "Invalid Code or UserName please re-Login" });

        }
        
        [HttpPost("EnableTwoFactor/{userId}")]
        public async Task<IActionResult> EnableTwoFactor(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID '{userId}' not found.");
            }

            user.TwoFactorEnabled = true;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest($"Failed to enable two-factor authentication for user '{user.UserName}'.");
            }
            return StatusCode(StatusCodes.Status200OK,
              new { Status = "Success", Message = $"Two-factor authentication enabled successfully for user '{user.UserName}'." });
           /* return Ok($"Two-factor authentication enabled successfully for user '{user.UserName}'.");*/
        }
      
        [HttpPost("DisableTwoFactor/{userId}")]
        public async Task<IActionResult> DisableTwoFactor(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID '{userId}' not found.");
            }

            user.TwoFactorEnabled = false;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest($"Failed to disable two-factor authentication for user '{user.UserName}'.");
            }
           
            return StatusCode(StatusCodes.Status200OK,
                new { Status = "Success", Message = $"Two-factor authentication disabled successfully for user '{user.UserName}'." });
            /*            return Ok($"Two-factor authentication disabled successfully for user '{user.UserName}'.");
            */
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                      new { Status = "Success", Message = "Email Verified Successfully" });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                       new { Status = "Error", Message = "This User Doesnot exist!" });
        }



        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var ForgotPasswordLink = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email! }, "ForgotPasswordLink email link", ForgotPasswordLink!);
                emailService.SendEmail(message);
                var forgotPasswordDTO = new ForgotPasswordDTO
                {
                    Token = token,
                    Status="Success",
                    Message = $"Password change request is Sent to {user.Email} Successfully"

                };
                return (StatusCode(StatusCodes.Status200OK, forgotPasswordDTO));
               /* return StatusCode(StatusCodes.Status200OK,
                   new Models.Response { Status = "Success", Message = $"Password change request is Sent to {user.Email} SuccessFully" });
                */

            }
            return StatusCode(StatusCodes.Status400BadRequest,
            new Models.Response { Status = "Failed", Message = $"Couldn't send email to {user.Email}, please Try Again" });


        }

        [HttpGet("resetPassword")]
        public async Task<IActionResult> ResetPassword(string Token, string email)
        {
            var model = new ResetPassword { Token = Token, Email = email };
            return Ok(new { model.Token });
        }
        [HttpPost("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPassword)
        {
            var user = await userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                new Models.Response { Status = "Failed", Message = $"there's no user signed up with that Email" });
            }
            else if (user != null)
            {
                var resetPassResult = await userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (!resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                return StatusCode(StatusCodes.Status200OK,
                    new Models.Response { Status = "Success", Message = $"Password has been changed" });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                new Models.Response { Status = "Failed", Message = $"Couldn't send link to {user.Email}, please try again" });
        }
        
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePassword, string email)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(email);
                var result = await userManager.ChangePasswordAsync(user, changePassword.Password, changePassword.NewPassword);
                //if (result.Succeeded)
                //{
                //    ModelState.Clear();
                //}
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                    new Models.Response { Status = "Success", Message = $"Password has been changed" });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                new Models.Response { Status = "Failed", Message = $"Couldn't change Password, please try again" });
        }





       
        [HttpPost("contactUs")]
        public async Task<IActionResult> ContactUs(ContactUsDTO contactInfo)
        {
            if (ModelState.IsValid)
            {
                // Here you can handle the contact information as needed.
                // For example, you might send an email to the website administrator.
                // You can also store the contact information in a database.

                // For demonstration purposes, let's assume we send an email to the website administrator.

                // Construct the email message
                var messageBody = $"Name: {contactInfo.Name}\nEmail: {contactInfo.Email}\nMessage: {contactInfo.Message}";

                var message = new ContactUsMessage("User", new string[] { "ahmedfarag010901@gmail.com" }, "Contact Us Form Submission", messageBody);

                // Send the email
                emailService.SendEmailfromUser(message);

                return StatusCode(StatusCodes.Status200OK,
                    new Models.Response { Status = "Success", Message = "Your message has been sent. We will get back to you shortly." });
            }

            return BadRequest(ModelState);
        }
































    }
}