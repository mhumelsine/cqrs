using Isf.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Isf.Core.Web
{
    public class CqrsApiController : CqrsController
    {
        //happy path
        protected async Task<IActionResult> DispatchAsync<TCommand>(TCommand command, Func<object> createOkResponse)
            where TCommand : Command
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await commandBus.PublishAsync(command);

            if (result.State == ExecutionStatus.Succeeded)
            {
                return Ok();
            }
            else
            {
                ModelState.AddModelErrors(result.Notification);
                return BadRequest(ModelState);
            }
        }

        protected async Task<IActionResult> DispatchAsync<TQuery>(TQuery query)
            where TQuery : Query
        {
            //not sure if this is "required", but might be useful ensuring there is an ID on GetByID queries
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await queryBus.PublishAsync(query);

            if (result.State == ExecutionStatus.Succeeded)
            {
                return Ok(result.Result);
            }
            else
            {
                ModelState.AddModelErrors(result.Notification);
                return BadRequest(ModelState);
            }
        }
    }
}
