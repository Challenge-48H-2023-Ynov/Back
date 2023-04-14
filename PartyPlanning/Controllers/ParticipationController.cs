using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyPlanning.Data;
using PartyPlanning.Model.ParticipationModels;

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

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<ParticipationDTO>>> GetParticipations()
        => await _context.Participation.Select(p => p.ToDTO()).ToListAsync();

    [HttpGet]
    [Route("party/{IdParty}")]
    public async Task<ActionResult<List<ParticipationDTO>>> GetParticipationByParty([FromRoute] Guid IdParty)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        Party? party = await _context.Party.FirstOrDefaultAsync(p => p.IdParty == IdParty);
        if (party == null) return BadRequest("Party not found");

        List<Participation>? participation = await _context.Participation.Where(p => p.IdParty == IdParty).ToListAsync();
        if (participation == null) return BadRequest("Participation not found");

        return participation.ToDTOList();
    }

    [HttpGet]
    [Route("user/{IdUser}")]
    public async Task<ActionResult<List<ParticipationDTO>>> GetParticipationByUser([FromRoute] Guid IdUser)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        PartyUser? user = await _userManager.FindByIdAsync(IdUser.ToString());
        if (user == null) return BadRequest("User not found");

        List<Participation>? participation = await _context.Participation.Where(p => p.IdUser == IdUser).ToListAsync();
        if (participation == null) return BadRequest("Participation not found");

        return participation.ToDTOList();
    }

    [HttpGet]
    [Route("{IdParty}/{IdUser}")]
    public async Task<ActionResult<ParticipationDTO>> GetParticipation([FromRoute] Guid IdParty, Guid IdUser)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        PartyUser? user = await _userManager.FindByIdAsync(IdUser.ToString());
        if (user == null) return BadRequest("User not found");

        Party? party = await _context.Party.Include(p => p.Participations).FirstOrDefaultAsync(p => p.IdParty == IdParty);
        if (party == null) return BadRequest("Party not found");


        Participation? participation = _context.Participation.FirstOrDefault(p => p.IdParty == IdParty && p.IdUser == IdUser);
        if (participation == null) return BadRequest("Participation not found");

        return participation.ToDTO();
    }

    [HttpPost]
    [Route("new/{IdParty}/{IdUser}")]
    public async Task<IActionResult> CreateParticipation([FromRoute] Guid IdParty, Guid IdUser, [FromBody] MakeParticipationDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (IdParty != dto.IdParty) return NotFound("L'IdParty est != de celui du dto");
        if (IdUser != dto.IdUser) return NotFound("L'IdUser est != de celui du dto");

        PartyUser? user = await _userManager.FindByIdAsync(IdUser.ToString());
        if (user == null) return BadRequest("User not found");

        Party? party = await _context.Party.Include(p => p.Participations).FirstOrDefaultAsync(p => p.IdParty == IdParty);
        if (party == null) return BadRequest("Party not found");

        if (party.IdUser == IdUser) return BadRequest("User is the owner of the party");
        if (party.Participations!.Any(p => p.IdUser == IdUser)) return BadRequest("User already participating to this party");

        Participation participation = new()
        {
            IdUser = IdUser,
            IdParty = IdParty,
            PlaceDispo = dto.PlaceDispo,
            IsSAM = dto.IsSAM,
            Role = "Participant",
            DateArrivee = dto.DateArrivee,
            DateDepart = dto.DateDepart
        };

        try
        {
            _context.Participation.Add(participation);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.InnerException);
        }

        return Ok($"New participation created for: {user.UserName}, in: {party.Name}");
    }

    [HttpPut]
    [Route("{IdParty}/{IdUser}")]
    public async Task<IActionResult> EditParticipation([FromRoute] Guid IdParty, Guid IdUser, [FromBody] EditParticipationDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (IdParty != dto.IdParty) return NotFound("L'IdParty est != de celui du dto");
        if (IdUser != dto.IdUser) return NotFound("L'IdUser est != de celui du dto");

        PartyUser? user = await _userManager.FindByIdAsync(IdUser.ToString());
        if (user == null) return BadRequest("User not found");

        Party? party = await _context.Party.FirstOrDefaultAsync(p => p.IdParty == IdParty);
        if (party == null) return BadRequest("Party not found");

        Participation? entity = _context.Participation.FirstOrDefault(p => p.IdParty == IdParty && p.IdUser == IdUser);
        if (entity == null) return BadRequest("Participation not found");

        entity.PlaceDispo = dto.PlaceDispo;
        entity.IsSAM = dto.IsSAM;
        entity.DateArrivee = dto.DateArrivee;
        entity.DateDepart = dto.DateDepart;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.InnerException);
        }

        return Ok($"Participation for: {user.UserName}, in: {party.Name} as been updated");
    }

    [HttpDelete]
    [Route("{IdParty}/{IdUser}")]
    public async Task<IActionResult> DeleteParticipation([FromRoute] Guid IdParty, Guid IdUser)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        PartyUser? user = await _userManager.FindByIdAsync(IdUser.ToString());
        if (user == null) return BadRequest("User not found");

        Party? party = await _context.Party.FirstOrDefaultAsync(p => p.IdParty == IdParty);
        if (party == null) return BadRequest("Party not found");

        Participation? participation = _context.Participation.FirstOrDefault(p => p.IdParty == IdParty && p.IdUser == IdUser);
        if (participation == null) return BadRequest("Participation not found");

        try
        {
            _context.Participation.Remove(participation);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.InnerException);
        }

        return NoContent();
    }
}