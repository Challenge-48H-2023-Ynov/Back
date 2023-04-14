using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyPlanning.Data;
using PartyPlanning.Model.InvitationModels;
using PartyPlanning.Model.PartyModels;

namespace PartyPlanning.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
public class InvitationController : ControllerBase
{

    private readonly UserManager<PartyUser> _userManager;
    private readonly PartyContext _context;

    public InvitationController(UserManager<PartyUser> userManager, PartyContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<InvitationDTO>>> GetInvitations()
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        List<Invitation> invitations = await _context.Invitation.Include(i => i.User).Include(i => i.Party).ToListAsync();
        return invitations.ToDTOList();
    }

    [HttpGet]
    [Route("party/{IdParty}")]
    public async Task<ActionResult<List<InvitationDTO>>> GetInvitationsByIdParty([FromRoute] Guid IdParty)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        List<Invitation>? invitations = await _context.Invitation.Include(i => i.User).Include(i => i.Party).Where(i => i.IdParty == IdParty).ToListAsync();
        if (invitations == null) return NotFound("No Invitations found with this PartyId");

        return invitations.ToDTOList();
    }

    [HttpGet]
    [Route("user/{IdUser}")]
    public async Task<ActionResult<List<InvitationDTO>>> GetInvitationsByIdUser([FromRoute] Guid IdUser)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        List<Invitation>? invitations = await _context.Invitation.Include(i => i.User).Include(i => i.Party).Where(i => i.IdUser == IdUser).ToListAsync();
        if (invitations == null) return NotFound("No Invitations found with this IdUser");

        return invitations.ToDTOList();
    }


    [HttpPost]
    [Route("new/{IdParty}/{IdUser}")]
    public async Task<IActionResult> CreateInvitation([FromRoute] Guid IdParty, Guid IdUser, [FromBody] MakeInvitationDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (IdParty != dto.IdParty) return NotFound("L'IdParty est != de celui du dto");
        if (IdUser != dto.IdUser) return NotFound("L'IdUser est != de celui du dto");

        PartyUser? user = await _context.PartyUsers.Include(u => u.Invitations).FirstOrDefaultAsync(u => u.Id == IdUser);
        if (user == null) return BadRequest("User not found");

        Party? party = await _context.Party.Include(p => p.Invitations).Include(p => p.Participations).FirstOrDefaultAsync(p => p.IdParty == IdParty);
        if (party == null) return BadRequest("Party not found");

        if (party.IdUser == IdUser) return BadRequest("User is the owner of the party");
        if (party.Participations!.Any(p => p.IdUser == IdUser)) return BadRequest("User already participating to this party");
        if (user.Invitations!.Any(i => i.IdParty == IdParty)) return BadRequest("User already invited to this party");
        if (party.Invitations!.Any(i => i.IdUser == IdUser)) return BadRequest("User already invited to this party");

        Invitation invitation = new()
        {
            IdParty = IdParty,
            IdUser = IdUser,
            Party = party,
            User = user
        };

        try
        {
            _context.Invitation.Add(invitation);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }


        return Ok($"New invitation created for: {user.UserName}, in: {party.Name}");
    }


    [HttpDelete]
    [Route("{IdParty}/{IdUser}")]
    public async Task<IActionResult> GetParties([FromRoute] Guid IdParty, Guid IdUser)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        PartyUser? user = await _context.PartyUsers.Include(u => u.Invitations).FirstOrDefaultAsync(u => u.Id == IdUser);
        if (user == null) return NotFound("User not found");

        Party? party = await _context.Party.Include(p => p.Invitations).FirstOrDefaultAsync(p => p.IdParty == IdParty);
        if (party == null) return NotFound("Party not found");

        Invitation? invitation = await _context.Invitation.FirstOrDefaultAsync(i => i.IdUser == IdUser && i.IdParty == IdParty);
        if (invitation == null) return BadRequest("User does not have an invitation to this party");

        try
        {
            _context.Invitation.Remove(invitation!);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }


        return NoContent();
    }
}
