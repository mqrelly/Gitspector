using System;
using Microsoft.Build.Framework;

namespace Gitspector.Tasks
{
    public class InspectGitRepo : Microsoft.Build.Utilities.Task
    {
        private Repository repo;

        [Required]
        public string StartLookingInDir { get; set; }

        [Output]
        public string HeadCommit
        {
            get { return this.repo.HeadCommit; }
        }

        public override bool Execute()
        {
            this.repo = new Repository(this.StartLookingInDir);
            return true;
        }
    }
}
