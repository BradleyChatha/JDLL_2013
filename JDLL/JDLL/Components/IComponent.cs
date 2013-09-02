using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Components
{
    public abstract class IComponent
    {
        public String Name;
        public String Description;

        public int ID;
        public bool shouldUpdate = true;

        public abstract void Run(object Parent);

        public abstract void Update();

        public abstract void Dispose();

        public void SetDetails(String Name, String Description)
        {
            this.Name = Name;
            this.Description = Description;
        }
    }
}
