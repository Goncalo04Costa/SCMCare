using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Modelos;
using WebApplication1.DTOs;
using WebApplication1.Excecoes;
using WebApplication1.Modelos;

namespace WebApplication1.Servicos
{
    public class AuthService
    {
        private readonly UserManager<UserFuncionario> _userManager;
        private readonly JwtService _jwtService;

        public AuthService(UserManager<UserFuncionario> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<string> RegisterAsync(UserRegistrationDTO registrationDTO)
        {
            // Create a new UserFuncionario object with the provided registration information
            var newUser = new UserFuncionario
            {
                Nome = registrationDTO.UserName,
                Email = registrationDTO.Email
            };

            try
            {
                // Attempt to create the new user account
                var result = await _userManager.CreateAsync(newUser, registrationDTO.Password);

                // Check if the user account was created successfully
                if (result.Succeeded)
                {
                    // If the user account was created successfully, generate a JWT token for the new user and return it
                    return _jwtService.CreateToken(newUser);
                }
                else
                {
                    // If there were errors during user creation, throw an exception or handle them accordingly
                    throw new Exception("Failed to create user account. See IdentityResult errors for details.");
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs during user registration, handle it appropriately
                throw new Exception("An error occurred while registering the user.", ex);
            }
        }
        public async Task<string> LoginAsync(string email, string password)
        {
            
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
               
                return null;
            }

           
            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
            {
                
                return null;
            }

           
            return _jwtService.CreateToken(user);
        }
    }
}
