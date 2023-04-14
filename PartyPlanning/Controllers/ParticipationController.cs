using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PartyPlanning.Data;

namespace PartyPlanning.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
public class ParticipationController : ControllerBase
{
    private readonly UserManager<PartyUser> _userManager;

    private readonly PartyContext _context;

    public ParticipationController(UserManager<PartyUser> userManager, PartyContext context)
    {
        _userManager = userManager;
        _context = context;
    }
}