using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyPlanning.Data;
using PartyPlanning.Model.PartyModels;

namespace PartyPlanning.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
public class PartyController : ControllerBase
{
    private readonly UserManager<PartyUser> _userManager;
    private readonly PartyContext _context;

    public PartyController(UserManager<PartyUser> userManager, PartyContext context)
    {
        _userManager = userManager;
        _context = context;
    }


    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<PartyDTO>>> GetParties()
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        List<Party> parties = await _context.Party.Include(party => party.User).ToListAsync();
        return parties.ToDTOList();
    }

    [HttpGet]
    [Route("{IdParty}")]
    public async Task<ActionResult<PartyDTO>> GetParty([FromRoute] Guid IdParty)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Party? party = await _context.Party.Include(party => party.User).FirstOrDefaultAsync(p => p.IdParty == IdParty);
        if (party == null) return NotFound("No Party with this Id");

        return party.ToDTO();
    }


    [HttpPost]
    [Route("new")]
    public async Task<IActionResult> CreateParty([FromBody] MakePartyDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        PartyUser? user = await _userManager.FindByIdAsync(dto.IdPartyMaker.ToString());
        if (user == null) return NotFound("User not found");
        Party party = new Party
        {
            IdUser = user.Id,
            Name = dto.Name,
            Description = dto.Description,
            Adresse = dto.Adresse,
            DateStart = dto.DateStart,
            DateFin = dto.DateEnd,
        };

        try
        {
            _context.Party.Add(party);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }


        return Ok($"New Party Create with Name : {party.Name}");
    }

    [HttpPut]
    [Route("{IdParty}")]
    public async Task<IActionResult> EditParty([FromRoute] Guid IdParty, [FromBody] EditPartyDTO editDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (IdParty != editDTO.IdPartyMaker) return BadRequest("L'id est != de celui du dto");

        Party? entity = await _context.Party.FirstOrDefaultAsync(p => p.IdParty == IdParty);
        if (entity == null) return NotFound("No Party with this Id");

        entity.Name = editDTO.Name;
        entity.Description = editDTO.Description;
        entity.Adresse = editDTO.Adresse;
        entity.DateStart = editDTO.DateStart;
        entity.DateFin = editDTO.DateEnd;


        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound();
        }

        return Ok($"Party with id : ${IdParty} successfully modified");
    }

    [HttpDelete]
    [Route("{IdParty}")]
    public async Task<IActionResult> DeletePartie([FromRoute] Guid IdParty)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Party? party = await _context.Party.FirstOrDefaultAsync(p => p.IdParty == IdParty);
        if (party == null) return NotFound("No Party with this Id");

        PartyUser? user = await _context.PartyUsers.Include(u => u.Parties).FirstOrDefaultAsync(u => u.Id == party.IdUser);
        if (user == null) return NotFound("No User with this Id");

        try
        {
            user.Parties.Remove(party);
            _context.Party.Remove(party);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.InnerException);
        }


        return NoContent();
    }


}