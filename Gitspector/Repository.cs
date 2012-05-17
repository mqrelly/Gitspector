using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Gitspector
{
    public class Repository
    {
        #region Constructor

        public Repository( string git_repo_dir )
        {
            this.repo_dir = Path.GetFullPath(git_repo_dir);
            this.git_dir = Path.Combine(this.repo_dir, ".git");

            while( !Directory.Exists(this.git_dir) )
            {
                this.repo_dir = Path.GetDirectoryName(this.repo_dir);
                if( string.IsNullOrEmpty(this.repo_dir) )
                    throw new ArgumentException("Git repository not found.");
                this.git_dir = Path.Combine(this.repo_dir, ".git");
            }
        }

        #endregion

        #region Private

        private string repo_dir;
        private string git_dir;

        private string ConvertRefPath( string reference )
        {
            return reference.Replace('\\', '/');
        }

        private string Dereference( string reference )
        {
            reference = this.ConvertRefPath(reference);

            if( reference.StartsWith("ref: ") )
                reference = reference.Substring(5);

            string deref = File.ReadAllText(Path.Combine(this.git_dir, reference)).Trim();

            if( deref.StartsWith("ref: ") )
                return Dereference(deref);
            else
                return deref;
        }

        #endregion

        #region Public

        public string RootDir
        {
            get { return this.repo_dir; }
        }

        public string Head
        {
            get { return File.ReadAllText(Path.Combine(this.git_dir, "HEAD")).Trim(); }
        }

        public string HeadCommit
        {
            get { return this.Dereference("HEAD"); }
        }

        public string HeadCommitOfRef( string branch )
        {
            branch = this.ConvertRefPath(branch);

            if( !branch.StartsWith("refs/") )
                branch = "refs/heads/" + branch;

            return this.Dereference(branch);
        }

        public IList<string> Branches
        {
            get { return new List<string>(this.GetRefs(new Regex("^refs/heads/"))); }
        }

        public IList<string> Remotes
        {
            get
            {
                var remotes = new List<string>();

                foreach( string remote in Directory.EnumerateDirectories(
                    Path.Combine(this.git_dir, "refs", "remotes")) )
                    remotes.Add(this.ConvertRefPath(remote.Substring(remote.LastIndexOf("refs"))));

                return remotes;
            }
        }

        public IEnumerable<string> GetRefs( string pattern )
        {
            pattern = this.ConvertRefPath(pattern);

            if( !pattern.StartsWith("refs/") )
                pattern = "refs/" + pattern;

            foreach( string branch in Directory.EnumerateFiles(
                this.git_dir, pattern, SearchOption.AllDirectories) )
                yield return this.ConvertRefPath(branch.Substring(branch.LastIndexOf("refs")));
        }

        public IEnumerable<string> GetRefs( Regex pattern )
        {
            foreach( string ref_file in Directory.EnumerateFileSystemEntries(
                 Path.Combine(this.git_dir, "refs"), "*", SearchOption.AllDirectories) )
            {
                string ref_ = this.ConvertRefPath(ref_file.Substring(ref_file.LastIndexOf("refs")));
                if( pattern.IsMatch(ref_) )
                    yield return ref_;
            }
        }

        #endregion
    }
}
