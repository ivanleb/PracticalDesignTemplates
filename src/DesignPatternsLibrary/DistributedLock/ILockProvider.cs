using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.DistributedLock
{
    internal interface ILockProvider
    {
        ILock CreateLock(string name);
    }
}
