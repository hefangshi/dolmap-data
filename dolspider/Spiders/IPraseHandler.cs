using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dolspider.Spiders
{
    interface IPraseHandler<T>
    {
        T Prase();
    }
}
