using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Consts;
using FreelancerPlatform.Application.Dtos.Account;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.Login;
using FreelancerPlatform.Application.Dtos.SystemManagement;
using FreelancerPlatform.Application.Helpers;
using FreelancerPlatform.Application.ServiceImplementions.Base;
using FreelancerPlatform.Domain.Entity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.ServiceImplementions
{
    public class SystemManagementService : BaseService<SystemManagement>, ISystemManagementService
    {
        private readonly ISystemManagementRepository _systemManagementRepository;
        public SystemManagementService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SystemManagement> logger,  ISystemManagementRepository systemManagementRepository, ISystemManagementRoleRepository systemManagementRoleRepository, IPermissionRepository permissionRepository) : base(unitOfWork, mapper, logger)
        {
            _systemManagementRepository = systemManagementRepository;
        }

        public async Task<ServiceResult> RegisterAccountAsync(SystemManagementRegisterRequest request)
        {
            try
            {
                var entities = await _systemManagementRepository.GetAllAsync();
                var freelancerExis = entities.FirstOrDefault(x => x.UserName.Equals(request.UserName));
                if (freelancerExis != null)
                {
                    return new ServiceResult()
                    {
                        Status = StatusResult.ClientError,
                        Message = "User name đã tồn tại, vui lòng chọn email khác"
                    };
                }
                var account = new SystemManagement();
                var passwordHasher = PasswordHelper.HashPassword(request.Password);
                account.UserName = request.UserName;
                account.Password = passwordHasher;
                if (request.PhoneNumber != null)
                {
                    account.PhoneNumber = request.PhoneNumber;
                }
                await _systemManagementRepository.CreateAsync(account);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Đăng ký thành công",
                    Status = StatusResult.Success
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<LoginResult> LoginAccountAsync(SystemManagementLoginRequest request)
        {
            try
            {
                var entities = await _systemManagementRepository.GetAllAsync();
                var freelancer = entities.FirstOrDefault(x => x.UserName.Equals(request.UserName));
                if (freelancer == null)
                {
                    return new LoginResult()
                    {
                        Message = "Không tìm thấy thônng tin email",
                        Status = StatusResult.ClientError
                    };
                }
                if (freelancer.Status)
                {
                    return new LoginResult()
                    {
                        Message = "Tài khoản đã bị khóa",
                        Status = StatusResult.ClientError
                    };
                }
                var verifyPassword = PasswordHelper.VerifyPassword(freelancer.Password, request.Password);
                if (!verifyPassword)
                {
                    return new LoginResult()
                    {
                        Message = "Sai mật khẩu",
                        Status = StatusResult.ClientError
                    };
                }

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,freelancer.Id.ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourStrongSecretKey1234567890123456"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                        "khanhhuynh.paptech",
                        "khanhhuynh.paptech",
                        claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: creds
                    );
                return new LoginResult()
                {
                    Status = StatusResult.Success,
                    Message = "Đăng nhập thành công",
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);

                return new LoginResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<List<SystemManagementQuickViewModel>> GetAllAdmin()
        {
            var admins = await _systemManagementRepository.GetAllAsync();
            return admins.Select(x => new SystemManagementQuickViewModel()
            {
                CreateDay = x.CreateDay,
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                UserName = x.UserName,
                Status = x.Status
            } ).ToList();
        }

        public async Task<ServiceResult> LockAcountAsync(int adminId)
        {
            try
            {
                var entities = await _systemManagementRepository.GetByIdAsync(adminId);
                if (entities == null)
                {
                    return new ServiceResult()
                    {
                        Status = StatusResult.ClientError,
                        Message = "Không tìm thấy thông tin"
                    };
                }
                entities.Status = true;
                _systemManagementRepository.Update(entities);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Cập nhật thành công",
                    Status = StatusResult.Success
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<ServiceResult> UnLockAcountAsync(int adminId)
        {
            try
            {
                var entities = await _systemManagementRepository.GetByIdAsync(adminId);
                if (entities == null)
                {
                    return new ServiceResult()
                    {
                        Status = StatusResult.ClientError,
                        Message = "Không tìm thấy thông tin"
                    };
                }
                entities.Status = false;
                _systemManagementRepository.Update(entities);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Cập nhật thành công",
                    Status = StatusResult.Success
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }
        /*public async Task<ServiceResult> CreateSystemManagementAsync(SystemManagementCreateRequest request)
{
try
{
var entity = _mapper.Map<SystemManagement>(request);

await _systemManagementRepository.CreateAsync(entity);
await _unitOfWork.SaveChangeAsync();

return new ServiceResult()
{
Message = "Tạo thông tin thành công",
Status = StatusResult.Success
};
}
catch (Exception ex)
{
_unitOfWork.Cancel();
_logger.LogError(ex.InnerException.Message);

return new ServiceResult()
{
Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
Status = StatusResult.SystemError
};
}
}

public async Task<ServiceResult> DeleteAccountAsync(int id)
{
try
{
SystemManagement entity = await _systemManagementRepository.GetByIdAsync(id);
if (entity == null)
{
return new ServiceResult()
{
Message = "Không tìm thấy thônng tin",
Status = StatusResult.ClientError
};
}
entity.IsDeleted = true;
entity.CreateUpdate = DateTime.Now;
_systemManagementRepository.Update(entity);
var systemManagementRoleOfId = (await _systemManagementRoleRepository.GetAllAsync()).Where(x => x.SystemManagementId == id && x.IsDeleted == false);
foreach(var item in systemManagementRoleOfId)
{
item.IsDeleted = true;
item.CreateUpdate = DateTime.Now;
_systemManagementRoleRepository.Update(item);
}
await _unitOfWork.SaveChangeAsync();

return new ServiceResult()
{
Message = "Xoá tin thành công",
Status = StatusResult.Success
};
}
catch (Exception ex)
{
_unitOfWork.Cancel();
_logger.LogError(ex.InnerException.Message);

return new ServiceResult()
{
Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
Status = StatusResult.SystemError
};
}
}

public async Task<Pagination<SystemManagementQuickViewModel>> GetAllSystemManagementAsync(int pageIndex, int pageTake, string? keywork = null)
{
var query = (await _systemManagementRepository.GetAllAsync()).Where(x => !x.IsDeleted);
if (keywork != null)
{
query = query.Where(x => x.UserName.Equals(keywork));
}
var pagination = new Pagination<SystemManagementQuickViewModel>();
pagination.PageIndex = pageIndex;
pagination.PageSize = pageTake;
pagination.TotalRecords = query.Count();

var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<SystemManagementQuickViewModel>(x)).ToList();
pagination.Items = items;

return pagination;
}

public async Task<SystemManagementViewModel> GetSystemManagementAsync(int id)
{
SystemManagement entity = await _systemManagementRepository.GetByIdAsync(id);
return entity.IsDeleted == false ? _mapper.Map<SystemManagementViewModel>(entity) : new SystemManagementViewModel();
}

public async Task<ServiceResult> LockAccountAsync(int id)
{
try
{
SystemManagement entity = await _systemManagementRepository.GetByIdAsync(id);
if (entity == null)
{
return new ServiceResult()
{
Message = "Không tìm thấy thônng tin",
Status = StatusResult.ClientError
};
}
entity.Status = !entity.Status;
entity.CreateUpdate = DateTime.Now;
_systemManagementRepository.Update(entity);
await _unitOfWork.SaveChangeAsync();

return new ServiceResult()
{
Message = "Khóa tài khoản thành công",
Status = StatusResult.Success
};
}
catch (Exception ex)
{
_unitOfWork.Cancel();
_logger.LogError(ex.InnerException.Message);

return new ServiceResult()
{
Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
Status = StatusResult.SystemError
};
}
}

public async Task<LoginResult> LoginAsync(LoginRequest request)
{
try
{
var entities = await _systemManagementRepository.GetAllAsync();
var systemManagement = entities.FirstOrDefault(x => x.UserName.Equals(request.UserName));
if (systemManagement == null)
{
return new LoginResult()
{
Message = "Không tìm thấy thônng tin username",
Status = StatusResult.ClientError
};
}
var verifyPassword = PasswordHelper.VerifyPassword(systemManagement.Password, request.Password);
if (!verifyPassword)
{
return new LoginResult()
{
Message = "Sai mật khẩu",
Status = StatusResult.ClientError
};
}

var roles = (await _systemManagementRoleRepository.GetAllAsync()).Where(x => x.SystemManagementId == systemManagement.Id && x.IsDeleted == false).Select(x => x.RoleId);
var permission = (await _permissionRepository.GetAllAsync()).Where(x => roles.Contains(x.Id)).Select(x => x.Name).ToList();


var claims = new List<Claim>()
{
new Claim(ClaimTypes.NameIdentifier,systemManagement.Id.ToString()),
new Claim(ClaimTypes.Name,systemManagement.UserName),
new Claim(ClaimTypes.Role, Const.SystemManagement),
new Claim(Const.Permission, string.Join(";",permission ))
};
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourStrongSecretKey1234567890123456"));
var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
var token = new JwtSecurityToken(
"khanhhuynh.paptech",
"khanhhuynh.paptech",
claims,
expires: DateTime.Now.AddMinutes(60),
signingCredentials: creds
);
return new LoginResult()
{
Status = StatusResult.Success,
Message = "Đăng nhập thành công",
Token = new JwtSecurityTokenHandler().WriteToken(token)
};
}
catch (Exception ex)
{
_logger.LogError(ex.InnerException.Message);

return new LoginResult()
{
Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
Status = StatusResult.SystemError
};
}
}

public async Task<ServiceResult> RegisterAccountAsync(SystemManagementRegisterRequest request)
{
try
{
var account = new SystemManagement();
var passwordHasher = PasswordHelper.HashPassword(request.Password);
account.UserName = request.UserName;
account.Password = passwordHasher;
account.SystemManagementRoles = new List<SystemManagementRole>();
foreach (var roleId in request.RoleIds)
{
account.SystemManagementRoles.Add(new SystemManagementRole() { RoleId = roleId});
}
await _systemManagementRepository.CreateAsync(account);
await _unitOfWork.SaveChangeAsync();





return new ServiceResult()
{
Message = "Xoá tin thành công",
Status = StatusResult.Success
};
}
catch (Exception ex)
{
_unitOfWork.Cancel();
_logger.LogError(ex.InnerException.Message);

return new ServiceResult()
{
Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
Status = StatusResult.SystemError
};
}
}

public async Task<ServiceResult> UpdatePasswordAsync(int id, PasswordUpdateRequest request)
{
try
{
SystemManagement entity = await _systemManagementRepository.GetByIdAsync(id);
if (entity == null)
{
return new ServiceResult()
{
Message = "Không tìm thấy thônng tin",
Status = StatusResult.ClientError
};
}
var verify = PasswordHelper.VerifyPassword(entity.Password, request.CurrentPassword);
if (!verify)
{
return new ServiceResult()
{
Message = "Sai mật khẩu hiện tại",
Status = StatusResult.ClientError
};
}
if (!request.NewPassword.Equals(request.ConfirmPassword))
{
return new ServiceResult()
{
Message = "Sai mật khẩu xác nhận",
Status = StatusResult.ClientError
};
}
var passwordHasher = PasswordHelper.HashPassword(request.NewPassword);
entity.Password = passwordHasher;
entity.CreateUpdate = DateTime.Now;
_systemManagementRepository.Update(entity);
await _unitOfWork.SaveChangeAsync();

return new ServiceResult()
{
Message = "Cập nhật mật khẩu thành công",
Status = StatusResult.Success
};
}
catch (Exception ex)
{
_unitOfWork.Cancel();
_logger.LogError(ex.InnerException.Message);

return new ServiceResult()
{
Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
Status = StatusResult.SystemError
};
}
}

public async Task<ServiceResult> UpdateSystemManagementAsync(int id, SystemManagementUpdateRequest request)
{
try
{
SystemManagement entity = await _systemManagementRepository.GetByIdAsync(id);
if (entity == null)
{
return new ServiceResult()
{
Message = "Không tìm thấy thônng tin",
Status = StatusResult.ClientError
};
}
var updateEntity = _mapper.Map<SystemManagement>(request);
entity.CreateUpdate = DateTime.Now;
_systemManagementRepository.Update(updateEntity);

await _unitOfWork.SaveChangeAsync();

return new ServiceResult()
{
Message = "cập nhật thành công",
Status = StatusResult.Success
};
}
catch (Exception ex)
{
_unitOfWork.Cancel();
_logger.LogError(ex.InnerException.Message);

return new ServiceResult()
{
Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
Status = StatusResult.SystemError
};
}
}

public async Task<ServiceResult> UpdateSystemManagementRoleAsync(int id, SystemManagementRoleUpdateRequest request)
{
try
{
var entities = (await _systemManagementRoleRepository.GetAllAsync()).Where(x => x.SystemManagementId == id && x.IsDeleted == false);
if (entities.Count() == 0)
{
return new ServiceResult()
{
Message = "Không tìm thấy thônng tin",
Status = StatusResult.ClientError
};
}
foreach(var entity in entities)
{
entity.IsDeleted = true;
entity.CreateUpdate = DateTime.Now;
_systemManagementRoleRepository.Update(entity);
}
foreach(var roleId in request.RoleIds)
{
await _systemManagementRoleRepository.CreateAsync(new SystemManagementRole()
{
RoleId = roleId,
SystemManagementId = id
});
}

await _unitOfWork.SaveChangeAsync();

return new ServiceResult()
{
Message = "cập nhật thành công",
Status = StatusResult.Success
};
}
catch (Exception ex)
{
_unitOfWork.Cancel();
_logger.LogError(ex.InnerException.Message);

return new ServiceResult()
{
Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
Status = StatusResult.SystemError
};
}
}*/
    }
}
