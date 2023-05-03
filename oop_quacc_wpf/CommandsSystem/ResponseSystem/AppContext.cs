using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop_quacc_wpf.CommandsSystem.ResponseSystem
{
    /// <summary>
    /// Defines special behaviours for App to respond to.
    /// </summary>
    public record AppContext(bool ShouldExit, bool ShouldHide, string ResponseMessage);
}
