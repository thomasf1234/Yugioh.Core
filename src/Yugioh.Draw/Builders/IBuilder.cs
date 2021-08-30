using System;
using System.Collections.Generic;
using System.Text;

namespace Yugioh.Draw.Builders
{
    public interface IBuilder<TRequest>
    {
        TRequest Build();
    }
}
