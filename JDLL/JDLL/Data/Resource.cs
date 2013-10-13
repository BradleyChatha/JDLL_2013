using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JDLL.Data
{
    /// <summary>
    /// Structure used to store Header data
    /// </summary>
    public struct Header
    {
        /// <summary>
        /// The name of the HEADER, used for when accessing Entries
        /// </summary>
        public String HEADERNAME;

        /// <summary>
        /// The Extention (Type) of the file, used for custom processing
        /// </summary>
        public String TYPE;

        /// <summary>
        /// The Path to the file that the entry is made for, used for accessing the file
        /// </summary>
        public String FILENAME;

        /// <summary>
        /// The Key of the entry, used for accessing the Entry directly
        /// </summary>
        public String KEY;
    }

    /// <summary>
    /// Class that handles .sealresource files, Exposes enough info for custom processing if needed
    /// </summary>
    public class Resource
    {
        String _FileName; // Global FileName
        String _StructMaker = "&"; // String that defines when a Struct is being declared
        String _EntryMarker = "$"; // String that defines when an Entry is being declared
        String _Tab = "\t\t\t"; // Useless
        String _Extention = ".sealresource"; // Extention to always use

        /// <summary>
        /// A dictionary containing the current Headers that can be used in Entries, FILENAME and KEY both have default values, Public for custom processing
        /// </summary>
        public Dictionary<String, Header> Headers = new Dictionary<String, Header>();

        /// <summary>
        /// A dictionary containt the current Entries, All Header Fields are filled, Public for custom processing
        /// </summary>
        public Dictionary<String, Header> Entries = new Dictionary<String, Header>();

        /// <summary>
        /// Class that handles .sealresource files, Exposes enough info for custom processing if needed
        /// </summary>
        /// <param name="filename">Name of the file, will always end in .sealresource</param>
        public Resource(String filename)
        {
            if (!filename.EndsWith(this._Extention)) // Checking to see if the filename has '.sealresource' as the extention
            {
                filename += this._Extention; // If not then add it
            }

            if (!File.Exists(filename)) // Checking to see if the file exists
            {
                File.Create(filename).Dispose(); // If not create it
            }

            this._FileName = filename; // Setting the global filename

            if (File.ReadAllLines(filename).Length != 0) // Checking to see if the file has at least 1 line of text
            {
                this.FillHeaders(); // If so Fill the Headers Dictionary
                this.FillEntries(); // And Fill the Entries Dictionary
            }
        }

        /// <summary>
        /// Reads all the bytes from the file assigned to 'key' and then returns them
        /// </summary>
        /// <param name="key">The key of the file you want to read</param>
        /// <returns>All bytes from the file designated to 'key'</returns>
        public byte[] ReadBytes(String key)
        {
            if (!this.Entries.ContainsKey(key) || !File.Exists(this.Entries[key].FILENAME)) // Making sure the key AND the file exist
            {
                throw new ArgumentException(key + " doesn't exist"); // If not throw an exception
            }

            return File.ReadAllBytes(this.Entries[key].FILENAME); // Return the bytes of the file 'key' is associated to
        }

        /// <summary>
        /// Reads all the lines from the file assigned to 'key' and then returns them
        /// </summary>
        /// <param name="key">The key of the file you want to read</param>
        /// <returns>Reads all the lines from the file designated to 'key'</returns>
        public String[] ReadAllLines(String key)
        {
            if (!this.Entries.ContainsKey(key) || !File.Exists(this.Entries[key].FILENAME)) // Making sure the key AND the file exist
            {
                throw new ArgumentException(key + " doesn't exist"); // If not throw an exception
            }

            return File.ReadAllLines(this.Entries[key].FILENAME); // Return the contents of the file 'key' is associated to
        }


        /// <summary>
        /// Returns a FileStream of the file assigned to 'key' using the designated FileMode
        /// </summary>
        /// <param name="key">The key of the file you want to stream</param>
        /// <param name="mode">The FileModes you want to open the stream with</param>
        /// <returns>A FileStream of the file designated to 'key'</returns>
        public FileStream CreateStream(String key, FileMode mode)
        {
            if (!this.Entries.ContainsKey(key) || !File.Exists(this.Entries[key].FILENAME)) // Making sure the key AND the file exist
            {
                throw new ArgumentException(key + " doesn't exist"); // If not throw an exception
            }

            return new FileStream(this.Entries[key].FILENAME, mode); // Create a file stream of the file associated to 'key' and return it
        }

        /// <summary>
        /// Creates a header that is used to define entries
        /// </summary>
        /// <param name="headerName">Name of the header</param>
        /// <param name="typeValue">The common extention, Currently no use</param>
        public void WriteHeader(String headerName, String typeValue)
        {
            if (this.Headers.ContainsKey(headerName)) // Making sure the header doesn't exist
            {
                return; // If not then return, no need for an exception
            }

            String[] Contents = File.ReadAllLines(this._FileName); // Get the current contents of the file
            List<String> New = new List<String>(); // Used to merge the current content with the new header

            New.AddRange(new String[]
            {
                this._StructMaker + headerName,
                "{",
                "\t" + "TYPE" + this._Tab + typeValue,
                "\t" + "FILENAME" + "\t\t" + "string",
                "}\n"
            }); // Add the new Header
            New.AddRange(Contents); // Add the current content

            File.WriteAllLines(this._FileName, New.ToArray()); // Write the new content to the file 'New'

            Header New2 = new Header(); // Create a new Header Object
            New2.HEADERNAME = headerName; // Setting the HEADERNAME to 'header'
            New2.TYPE = typeValue; // Setting the TYPE to 'typeValue'
            New2.FILENAME = "string"; // Setting the FILENAME to default

            this.Headers[New2.HEADERNAME] = New2; // Register the Header 'New2'
        }

        /// <summary>
        /// Write an entry that is defined with a Header(Must exist), a filepath to a file and a key to access that file
        /// </summary>
        /// <param name="header">The header to associate with it</param>
        /// <param name="filePath">The path to a file the key is associated tos</param>
        /// <param name="key">The key to associate to 'filePath'</param>
        public void WriteEntry(String header, String filePath, String key)
        {
            if (!this.Headers.ContainsKey(header)) // Making sure the header exists
            {
                throw new ArgumentException(header + " does not exist"); // Throw an error if it doesn't
            }

            if (this.Entries.ContainsKey(key)) // Making sure the Entry doesn't exist
            {
                return; // If not just return
            }

            String[] Contents = File.ReadAllLines(this._FileName); // Gets the current contents of the file
            List<String> New = new List<String>(); // Used to merge the current contents with the new entry
            New.AddRange(Contents); // Add the current content
            New.Add(this._EntryMarker + header + " |> " + key + " = " + filePath); // Add the new entry

            File.WriteAllLines(this._FileName, New.ToArray()); // Write the new Contents to the file 'New'

            Header NewHeader = new Header(); // Create a new Header object, used for the entry
            NewHeader.HEADERNAME = header; // Setting the HEADERNAME to 'header'
            NewHeader.TYPE = this.Headers[NewHeader.HEADERNAME].TYPE; // Setting the type to the Headers type
            NewHeader.FILENAME = filePath; // Setting the filepath to 'filepath'
            NewHeader.KEY = key; // Setting the key to 'key'

            this.Entries[key] = NewHeader; // Register the entry 'NewHeader'
        }

        /// <summary>
        /// Returns all entries that are associated with 'header'
        /// </summary>
        /// <param name="header">The header that you want to get the entries of</param>
        /// <returns>All Headers that have 'header' as their HEADERNAME</returns>
        public Header[] GetAllEntriesOfHeader(String header)
        {
            List<Header> ToReturn = new List<Header>(); // Holds the entries

            foreach (KeyValuePair<String, Header> kvp in Entries) // Goes though every current entry
            {
                if (kvp.Value.HEADERNAME.Equals(header)) // Checks to see if it's HEADERNAME is the same as 'header'
                {
                    ToReturn.Add(kvp.Value); // If it is then add it to 'ToReturn'
                }
            }

            return ToReturn.ToArray(); // Return it in Array form
        }

        /// <summary>
        /// Organises all of the Entries by their headers
        /// </summary>
        public void Organise()
        {
            String[] Contents = File.ReadAllLines(this._FileName); // Current contents of the file
            List<String> New = new List<String>(); // Organised Entries
            List<Header> Heads = new List<Header>(); // Getting the used Headers

            bool Paragraph = false; // Make a new line to seperate the Entries

            foreach (KeyValuePair<String, Header> h in this.Headers)
            {
                Heads.Add(h.Value);

                foreach (String s in Contents)
                {
                    if (s.StartsWith(this._EntryMarker + h.Value.HEADERNAME))
                    {
                        New.Add(s);
                        Paragraph = true;
                    }
                }

                if (Paragraph)
                {
                    New.Add("");
                }

                Paragraph = false;
            }

            File.Create(this._FileName).Dispose(); // Empties the file

            List<String> New2 = new List<String>(); // The new contents of the file, contains the headers and 'New' (Organised Entries)

            for (int i = 0; i < Heads.ToArray().Length; i++) // Going through each header and writing it to the start of 'New2'
            {
                New2.AddRange(new String[]
                {
                    this._StructMaker + Heads[i].HEADERNAME,
                    "{",
                    "\t" + "TYPE" + this._Tab + Heads[i].TYPE,
                    "\t" + "FILENAME" + "\t\t" + "string",
                    "}\n"
                });
            }

            New2.AddRange(New); // Add the organised Entries
            File.WriteAllLines(this._FileName, New2.ToArray()); // Write the new contents 'New2' into the file
        }

        /// <summary>
        /// Returns whether the entry (Defined using 'key') exists
        /// </summary>
        /// <param name="key">The key of the entry</param>
        /// <returns>Whether the entry exists</returns>
        public bool DoesEntryExist(String key)
        {
            return (this.Entries.ContainsKey(key)); // Checks to see if the key exists, then returns the value
        }

        private void FillEntries()
        {
            String[] Contents = File.ReadAllLines(this._FileName);

            foreach (String s in Contents)
            {
                if (s.StartsWith(this._EntryMarker))
                {
                    String[] Data = s.Split('|', '>');

                    String Data1 = Data[0].Trim(' ', '\t').Split(this._EntryMarker.ToCharArray()).Last(); // Header
                    String Data2 = Data.Last().Split('=').First().Trim(' '); // Key
                    String Data3 = Data.Last().Split('=').Last().Trim(' '); // File Path

                    // $EXAMPLE Key = Path
                    // Data1 = EXAMPLE
                    // Data2 = Key
                    // Data3 = Path

                    Header NewHeader = new Header();
                    NewHeader.HEADERNAME = Data1;
                    NewHeader.TYPE = this.Headers[NewHeader.HEADERNAME].TYPE;
                    NewHeader.FILENAME = Data3;
                    NewHeader.KEY = Data2;

                    this.Entries[Data2] = NewHeader;
                }
            }
        }

        private void FillHeaders()
        {
            String[] Contents = File.ReadAllLines(this._FileName);

            bool Setting = false;

            Header New = new Header();

            foreach (String s in Contents)
            {
                if (!Setting)
                {
                    if (s.StartsWith(this._StructMaker))
                    {
                        New.HEADERNAME = s.Split(this._StructMaker.ToCharArray()).Last();
                    }

                    if (s.Equals("{"))
                    {
                        Setting = true;
                    }
                }

                if (Setting)
                {
                    if (s.Equals("}"))
                    {
                        this.Headers[New.HEADERNAME] = New;
                        New = new Header();
                        Setting = false;
                    }

                    String Data1 = "";
                    String Data2 = "";

                    foreach (String s2 in s.Split('\t'))
                    {
                        if (!s2.Equals(" "))
                        {
                            if (Data1.Equals(""))
                            {
                                Data1 = s2;
                            }
                            else
                            {
                                Data2 = s2;
                            }
                        }
                    }

                    if (Data1.Equals("TYPE"))
                    {
                        New.TYPE = Data2;
                    }

                    if (Data1.Equals("FILENAME"))
                    {
                        New.FILENAME = Data2;
                    }
                }
            }
        }
    }
}
