using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace JDLL.Components
{
    public class ComponentHolder
    {
        volatile List<IComponent> Components = new List<IComponent>();
        volatile bool shouldClose = false;

        volatile object Parent;

        public ComponentHolder(object Parent)
        {
            this.Parent = Parent;

            Thread Starter = new Thread(new ThreadStart(() => Start()));
            Starter.Start();
        }

        private void Start()
        {
            while (true)
            {
                if (shouldClose)
                    break;

                try
                {
                    for (int i = 0; i < Components.ToArray().Length; i++)
                        Components[i].Update();

                    Thread.Sleep(100);
                }
                catch(Exception ex)
                {
                    throw ex;
                }

            }

            while (true)
            {
                try
                {
                    if (Components.ToArray().Length == 0)
                        break;

                    for (int i = 0; i < Components.ToArray().Length; i++)
                    {
                        Components[i].Dispose();
                        Components.RemoveAt(i);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Add(IComponent Component)
        {
            Component.Run(Parent);
            Components.Add(Component);
        }

        public void Remove(String Name)
        {
            for (int i = 0; i < Components.ToArray().Length; i++)
                if (Components[i].Equals(Name))
                {
                    Components.RemoveAt(i);
                    break;
                }
        }

        public void Remove(IComponent Component)
        {
            Components.Remove(Component);
        }

        public void Stop()
        {
            shouldClose = true;
        }
    }
}
