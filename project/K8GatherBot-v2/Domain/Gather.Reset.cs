namespace K8GatherBotv2.Domain
{
    using System.Threading.Tasks;

    using K8GatherBotv2.Locales;

    /// <summary>
    /// The gather.
    /// </summary>
    public partial class Gather
    {
        /// <summary>
        /// Tries to reset the gather.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<TaskResult> TryReset(Player player)
        {
            await Task.Yield();

            // TODO: Maybe check only "admin users" to reset bot?
            this.Reset();

            return new TaskResult(true, ErrorCode.None, Keys.AdminResetSuccessful);
        }
    }
}
