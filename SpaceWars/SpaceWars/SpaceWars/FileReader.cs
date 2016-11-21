using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SpaceWars
{
    class FileReader
    {
        List<String> list;
        String line;

        // Constructors
        public FileReader()
        {
            list = new List<String>();
            line = "";
        }

        // Methods
        public List<String> read()
        {
            using (StreamReader reader = new StreamReader("C:\\Users\\Kyle\\Desktop\\scores.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                }
                reader.Close();
            }

            return list;
        }

        public void write(String line)
        {
            using (StreamWriter writer = new StreamWriter("C:\\Users\\Kyle\\Desktop\\scores.txt", true))
            {
                writer.WriteLine(line);
                writer.Close();
            }
        }
    }
}
