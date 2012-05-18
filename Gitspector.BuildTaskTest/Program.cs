using System;

namespace Gitspector.BuildTaskTest
{
    class Program
    {
        static void Main( string[] args )
        {
            string head_commit;
            var this_assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var stream = this_assembly.GetManifestResourceStream("Gitspector.BuildTaskTest.git-head-sha1.txt");

            using( var reader = new System.IO.StreamReader(stream) )
                head_commit = reader.ReadToEnd().Trim();

            Console.WriteLine("My git HEAD SHA-1 was {0} when I was built.", head_commit);
        }
    }
}
