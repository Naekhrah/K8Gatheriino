namespace K8GatherBotv2.Domain
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using K8GatherBotv2.Locales;

    /// <summary>
    /// The gather.
    /// </summary>
    public partial class Gather
    {
        /// <summary>
        /// Gets the gather status.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<TaskResult> Status(Player player)
        {
            try
            {
                if (this.Queue.Any())
                {
                    if (this.IsFull)
                    {
                        //compare readycheck list to queue, print out those who are not ready
                        var readyCount = this.Queue.Count(p => p.Ready);
                        var notinlist = this.Queue.Where(p => !p.Ready).Select(p => p.Name);

                        //full queue, ready phase
                        await this.messenger.SendFormattedMessage(
                            " readycheck  " + "(" + readyCount + "/" + this.settings.QueueSize + ")",
                            new[] { Tuple.Create(this.settings.Localization[Keys.StatusQueuePlayers] + " ", string.Join("\n", notinlist)) });
                    }
                    else
                    {
                        if (this.Queue.Any(p => p.Team != null))
                        {
                            //picking phase
                            await this.messenger.SendFormattedMessage(
                                this.settings.Localization[Keys.StatusPickedTeams],
                                this.Game.Teams.Select(t => Tuple.Create($"{t.Name}: ", string.Join("\n", t.Members.Select(p => p.Name)))));
                        }
                        else
                        {
                            //queue phase
                            await this.messenger.SendFormattedMessage(
                                this.settings.Localization[Keys.StatusQueueStatus] + " " + "(" + this.Queue.Count + "/" + this.settings.QueueSize + ")",
                                new [] { Tuple.Create(this.settings.Localization[Keys.StatusQueueStatus] + " ", string.Join("\n", this.Queue.Select(p => p.Name))) });
                        }
                    }

                    Trace.WriteLine("!status");
                    return new TaskResult(true, ErrorCode.None, Keys.Ok);
                }

                await this.messenger.SendMessage($"<@!{player.Id}> " + this.settings.Localization[Keys.QueuePhaseEmptyQueue]);
                Trace.WriteLine("!status" + " --- " + DateTime.Now);

                return new TaskResult(true, ErrorCode.QueueIsEmpty, Keys.QueuePhaseEmptyQueue);
            }
            catch (Exception)
            {
                Trace.WriteLine("EX-!status" + " --- " + DateTime.Now);
                return new TaskResult(true, ErrorCode.QueueIsEmpty, Keys.QueuePhaseEmptyQueue);
            }
        }
    }
}
