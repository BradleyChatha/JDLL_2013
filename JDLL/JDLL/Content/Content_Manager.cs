using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDLL.Content
{
    public class Content_Manager
    {
        Dictionary<String, IContentProcessor> Processors = new Dictionary<String, IContentProcessor>();

        public String Filename { get; private set; }

        public Content_Manager(String filename)
        {
            this.Filename = filename;
        }

        public void RegisterProcessor(IContentProcessor processor)
        {
            this.Processors.Add(processor.TypeName(), processor);
        }
    }
}
