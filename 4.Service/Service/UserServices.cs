using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data.Repositories;
using Data.Repositories.Base;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Base;
using Service.Interfaces;

namespace Service;

public class UserServices : BaseServices<IdentityUser<long>>, IUserServices
{
    private readonly IUserRepository _repository;
    private readonly UserManager<IdentityUser<long>> _userManager;
    private readonly SignInManager<IdentityUser<long>> _signInManager;
    private readonly IConfiguration _configuration;

    public UserServices(IUserRepository repository, UserManager<IdentityUser<long>> userManager,
        IUnitOfWork unitOfWork, SignInManager<IdentityUser<long>> signInManager, IConfiguration configuration) :
        base(unitOfWork)
    {
        _repository = repository;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<bool> CreateAccount(IdentityUser<long> user, string pass)
    {
        var result = await _userManager.CreateAsync(user, pass);

        if (result.Succeeded)
            return true;

        return false;
    }

    public async Task DeleteAccount(IdentityUser<long> user)
    {
        _repository.Delete(user);

        await _unitOfWork.CommitChangesAsync();
    }

    public async Task<string> Login(string userName, string pass)
    {
        var user = await _repository.Read(user => user.UserName == userName);

        //ToDo: Use Custom Exceptions
        //TODo: Create Exception Handler middleware to catch these
        if (user is null)
            throw new ArgumentNullException();

        //ToDo: Use Custom Exceptions
        if (!(await _signInManager.CheckPasswordSignInAsync(user, pass, false)).Succeeded)
        {
            throw new Exception();
        }

        //ToDo: replace _configuration with custom appsettings class

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:Key"]));
        var keyX = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:KeyX"]));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var encryptingCredentials = new EncryptingCredentials(keyX,SecurityAlgorithms.Aes128KW,SecurityAlgorithms.Aes256CbcHmacSha512);
        
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("SecurityStamp", user.SecurityStamp)
        };
        var jwtHandler = new JwtSecurityTokenHandler();
        
        var tokenOptions = new SecurityTokenDescriptor(){
            Issuer= _configuration["JwtOptions:ValidIssuer"],
            Audience= _configuration["JwtOptions:ValidAudience"],
            Expires = DateTime.Now.AddMonths(1),
            SigningCredentials= signingCredentials,
            EncryptingCredentials = encryptingCredentials,
            //ToDo: should we use Subject for claims or use Claims instead
            Subject = new ClaimsIdentity(claims)
        };

        var token = jwtHandler.CreateJwtSecurityToken(tokenOptions);
        return jwtHandler.WriteToken(token);
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IdentityUser<long>> FindUser(long id)
    {
        return await _repository.Read(x => x.Id == id);
    }
}