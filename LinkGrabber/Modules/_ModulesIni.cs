using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkGrabber.Modules
{
    public static class ModulesIni
    {
        public static void InitPackageModules(this LinkGrabber lg)
        {
            lg.AddGrabberModule(new YoutubeGrabber());
            lg.AddGrabberModule(new StreamCloudGrabber());
        }
    }
}
