using System;
using System.Collections.Generic;
using System.Text;

namespace Isf.Core.Common
{
    public interface IUsernameProvider
    {
        string GetUsername();
    }

    public class StaticUsernameProvider : IUsernameProvider
    {
        public string GetUsername()
        {
            return "StaticID";
        }
    }
}
