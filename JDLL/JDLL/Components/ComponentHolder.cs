using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using JDLL.Data.Logging;

namespace JDLL.Components
{
    public class ComponentHolder
    {
        const String Sender = "Comp.Holder";

        volatile List<IComponent> Components = new List<IComponent>();
        volatile bool shouldClose = false;
        volatile bool Update = false;

        volatile object Parent;

        volatile int Count = 0;
        public volatile int Timer_Interval = 100;

        volatile Log Log;

        public ComponentHolder(object Parent, Log Log = null)
        {
            this.Parent = Parent;

            if (Log != null)
                this.Log = Log;

            Thread Starter = new Thread(new ThreadStart(() => Start()));
            Starter.Start();

            WriteToLog(Sender, "Started", null, true);
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

                    Thread.Sleep(Timer_Interval);
                }
                catch(Exception ex)
                {
                    WriteToLog(Sender, ex.Message, new SEV_Severe(ex), true);
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
                    WriteToLog(Sender, ex.Message, new SEV_Severe(ex), true);
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

            WriteToLog(Sender, "Component Added - Name = " + Component.Name, null, false);
        }

        public void Remove(String Name)
        {
            while (Update) { }

            for (int i = 0; i < Components.ToArray().Length; i++)
                if (Components[i].Name.Equals(Name))
                {
                    Components[i].Dispose();
                    Components.RemoveAt(i);
                    WriteToLog(Sender, "Component Removed - Name = " + Components[i].Name, null, false);
                    break;
                }
        }

        [Obsolete("Currently does not work, can potentially cause a crash", true)]
        public void Remove(IComponent Component)
        {
            while (Update) { }

            int Backup = Component.ID;

            for (int i = 0; i < Components.ToArray().Length; i++)
            {
                Component.ID = i;

                if (Components[i] == Component)
                {
                    Components[i].Dispose();
                    Components.RemoveAt(i);
                    WriteToLog(Sender, "Component Removed - Name = " + Component.Name, null, false);
                    break;
                }
            }

            Component.ID = Backup;
        }

        public void Remove(int ID)
        {
            while (Update) { }

            Components[ID].Dispose();
            Components.RemoveAt(ID);
            WriteToLog(Sender, "Component Removed - Name = " + Components[ID].Name, null, false);

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
            WriteToLog(Sender, "Stopped", new SEV_Info(), true);
        }

        public void WriteToLog(String Sender, String Message, ISeverety Severety, bool Save = false)
        {
            if (Severety == null)
                Severety = new SEV_Info();


            if (Log != null)
            {
                Log.Write(Sender, Message, Severety, Save);
            }
        }

        ~ComponentHolder()
        {
            if(!shouldClose)
                Stop();

            shouldClose = true;

            Thread.Sleep(200);

            Components = null;
            Parent = null;
            Count = 0;
            Log = null;
        }
    }
}
