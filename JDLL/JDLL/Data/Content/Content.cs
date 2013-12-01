using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JDLL.Data.Content
{
    public class Content
    {
        public String Filepath { get; private set; }

        public Content(String filepath)
        {
            this.Filepath = filepath;
        }


    }
}
