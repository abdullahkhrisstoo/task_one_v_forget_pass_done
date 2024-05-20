using Microsoft.EntityFrameworkCore;
using task_one_v2.Models;
using Microsoft.Extensions.Localization;
using task_one_v2.App_Core.ConstString;
using task_one_v2.ViewModel;
using System.Diagnostics;
using task_one_v2.App_Core.extension;
using Org.BouncyCastle.Crypto.Generators;
namespace task_one_v2.App_Core.StateMangement
{
    public interface IAuthManager
    {
        Task<AuthResult> LoginAsync(UserLogin userLogin);
        Task<AuthResult> RegisterAsync(UserDataRegisterViewModel userRegister);
        Task<AuthResult> UpdateProfileAsync(UpdateUserDataViewModel updateProfileViewModel);
        Task<AuthResult> UpdatePasswordAsync(EditPasswordViewModel editPasswordViewModel);
        decimal? GetCurrentCredentialUserId();
        decimal? GetCurrentUserId();
        Task LogoutAsync();
        UserCredential? GetCurrentUserCredentialData();
        UserInfo? GetCurrentUserData();
        UpdateUserDataViewModel? GetCredentialDataAndUserInfo();



        Task<UserCredential> GetUserByEmailAsync(string email);
        Task UpdatePasswordAsyncForget(decimal userId, string newPassword);
    }

    public class AuthManager : IAuthManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ModelContext _context;
        private readonly IStringLocalizer<AuthManager> _localizer;
        private readonly FileHelper.FileHelper _fileHelper;

        public AuthManager(IHttpContextAccessor httpContextAccessor, ModelContext context, IStringLocalizer<AuthManager> localizer, FileHelper.FileHelper fileHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _localizer = localizer;
            _fileHelper = fileHelper;
        }

        public decimal? GetCurrentUserId()=> _httpContextAccessor.HttpContext?.Session.GetInt32(ConstantApp.userSessionKey);
        
        public decimal? GetCurrentCredentialUserId()=> _httpContextAccessor.HttpContext?.Session.GetInt32(ConstantApp.userCredentialsSessionKey);




        public UserInfo? GetCurrentUserData()
        {
            var userId = GetCurrentUserId();
            if (userId != null)
            {
                return _context.UserInfos.AsNoTracking().SingleOrDefault(u => u.Userid == userId);
            }
            return null;
        }

        public UserCredential? GetCurrentUserCredentialData()
        {
            var userId = GetCurrentCredentialUserId();
            if (userId != null)
            {
                return _context.UserCredentials.AsNoTracking().SingleOrDefault(u => u.Id == userId);
            }
            return null;
        }



       private async Task<bool> IsEmailExist(string email) {

            var existingUser = await _context.UserCredentials.SingleOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return true;
            }
            return false;
        }



        public UpdateUserDataViewModel? GetCredentialDataAndUserInfo()
        {
           if (GetCurrentUserData() != null && GetCurrentCredentialUserId() != null)
            {
                UpdateUserDataViewModel user = new UpdateUserDataViewModel
                {
                    UserInfoId = GetCurrentUserId() ?? 0,
                    FirstName = GetCurrentUserData().FirstName,
                    LastName = GetCurrentUserData().LastName,
                    PersonalImageFile = null,
                    Image = GetCurrentUserData().Image,
                    Nationality = GetCurrentUserData().Nationality,
                    Roleid = GetCurrentUserData().Roleid,
                    VerificationStatusId = GetCurrentUserData().VerificationStatusId,
                    CertificateImage = GetCurrentUserData().CertificateImage,
                    UserCredentialId = GetCurrentCredentialUserId() ?? 0,
                    Email = GetCurrentUserCredentialData().Email,
                    Password = GetCurrentUserCredentialData().Password,
                    CertificateImageFile = null
                };
                return user;
            }
            return null;

        }



        public async Task<AuthResult> LoginAsync(UserLogin userLogin)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            try
            {
                var user = await _context.UserCredentials
                    .Include(u => u.User)
                    .SingleOrDefaultAsync(u => u.Email == userLogin.Email.ToLower().Trim() && u.Password == userLogin.Password.Encrypt());

                if (user == null)
                {
                    return new AuthResult
                    {
                        Success = false,
                        ErrorMessage = _localizer["invalid email or password"]
                    };
                }

                var userId = user.User.Userid;
                var roleId = user.User?.Roleid;

                if (roleId == null)
                {
                    return new AuthResult
                    {
                        Success = false,
                        ErrorMessage = _localizer["User information is missing."]
                    };
                }
                httpContext?.Session.SetInt32(ConstantApp.userSessionKey, (int)userId);
                httpContext?.Session.SetInt32(ConstantApp.userCredentialsSessionKey, (int)user.Id);


                switch (roleId)
                {
                    case ConstantApp.adminRole:
                        return new AuthResult
                        {
                            Success = true,
                            RedirectController = "Admin",
                            RedirectAction = "Index"
                        };
                    case ConstantApp.userRole:
                        return new AuthResult
                        {
                            Success = true,
                            RedirectController = "Home",
                            RedirectAction = "Home"
                        };
                    case ConstantApp.chefRole:
                        return new AuthResult
                        {
                            Success = true,
                            RedirectController = "Chef",
                            RedirectAction = "MyRecipe"
                        };
                    default:
                        return new AuthResult
                        {
                            Success = false,
                            ErrorMessage = _localizer["Something went wrong."]
                        };
                }
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = _localizer["Something went wrong."]
                };
            }
        }
        public async Task<AuthResult> RegisterAsync(UserDataRegisterViewModel userData)
        {
            try
            {
                if (!ValidateUserData(userData))
                {
                    return new AuthResult { Success = false, ErrorMessage = _localizer["Invalid user data"] };
                }

                var checkEmail = await IsEmailExist(userData.Email??string.Empty);

                if (checkEmail==true)
                {
                    return new AuthResult { Success = false, ErrorMessage = _localizer["Email is Already Exist"] };
                }

                var userInfo = new UserInfo
                {
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Roleid = (userData.IsChef) ? ConstantApp.chefRole : ConstantApp.userRole,
                    VerificationStatusId = (userData.IsChef ) ? ConstantApp.pendingChef : ConstantApp.approvedChef
                };


                await _context.AddAsync(userInfo);
                await _context.SaveChangesAsync();

                var credential = new UserCredential
                {
                    Email = userData.Email.ToLower().Trim(),
                    Password = userData.Password.Encrypt(),
                    Userid = userInfo.Userid
                };
                await _context.AddAsync(credential);
                await _context.SaveChangesAsync();


                var payment = new Payment
                {
                    Userid = userInfo.Userid,
                    Fullname = userInfo.FirstName + " " + userInfo.LastName,
                    Numberid = "123412341234",
                    Cvc = "000",
                    Expiredate = new DateTime(2000, 10, 15), 
                    Amount = 1500
                };
                await _context.AddAsync(payment);
                await _context.SaveChangesAsync();

                var httpContext = _httpContextAccessor.HttpContext;
                httpContext.Session.SetInt32(ConstantApp.userSessionKey, (int)userInfo.Userid);
                httpContext.Session.SetInt32(ConstantApp.userCredentialsSessionKey, (int)credential.Id);

                string redirectController = (userData.IsChef ) ? "Chef" : "Home";
                string redirectAction = (userData.IsChef ) ? "MyRecipe" : "Home";

                return new AuthResult { Success = true, RedirectController = redirectController, RedirectAction = redirectAction };
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, ErrorMessage = _localizer["Something went wrong."] };
            }
        }

        public async Task LogoutAsync()
        {
         
        }
        public async Task<AuthResult> UpdateProfileAsync(UpdateUserDataViewModel updateProfileViewModel)
        {
            var user = GetCredentialDataAndUserInfo();
            if (user == null) 
                return  new AuthResult { Success = false, ErrorMessage = _localizer["Something went wrong."] };
            
            if (!ValidateAdminProfileData(updateProfileViewModel))
                return new AuthResult { Success = false, ErrorMessage = _localizer["Invalid user data"] };
            
            if (updateProfileViewModel.Password.Encrypt() != user.Password)
                return new AuthResult { Success = false, ErrorMessage = _localizer["Enter your correct password."] };

            var checkEmail = await IsEmailExist((updateProfileViewModel.Email ?? string.Empty) );
            //if (updateProfileViewModel.Email == GetCurrentUserCredentialData()?.Email) {
            //    checkEmail = false;
            //}
            if (checkEmail == true)
            {
                return new AuthResult { Success = false, ErrorMessage = _localizer["Email is Already Exist"] };
            }

            UserInfo userInfo = new UserInfo
            {
                Userid = updateProfileViewModel.UserInfoId ?? user.UserInfoId ?? 0,
                FirstName = updateProfileViewModel.FirstName ?? user.FirstName,
                LastName = updateProfileViewModel.LastName ?? user.LastName,
                Image = updateProfileViewModel.Image ?? user.Image,
                Nationality = updateProfileViewModel.Nationality ?? user.Nationality,
                Roleid = updateProfileViewModel.Roleid ?? user.Roleid ?? 0,
                VerificationStatusId = updateProfileViewModel.VerificationStatusId ?? user.VerificationStatusId ?? 0,
                CertificateImage = updateProfileViewModel.CertificateImage ?? user.CertificateImage
            };

            if (updateProfileViewModel.PersonalImageFile != null)
            {
                userInfo.Image = await _fileHelper.UploadFileAsync(updateProfileViewModel.PersonalImageFile);
            }

            if (updateProfileViewModel.CertificateImageFile != null)
            {
                userInfo.CertificateImage = await _fileHelper.UploadFileAsync(updateProfileViewModel.CertificateImageFile);
            }

            UserCredential userCredential = new UserCredential
            {
                Email = updateProfileViewModel.Email ?? user.Email,
                Password = updateProfileViewModel.Password.Encrypt() ?? user.Password,
                Id = updateProfileViewModel.UserCredentialId ?? user.UserCredentialId ?? 0,
                Userid = updateProfileViewModel.UserInfoId ?? user.UserInfoId ?? 0
            };

            try
            {
                //if (updateProfileViewModel.Email !) { }
                _context.Update(userInfo);
                await _context.SaveChangesAsync();

                _context.Update(userCredential);
                await _context.SaveChangesAsync();

                _context.Entry(userInfo).State = EntityState.Detached;
                _context.Entry(userCredential).State = EntityState.Detached;
                return new AuthResult { Success = true };

            }
            catch (DbUpdateConcurrencyException e)
            {
                Debug.WriteLine(e);
                return new AuthResult { Success = false, ErrorMessage = _localizer["Something went wrong."] };

            }

        }



        public async Task<AuthResult> UpdatePasswordAsync(EditPasswordViewModel editPasswordViewModel)
        {
            var user = GetCredentialDataAndUserInfo();
            if (user == null)
                return new AuthResult { Success = false, ErrorMessage = _localizer["Something went wrong."] };

            if (!ValidateUpdatePassword(editPasswordViewModel))
                return new AuthResult { Success = false, ErrorMessage = _localizer["Invalid user data"] };

            if (editPasswordViewModel.LastPassword.Encrypt() !=user.Password) 
                return new AuthResult { Success = false, ErrorMessage = _localizer["Enter your correct password."] };

            if (editPasswordViewModel.NewPassword != editPasswordViewModel.ConfirmPassword)
                 return new AuthResult { Success = false, ErrorMessage = _localizer["Passwords do not match"] };
           

            UserCredential userCredential = new UserCredential
            {
                Email = editPasswordViewModel.Email ?? user.Email,
                Password = editPasswordViewModel.NewPassword.Encrypt() ?? user.Password,
                Id = editPasswordViewModel.UserCredentialId ?? user.UserCredentialId ?? 0,
                Userid = editPasswordViewModel.UserInfoId ?? user.UserInfoId ??0
            };

            try
            {

                _context.Update(userCredential);
                await _context.SaveChangesAsync();
                _context.Entry(userCredential).State = EntityState.Detached;
                return new AuthResult { Success = true };

            }
            catch (DbUpdateConcurrencyException e)
            {
                Debug.WriteLine(e);
                return new AuthResult { Success = false, ErrorMessage = _localizer["Something went wrong."] };

            }






        }
        private bool ValidateUserData(UserDataRegisterViewModel userData)
        {
            if (string.IsNullOrEmpty(userData.FirstName))
                return false;
            if (string.IsNullOrEmpty(userData.LastName))
                return false;
            if (string.IsNullOrEmpty(userData.Email))
                return false;
            if (string.IsNullOrEmpty(userData.Password))
                return false;
            return true;
        }


        private bool ValidateAdminProfileData(UpdateUserDataViewModel adminProfileData)
        {
            if (string.IsNullOrEmpty(adminProfileData.FirstName))
                return false;
            if (string.IsNullOrEmpty(adminProfileData.LastName))
                return false;
            if (string.IsNullOrEmpty(adminProfileData.Email))
                return false;
            if (string.IsNullOrEmpty(adminProfileData.Password))
                return false;
            return true;
        }


        private bool ValidateUpdatePassword(EditPasswordViewModel editPasswordViewModel)
        {

            if (string.IsNullOrEmpty(editPasswordViewModel.LastPassword))
                return false;
            if (string.IsNullOrEmpty(editPasswordViewModel.NewPassword))
                return false;
            if (string.IsNullOrEmpty(editPasswordViewModel.ConfirmPassword))
                return false;
            return true;
        }



        public async Task<UserCredential> GetUserByEmailAsync(string email)
        {
            return await _context.UserCredentials.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdatePasswordAsyncForget(decimal userId, string newPassword)
        {
            var user = await _context.UserCredentials.FindAsync(userId);
            if (user != null)
            {
                user.Password = newPassword.Encrypt(); 
                await _context.SaveChangesAsync();
            }
        }

       
    }










    public class AuthResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string RedirectController { get; set; }
        public string RedirectAction { get; set; }
    }

}
