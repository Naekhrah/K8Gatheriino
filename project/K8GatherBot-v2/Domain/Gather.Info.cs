namespace K8GatherBotv2.Domain
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using K8GatherBotv2.Locales;

    /// <summary>
    /// The gather.
    /// </summary>
    public partial class Gather
    {
        /// <summary>
        /// Sends message of gather current status.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<TaskResult> Info(Player player)
        {
            try
            {
                await this.messenger.SendFormattedMessage(
                    null,
                    new[]
                        {
                            Tuple.Create(this.settings.Localization[Keys.InfoDeveloper] + " ", "kitsun8 & pirate_patch"),
                            Tuple.Create(this.settings.Localization[Keys.InfoPurpose] + " ", this.settings.Localization[Keys.InfoPurposeAnswer]),
                            Tuple.Create(this.settings.Localization[Keys.InfoFunFact] + " ", this.settings.Localization[Keys.InfoFunFactAnswer]),
                            Tuple.Create(this.settings.Localization[Keys.InfoCommands] + " ", "!add, !remove/rm, !ready/r, !pick/p, !gatherinfo/gi, !gstatus/gs, !resetbot, !f10/fat10, !fatkid, !top10/topten, !hs/highscore, !tk10, !thinkid, !c10, !captain, !relinquish")
                        });
                    
                Trace.WriteLine($"!gatherinfo - " + player.Name + "-" + player.Id + " --- " + DateTime.Now);
            }
            catch (Exception)
            {
                Trace.WriteLine($"!gatherinfo - EX -" + player.Name + "-" + player.Id + " --- " + DateTime.Now);
            }

            return new TaskResult(true, ErrorCode.None, Keys.Ok);
        }
    }
}
