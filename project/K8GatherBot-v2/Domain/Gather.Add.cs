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
        /// Tries to add <paramref name="player"/> to the gather.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<TaskResult> TryAdd(Player player)
        {
            if (this.Queue.Contains(player))
            {
                return new TaskResult(false, ErrorCode.PlayerAlreadyAdded, Keys.QueuePhaseAlreadyInQueue);
            }

            if (this.IsFull)
            {
                return new TaskResult(false, ErrorCode.QueueFull, Keys.PickPhaseAlreadyInProcess);
            }

            if (this.Queue.Any(p => p.Team != null))
            {
                return new TaskResult(false, ErrorCode.PickPhaseInProgress, Keys.ReadyPhaseCannotAdd);
            }

            player.Ready = false;
            this.queue.Add(player);
            
            // If queue complete, announce readychecks.
            if (this.IsFull)
            {
                await this.BeginReadyCheck();
            }

            return new TaskResult(true, ErrorCode.None, Keys.QueuePhaseAdded);
        }
    }
}
