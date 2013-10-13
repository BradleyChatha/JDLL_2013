using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using JDLL.Components;

namespace Testerr
{
    class Test : IComponent
    {
        Program Prog;
        String Message;

        public Test(String Message)
        {
            this.Name = "Test";
            this.Description = "This is a test Description";
            this.Message = Message;
        }

        public override void Run(object Parent)
        {
            Prog = (Program)Parent;
        }

        public override void Update()
        {
            Console.WriteLine(Message);
        }

        public override void Dispose()
        {
            Prog = null;
        }
    }
}
