using ContactsBookAPI.Application.Commands;
using ContactsBookAPI.Application.Queries;
using ContactsBookAPI.Domain.Exceptions;
using ContactsBookAPI.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContactsBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<IActionResult> CreateContact([FromBody] CreateContactCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                return Ok(result);
            }

            catch (UserOperationException)
            {
                return BadRequest(new Message { message = "Email exists!" });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateContact([FromBody] UpdateContactCommand command)
        {
            try
            {
                var res = await _mediator.Send(command);

                return Ok(res);
            }

            catch (UserOperationException ex)
            {
                return BadRequest(new Message { message = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteContact([FromQuery] int id)
        {
            try
            {
                var command = new DeleteContactCommand { Id = id };

                var res = await _mediator.Send(command);

                return Ok(res);
            }

            catch (UserOperationException ex)
            {
                return NotFound(new Message { message = ex.Message });
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetContactList([FromQuery] int? pageNumber, [FromQuery] int? pageSize, [FromQuery] string? search)
        {
            try
            {
                var query = new GetContactListQuery
                { 
                    PageNumber = pageNumber, 
                    PageSize = pageSize, 
                    Search = search 
                };

                var res = await _mediator.Send(query);

                return Ok(res);
            }

            catch (UserOperationException ex)
            {
                return BadRequest(new Message { message = ex.Message });
            }
        }
    }
}