using System;
using System.Collections.Generic;
using System.Text;

namespace Isf.Core.Logging
{
    public interface ILogger
    {
        void Debug(string message, Exception ex = null);
        void Trace(string message, Exception ex = null);
        void Info(string message, Exception ex = null);
        void Error(string message, Exception ex = null);
        void Fatal(string message, Exception ex = null);
    }
}
