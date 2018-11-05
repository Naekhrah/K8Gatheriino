namespace K8GatherBotv2.Domain
{
    using System.Linq;
    using System.Threading.Tasks;

    using K8GatherBotv2.Locales;

    /// <summary>
    /// The gather.
    /// </summary>
    public partial class Gather
    {
        /// <summary>
        /// Tries to set the <paramref name="player"/> ready.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<TaskResult> TrySetReady(Player player)
        {
            if (!this.Queue.Any())
            {
                return new TaskResult(false, ErrorCode.PickPhaseInProgress, Keys.PickPhaseAlreadyInProcess);

            }

            if (!this.Queue.Contains(player))
            {
                return new TaskResult(false, ErrorCode.PlayerNotInQueue, Keys.ReadyPhaseNotInQueue);
            }

            if (player.Ready)
            {
                return new TaskResult(false, ErrorCode.PlayerAlreadyReady, Keys.ReadyPhaseAlreadyMarkedReady);
            }

            player.Ready = true;

            if (this.Queue.All(p => p.Ready))
            {
                await this.BeginPickPhase();
            }

            return new TaskResult(true, ErrorCode.None, Keys.ReadyPhaseReady);
        }
    }
}
