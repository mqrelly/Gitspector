using System;

namespace Gitspector.Cli
{
    class Program
    {
        static void Main( string[] args )
        {
            string repo_dir = args.Length > 0 ? args[0] : ".";
            Console.WriteLine("Looking for repo from: {0}", repo_dir);
            try
            {
                var repo = new Repository(repo_dir);

                Console.WriteLine("Repo root dir: {0}", repo.RootDir);
                Console.WriteLine("HEAD:          {0}", repo.Head);
                Console.WriteLine("HEAD commit:   {0}", repo.HeadCommit);

                Console.WriteLine("Local branches:");
                foreach( string br in repo.Branches )
                    Console.WriteLine("  {0} -> {1}", br, repo.HeadCommitOfRef(br));

                Console.WriteLine("All remotes:");
                foreach( string rem in repo.Remotes )
                    Console.WriteLine("  {0}", rem);

                Console.WriteLine("All refs:");
                foreach( string br in repo.GetRefs("*") )
                    Console.WriteLine("  {0} -> {1}", br, repo.HeadCommitOfRef(br));
            }
            catch( Exception ex )
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }
    }
}
