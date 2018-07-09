namespace Softcrafts.jobservice.API.Controllers
{
    /// <summary>
    /// </summary>
    public class Names
    {
        /// <summary>
        /// </summary>
        protected Names(){}

        internal const string Jobs = "Jobs";
        internal const string CountAlle = "CountAlle";
        internal const string CountOffen = "CountOffen";
        internal const string CountInArbeit = "CountInArbeit";
        internal const string CountCanceled = "CountCanceled";
        internal const string ErmittleAlleJobs = "ErmittleAlleJobs";
        internal const string ErmittleUnerledigteJobs = "ErmittleUnerledigteJobs";
        internal const string ErmittleGecancelteJobs = "ErmittleGecancelteJobs";
        internal const string NeuerJob = "NeuerJob";


        internal const string JobDetails = "JobDetails";
        internal const string SetzeJobDone = "SetzeJobDone";
        internal const string SetzeJobInWork = "SetzeJobInWork";
        internal const string SetzeJobIsCanceled = "SetzeJobIsCanceled";
        internal const string LoescheJobIsCanceledFlag = "LoescheJobIsCanceledFlag";
        internal const string LoescheJob = "LoescheJob";

        internal const string Admin = "Admin";
        internal const string Heartbeat = "Heartbeat";
        internal const string GetAllApiUsers = "GetAllApiUsers";
        internal const string GetAllAdminApiUsers = "GetAllAdminApiUsers";
        internal const string NewApiUser = "NewApiUser";
        internal const string DeleteApiUser = "DeleteApiUser";
        internal const string SetApiUserAsAdmin = "SetApiUserAsAdmin";
        internal const string RevokeAdminFromApiUser = "RevokeAdminFromApiUser";

        internal static string GetJobCompleteJobNamePath(string jobname)
        {
            return $"/{Jobs}/{jobname}";
        }
    }

}
