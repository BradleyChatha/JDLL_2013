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
        volatile bool Update = false;

        volatile object Parent;

        volatile int Count = 0;

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
                    Update = true;

                    for (int i = 0; i < Components.ToArray().Length; i++)
                        if(Components[i].shouldUpdate)
                            Components[i].Update();

                    Update = false;

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
            Component.ID = Count;
            Count++;
            Component.Run(Parent);
            Components.Add(Component);
        }

        public void Remove(String Name)
        {
            for (int i = 0; i < Components.ToArray().Length; i++)
                if (Components[i].Equals(Name))
                {
                    Components[i].Dispose();
                    Components.RemoveAt(i);
                    break;
                }
        }

        public void Remove(IComponent Component)
        {
            for (int i = 0; i < Components.ToArray().Length; i++)
                if (Components[i] == Component)
                {
                    Components[i].Dispose();
                    Components.RemoveAt(i);
                    break;
                }
        }

        public void Remove(int ID)
        {
            for (int i = 0; i < Components.ToArray().Length; i++)
                if (Components[i].ID == Count)
                {
                    Components[i].Dispose();
                    Components.RemoveAt(i);
                    break;
                }

            while (Update) { }

            Count = 0;

            for (int i = 0; i < Components.ToArray().Length; i++)
            {
                Components[i].ID = Count;
                Count++;
            }
        }

        public IEnumerable<IComponent> GetComponents()
        {
            if (!Update)
                return Components;
            else
            {
                while (Update) { }
                return Components;
            }
        }

        public IComponent GetComponent(int ID)
        {
            for (int i = 0; i < Components.ToArray().Length; i++)
                if (Components[i].ID == ID)
                    return Components[i];

            return null;
        }

        public void Stop()
        {
            shouldClose = true;
        }
    }
}
