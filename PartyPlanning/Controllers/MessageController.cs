using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyPlanning.Data;
using PartyPlanning.Model.MessageModels;
using PartyPlanning.Model.PartyModels;

namespace PartyPlanning.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly UserManager<PartyUser> _userManager;
    private readonly PartyContext _context;

    public MessageController(UserManager<PartyUser> userManager, PartyContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<MessageDTO>>> GetMessages()
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        List<Message> messages = await _context.Message.ToListAsync();
        return messages.ToDTOList();
    }

    [HttpGet]
    [Route("message/{IdMessage}")]
    public async Task<ActionResult<MessageDTO>> GetMessageById([FromRoute] Guid IdMessage)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Message? message = await _context.Message.FirstOrDefaultAsync(p => p.IdMessage == IdMessage);
        if (message == null) return NotFound("No Message with this Id");

        return message.ToDTO();
    }

    [HttpGet]
    [Route("party/{IdParty}")]
    public async Task<ActionResult<List<MessageDTO>>> GetMessageByIdParty([FromRoute] Guid IdParty)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        List<Message>? messages = await _context.Message.Where(p => p.IdParty == IdParty).ToListAsync();
        if (messages == null) return NotFound("No Message founds for this party");

        return messages.ToDTOList();
    }

    [HttpGet]
    [Route("user/{IdUser}")]
    public async Task<ActionResult<List<MessageDTO>>> GetMessageByIdUser([FromRoute] Guid IdUser)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        List<Message>? messages = await _context.Message.Where(p => p.IdUser == IdUser).ToListAsync();
        if (messages == null) return NotFound("No Message founds for this user");

        return messages.ToDTOList();
    }

    [HttpPost]
    [Route("new/{IdParty}/{IdUser}")]
    public async Task<IActionResult> CreateMessage([FromRoute] Guid IdParty, Guid IdUser, [FromBody] MakeMessageDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (IdParty != dto.IdParty) return BadRequest("L'IdParty est != de celui du dto");
        if (dto.IdUser != IdUser) return BadRequest("L'IdUser est != de celui du dto");

        PartyUser? user = await _context.PartyUsers.Include(u => u.Participations).FirstOrDefaultAsync(u => u.Id == IdUser);
        if (user == null) return NotFound("User not found");

        Party? party = await _context.Party.FirstOrDefaultAsync(p => p.IdParty == IdParty);
        if (party == null) return NotFound("Paty not found");

        if (user.Participations.Any(p => p.IdParty != IdParty) && party.IdUser != user.Id) return BadRequest("L'Utilisateur n'est pas dans la party");

        Message message = new()
        {
            Content = dto.Content,
            SendDate = DateTime.UtcNow,
            IdParty = IdParty,
            IdUser = IdUser,
        };

        try
        {
            _context.Message.Add(message);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }

        return Ok($"New Message added to Party: {party.Name}");
    }

    [HttpDelete]
    [Route("{IdMessage}")]
    public async Task<IActionResult> DeleteMessage([FromRoute] Guid IdMessage)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        Message? message = await _context.Message.FirstOrDefaultAsync(m => m.IdMessage == IdMessage);
        if (message == null) return NotFound("No message with this Id");

        try
        {
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }

        return NoContent();
    }
}