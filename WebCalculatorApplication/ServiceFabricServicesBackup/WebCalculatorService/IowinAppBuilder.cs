using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCalculatorService
{
    public interface IowinAppBuilder
    {
        void Configuration(IAppBuilder appBuilder);
    }
}
