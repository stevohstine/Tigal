using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Tigal.Server.Data;
using Tigal.Server.Models;
using Tigal.Server.Services;
using Tigal.Server.Utils;
using Tigal.Shared.DTOs.Models;
using Tigal.Shared.Models;
using Tigal.Shared.Models.DTOs;

namespace Tigal.Server.Data
{
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ManagementDBContext _context;
        private readonly IJWTGenerator _jwtGenerator;
        private readonly IUserAccessor _userAccessor;
        private ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger, IJWTGenerator jwtGenerator, ManagementDBContext context, IUserAccessor userAccessor)
        {
            _jwtGenerator = jwtGenerator;
            _context = context;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        [HttpGet("/api/v1/users/get/all")]
        public async Task<List<Users>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("/api/v1/users/get/{id}")]
        public async Task<Users> GetUsers(int id)
        {
            if (_context.Users == null)
            {
                return new Users();
            }
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return new Users();
            }

            return users;
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/users/register")]
        public async Task<GenericResponseModel> PostUsers(RegisterDTO registerDTO)
        {
            if (registerDTO != null)
            {
                var checkIfUserExists = await _context.Users.FirstOrDefaultAsync(i => i.Phone == registerDTO.Phone);
                if (checkIfUserExists == null)
                {
                    if (registerDTO.Phone.Length != 10)
                    {
                        return new GenericResponseModel()
                        {
                            Message = "Invalid phone number.",
                            StatusCode = "1"
                        };
                    }

                    var user = new Users();
                    user.Phone = registerDTO.Phone;
                    user.Username = registerDTO.Username;
                    user.OtpCode = Utils.UtilsHelper.GenerateNewCode(5);
                    user.ProfileImage = "";
                    user.Location = "";
                    user.Latitude = "";
                    user.Longitude = "";
                    user.PolicyStatus = false;
                    if (registerDTO.Password != null)
                    {
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);
                        user.Password = hashedPassword;
                    }

                    user.CreatedAt = DateTime.Now;
                    user.UpdatedAt = DateTime.Now;
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    //return CreatedAtAction("GetUsers", new { id = user.Id }, user);
                    return new GenericResponseModel()
                    {
                        Message = "Registration success.",
                        StatusCode = "0"
                    };
                }
                else
                {
                    return new GenericResponseModel()
                    {
                        Message = "Phone number is already registered.",
                        StatusCode = "1"
                    };
                }

            }
            else
            {
                return new GenericResponseModel()
                {
                    Message = "Registration failed.",
                    StatusCode = "1"
                };
            }
        }

        [HttpDelete("/api/v1/users/delete/{id}")]
        public async Task<Users> DeleteUsers(int id)
        {
            if (_context.Users == null)
            {
                return new Users();
            }
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return new Users();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return new Users();
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/users/login")]
        public async Task<RegistrationLoginReponse> LoginUser(LoginDTO _loginDTO)
        {
            if (_loginDTO != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(i => i.Phone == _loginDTO.Phone);
                if (user == null)
                {
                    return new RegistrationLoginReponse()
                    {
                        Message = "User not found.",
                        StatusCode = "1",
                        Token = "none",
                        PhoneNumber = _loginDTO.Phone,
                        Username = "none",
                        JoinedOn = "none"
                    };
                }
                else
                {
                    //check if password is correct
                    bool passVerified = BCrypt.Net.BCrypt.Verify(_loginDTO.Password, user.Password);

                    if (passVerified)
                    {
                        return new RegistrationLoginReponse()
                        {
                            Message = "Login success.",
                            StatusCode = "0",
                            Token = _jwtGenerator.GetToken(user),
                            PhoneNumber = _loginDTO.Phone,
                            Username = user.Username,
                            JoinedOn = user.CreatedAt.ToString()
                        };
                    }
                    else
                    {
                        return new RegistrationLoginReponse()
                        {
                            Message = "Incorrect password.",
                            StatusCode = "1",
                            Token = "none",
                            PhoneNumber = _loginDTO.Phone,
                            Username = "none",
                            JoinedOn = "none"
                        };
                    }


                }
            }
            else
            {
                return new RegistrationLoginReponse()
                {
                    Message = "User not found.",
                    StatusCode = "1",
                    Token = "none",
                    PhoneNumber = _loginDTO.Phone,
                    Username = "none",
                    JoinedOn = "none"
                };
            }
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/users/request/password/reset")]
        public async Task<GenericResponseModel> RequestResetPassword(string phoneNumber)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.Phone == phoneNumber);
            if (user == null)
            {
                return new GenericResponseModel()
                {
                    Message = "User not found.",
                    StatusCode = "1"
                };
            }
            else
            {
                //send otp
                var otp = Utils.UtilsHelper.GenerateNewCode(5);
                //update db otp
                user.OtpCode = otp;
                await _context.SaveChangesAsync();
                //Utils.UtilsHelper.SendSms(new SmsRequestBody(){ "","","","",""});
                return new GenericResponseModel()
                {
                    Message = "Otp code sent successfully.",
                    StatusCode = "0"
                };
            }
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/users/reset/password")]
        public async Task<GenericResponseModel> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (resetPasswordModel != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(i => i.Phone == resetPasswordModel.phoneNumber);
                if (user == null)
                {
                    return new GenericResponseModel()
                    {
                        Message = "User not found.",
                        StatusCode = "1"
                    };
                }
                else
                {
                    if (resetPasswordModel.otpCode == user.OtpCode)
                    {
                        user.OtpCode = Utils.UtilsHelper.GenerateNewCode(5);
                        user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordModel.password);
                        await _context.SaveChangesAsync();

                        return new GenericResponseModel()
                        {
                            Message = "Password reset success.",
                            StatusCode = "0"
                        };
                    }
                    else
                    {
                        return new GenericResponseModel()
                        {
                            Message = "Invalid otp code.",
                            StatusCode = "1"
                        };
                    }
                }
            }
            else
            {
                return new GenericResponseModel()
                {
                    Message = "Invalid model.",
                    StatusCode = "1"
                };
            }
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/users/change/password")]
        public async Task<GenericResponseModel> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (changePasswordModel != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(i => i.Phone == changePasswordModel.phoneNumber);
                if (user == null)
                {
                    return new GenericResponseModel()
                    {
                        Message = "User not found.",
                        StatusCode = "1"
                    };
                }
                else
                {
                    bool passVerified = BCrypt.Net.BCrypt.Verify(changePasswordModel.oldPassword, user.Password);
                    if (passVerified)
                    {
                        user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordModel.newPassword);
                        await _context.SaveChangesAsync();
                        return new GenericResponseModel()
                        {
                            Message = "Password change success.",
                            StatusCode = "0"
                        };
                    }
                    else
                    {
                        return new GenericResponseModel()
                        {
                            Message = "Invalid old password.",
                            StatusCode = "1"
                        };
                    }

                }
            }
            else
            {
                return new GenericResponseModel()
                {
                    Message = "Invalid credentials.",
                    StatusCode = "1"
                };
            }
        }

        [HttpPost("/api/v1/users/verifyOtp")]
        public async Task<ActionResult<GenericResponseModel>> VerifyOTP([FromBody] VerifyOTPRequestBody verifyOTPRequestBody)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(i => i.Phone == verifyOTPRequestBody.Phone);
                if (user == null)
                {
                    return new GenericResponseModel()
                    {
                        Message = "User not found.",
                        StatusCode = "1"
                    };
                }
                else
                {
                    if (user.OtpCode == verifyOTPRequestBody.Otp)
                    {
                        //Update otp in the db
                        user.OtpCode = UtilsHelper.GenerateNewCode(5);
                        await _context.SaveChangesAsync();

                        return Ok(new GenericResponseModel()
                        {
                            Message = "OTP verification success",
                            StatusCode = "0"
                        });
                    }
                    else
                    {
                        return Ok(new GenericResponseModel()
                        {
                            Message = "OTP verification failed",
                            StatusCode = "1"
                        });
                    }
                }
            }catch(Exception e)
            {
                return Ok(new GenericResponseModel()
                {
                    Message = "Failed to verify otp",
                    StatusCode = "1"
                });

            }


        }

        [HttpPost("/api/v1/users/privacyPolicy")]
        public async Task<ActionResult<GenericResponseModel>> UpdatePrivacyPolicy([FromBody] PrivacyPolicyDataModel privacyPolicyDataModel)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(i => i.Phone == privacyPolicyDataModel.phone);
                if (user == null)
                {
                    return new GenericResponseModel()
                    {
                        Message = "User not found.",
                        StatusCode = "1"
                    };
                }
                else
                {
                    user.PolicyStatus = true;
                    await _context.SaveChangesAsync();

                    return Ok(new GenericResponseModel()
                    {
                        Message = "Privacy update success",
                        StatusCode = "0"
                    });
                   
                }
            }
            catch (Exception e)
            {
                return Ok(new GenericResponseModel()
                {
                    Message = "Failed to update privacy policy",
                    StatusCode = "1"
                });

            }
        }

        [HttpPost("/api/v1/users/update/profile/picture")]
        public async Task<ActionResult<ProfilePictureResponseModel>> UpdateProfilePicture([FromForm] UpdateProfilePhoto formFile)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.Phone == formFile.phoneNumber);
            if (user == null)
            {
                return Ok(new ProfilePictureResponseModel(){
                    StatusCode = "1",
                    Message = "User not found"
                });
            }

            if (formFile.profilePicture.FileName == null || formFile.profilePicture.Length == 0)
            {
                return Ok(new ProfilePictureResponseModel(){
                    StatusCode = "1",
                    Message = "No Image Selected"
                });
            }
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.profilePicture.FileName;
            var filePath = Path.Combine( Directory.GetCurrentDirectory(), "Documents/profiles", $"{uniqueFileName}");
            //var path = Path.Combine(_environment.WebRootPath, "Images/", item.FileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.profilePicture.CopyToAsync(stream);
                stream.Close();
            }

            var fileUrl = $"https://pilot.atomiot.co.ke/Documents/profiles/{uniqueFileName}";
            user.ProfileImage = fileUrl;
            await _context.SaveChangesAsync();

            return Ok(new ProfilePictureResponseModel(){
                StatusCode = "0",
                Message = "Update success",
                profileImageUrl = fileUrl
            });
        }
        private bool UsersExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
