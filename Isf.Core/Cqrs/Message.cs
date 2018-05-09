using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public class Message
    {
        public Guid Id { get; set; }
    }

    public class Query : Message { }

    public class Command : Message { }

    public enum ExecutionStatus
    {
        NotAttempted,
        Validated,
        ValidationFailed,
        Succeeded,
        Failed
    }

    public class ExecutionResult : Message
    {
        public readonly ExecutionStatus State;
        public readonly Notification Notification;

        public ExecutionResult(ExecutionStatus state, Notification notification)
        {
            this.State = state;
            this.Notification = notification;
        }
    }

    public class CommandResult : ExecutionResult{
        public CommandResult(ExecutionStatus state, Notification notification)
            :base(state, notification)
        {
        }

        public static Task<CommandResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }
        public static CommandResult Success()
        {
            return new CommandResult(ExecutionStatus.Succeeded, Notification.OK);
        }
    }

    public class QueryResult<TResult> : ExecutionResult
    {
        public readonly TResult Result;
        public QueryResult(ExecutionStatus state, Notification notification, TResult result)
            :base(state, notification)
        {
            this.Result = result;
        }

        public static QueryResult<TResult> Success(TResult result)
        {
            return new QueryResult<TResult>(
                ExecutionStatus.Succeeded, Notification.OK, result);
        }
        public static Task<QueryResult<TResult>> SuccessAsync(TResult result)
        {
            return Task.FromResult(Success(result));
        }
    }
        
}
