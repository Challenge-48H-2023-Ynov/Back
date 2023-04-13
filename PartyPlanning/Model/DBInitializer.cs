using Microsoft.AspNetCore.Identity;
using PartyPlanning.Data;

namespace PartyPlanning.Model;

public class DBInitializer
{
    private readonly PartyContext _context;
    private readonly UserManager<PartyUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public DBInitializer(PartyContext context, UserManager<PartyUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<bool> Initialize()
    {
        _context.Database.EnsureCreated();

        if (_context.Roles.Any() || _context.Users.Any()) return false;

        //Adding roles
        var roles = Roles.GetAllRoles();

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var resultAddRole = await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                if (!resultAddRole.Succeeded)
                    throw new ApplicationException("Adding role '" + role + "' failed with error(s): " + resultAddRole.Errors);
            }
        }

        //Adding Admin
        PartyUser admin = new PartyUser
        {
            FirstName = "Antoine",
            LastName = "Capitain",
            UserName = "Dercraker",
            Email = "antoine.capitain@gmail.com",
            PhoneNumber = "1234567890",
            DateOfBrith = DateTime.Now,
            Biography = "Hello :)",
            HavePermis = true,
            EmailConfirmed = true,
        };

        string pwd = "NMdRx$HqyT8jX6";

        IdentityResult? resultAddUser = await _userManager.CreateAsync(admin, pwd);
        if (!resultAddUser.Succeeded)
            throw new ApplicationException("Adding user '" + admin.UserName + "' failed with error(s): " + resultAddUser.Errors);

        var resultAddRoleToUser = await _userManager.AddToRoleAsync(admin, Roles.Admin);
        if (!resultAddRoleToUser.Succeeded)
            throw new ApplicationException("Adding user '" + admin.UserName + "' to role '" + Roles.Admin + "' failed with error(s): " + resultAddRoleToUser.Errors);

        await _context.SaveChangesAsync();

        return true;
    }
}