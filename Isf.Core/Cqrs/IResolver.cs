﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Isf.Core.Cqrs
{
    public interface IResolver
    {
        void Register<TAbstract, TConcrete>();
        T GetMe<T>();
        object GetMe(Type type);
    }

    
}
