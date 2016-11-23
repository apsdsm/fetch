using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fetch {
    public interface IBridge<T> {

        T controller { get; }

    }
}
