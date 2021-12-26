using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    internal class Context2 : IContext<string>
    {
        public string Data => "Job2";
    }
}
