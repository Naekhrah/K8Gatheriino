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
        /// Tries to remove the <paramref name="player"/> from gather.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<TaskResult> TryRemove(Player player, bool force)
        {
            if (!force && (this.IsFull || this.Queue.Any(p => p.Team != null)))
            {
                return new TaskResult(false, ErrorCode.PickPhaseInProgress, Keys.PickPhaseCannotRemove);
            }

            if (!this.Queue.Contains(player))
            {
                return new TaskResult(false, ErrorCode.NotInQueue, Keys.QueuePhaseNotInQueue);
            }

            this.queue.Remove(player);
            await this.messenger.SendMessage($"<@!{player.Id}> " + this.settings.Localization[Keys.QueuePhaseRemoved] + " " + this.Queue.Count + "/" + this.settings.QueueSize);

            return new TaskResult(true, ErrorCode.None, Keys.QueuePhaseRemoved);
        }
    }
}
