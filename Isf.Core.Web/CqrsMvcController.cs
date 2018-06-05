using Isf.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Web
{
    public class CqrsMvcController : CqrsController
    {
        //happy path
        protected async Task<IActionResult> DispatchCommandAsync<TCommand>(TCommand command, Func<CommandResult, IActionResult> onSuccess)
            where TCommand : Command
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }

            var result = await commandBus.PublishAsync(command);

            if (result.State == ExecutionStatus.Succeeded)
            {
                return onSuccess(result);
            }
            else
            {
                ModelState.AddModelErrors(result.Notification);
                return View(command);
            }
        }

        protected async Task<IActionResult> DispatchQueryAsync<TQuery>(TQuery query, Func<QueryResult, IActionResult> onSuccess)
            where TQuery : Query
        {
            //not sure if this is "required", but might be useful ensuring there is an ID on GetByID queries
            if (!ModelState.IsValid)
            {
                return View(query);
            }

            var result = await queryBus.PublishAsync(query);

            if (result.State == ExecutionStatus.Succeeded)
            {
                return onSuccess(result);
            }
            else
            {
                ModelState.AddModelErrors(result.Notification);
                return View(query);
            }
        }

        protected async Task<IActionResult> DispatchQueryAsync<TQuery>(TQuery query)
            where TQuery : Query
        {
            return await DispatchQueryAsync(query, q => View(q.Result));
        }
    }
}
