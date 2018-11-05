namespace K8GatherBotv2.Extensions
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    using K8GatherBotv2.Domain;

    /// <summary>
    /// The task result extensions.
    /// </summary>
    public static class TaskResultExtensions
    {
        /// <summary>
        /// Writes the task result data into log.
        /// </summary>
        /// <param name="taskResult">The task result.</param>
        /// <param name="functionName">Name of the function.</param>
        public static void Log(this TaskResult taskResult, [CallerMemberName]string functionName = null)
        {
            Trace.WriteLine($"EX-{functionName} --- {DateTime.Now}");
            Trace.WriteLine($"!#DEBUG INFO FOR ERROR: {taskResult.Key}");
        }
    }
}
