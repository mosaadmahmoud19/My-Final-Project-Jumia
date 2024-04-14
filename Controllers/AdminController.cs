using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using My_Final_Project.DTO;
using My_Final_Project.Models;
using My_Final_Project.Reposatory;
using My_Final_Project.Reposatory.InterfacesReposatory;
using System.Security.Claims;
using Response = My_Final_Project.Models.Response;

namespace My_Final_Project.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderRepo orderRepo;

        public AdminController(UserManager<ApplicationUser> userManager,IOrderRepo orderRepo)
        {
            this.userManager = userManager;
            this.orderRepo = orderRepo;
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = userManager.Users
       .Select(u => new UsersDTO
       {
           UserId = u.Id,
           UserName = u.UserName,
           Name = u.Name,
           Role = u.Role,
           Email = u.Email,
           EmailConfirmed = u.EmailConfirmed,
           Age = u.Age,
           Nationality = u.Nationality,
           PhoneNumber = u.Phone
           // Add more properties as needed
       })
       .ToList();

            return Ok(users);
        }

        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new Response { Status = "Failed", Message = "User Not Found" });
            }

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(new Response { Status = "Success", Message = "User deleted successfully" });
            }
            else
            {
                return BadRequest(new Response { Status = "Failed", Message = "Failed to delete user" });
            }

        }

        [HttpGet("getAllOrders")]
        public IActionResult getAllOrders()
        {
            var Orders = orderRepo.GetAllOrders();
            List<OrderDTO2> orders = new List<OrderDTO2>();
            foreach (var item in Orders)
            {
                OrderDTO2 orderDTO = new OrderDTO2()
                {
                    
                    OrderId = item.OrderId,
                    OrderDate = item.OrderDate,
                    TotalAmount = item.TotalAmount,
                    Email = item.ApplicationUser.Email,
                    Name = item.ApplicationUser.Name,
                };
                orders.Add(orderDTO);
            }
            return Ok(orders);
        }

        [HttpPost]
        [Route("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(string userId, string newRole)
        {
            // Find the user by their ID
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID '{userId}' not found.");
            }

            // Update the user's role attribute
            user.Role = newRole; // Assuming 'Role' is a property of ApplicationUser

            // Save the changes
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest($"Failed to update user role: {result.Errors}");
            }
            UpdateUserClaims(user.Id);
            return Ok($"User '{user.UserName}' role updated to '{newRole}'.");
        }
        [HttpPost]
        [Route("UpdateUserClaims")]
        public async Task<IActionResult> UpdateUserClaims(string userId)
        {
            // Find the user by their ID
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID '{userId}' not found.");
            }

            // Remove all existing claims for the user
            var existingClaims = await userManager.GetClaimsAsync(user);
            foreach (var claim in existingClaims)
            {
                var result = await userManager.RemoveClaimAsync(user, claim);
                if (!result.Succeeded)
                {
                    return BadRequest($"Failed to remove claim: {claim.Type}");
                }
            }

            // Add new claims for the user
            var claims = new List<Claim>
        {
             new Claim("UserId",user.Id),
            new Claim("role",user.Role),
            new Claim("Email", user.Email),
            new Claim("Name", user.Name),
            new Claim("Age",user.Age.ToString() ),
            new Claim("Nationality",user.Nationality),
            new Claim("Phone",user.Phone)
        };

            var addClaimsResult = await userManager.AddClaimsAsync(user, claims);
            if (!addClaimsResult.Succeeded)
            {
                return BadRequest($"Failed to add new claims: {string.Join(", ", addClaimsResult.Errors.Select(e => e.Description))}");
            }

            return Ok("User claims updated successfully.");
        }
    }
}


    

