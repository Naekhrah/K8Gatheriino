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
        /// Tries to add <paramref name="index"/> player to the team of the <paramref name="captain"/>.
        /// </summary>
        /// <param name="captain">The captain.</param>
        /// <param name="index">The index.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<TaskResult> TryPick(Player captain, int index)
        {
            // verify message sender
            if (!object.Equals(captain, this.PickTurn.Captain))
            {
                return this.Game.Teams.Any(t => object.Equals(captain, t.Captain)) 
                    ? new TaskResult(false, ErrorCode.NotYourTurn, Keys.PickPhaseNotYourTurn) 
                    : new TaskResult(false, ErrorCode.NotCaptain, Keys.PickPhaseNotCaptain);
            }

            if (index < 0 || index > this.settings.QueueSize)
            {
                return new TaskResult(false, ErrorCode.UnknownIndex, Keys.PickPhaseUnknownIndex);
            }

            if (this.Queue.Any(p => !p.Ready))
            {
                return new TaskResult(false, ErrorCode.EveryoneNotReady, Keys.PickPhaseEveryoneNotReady);
            }

            var pickedPlayer = this.Queue.Skip(index).Take(1).First();
            if (pickedPlayer.Team != null)
            {
                return new TaskResult(false, ErrorCode.AlreadyPicked, Keys.PickPhaseAlreadyPicked);
            }

            pickedPlayer.Team = captain.Team;

            // add thin kid (the first pick)
            if (this.Queue.Count(p => p.Team != null) == 3)
            {
                await this.settings.Data.AddThinKid(pickedPlayer);
            }

            var oldTurn = this.PickTurn;

            Team newPickTurn = null;
            foreach (var team in this.Game.Teams.Reverse())
            {
                if (object.Equals(this.PickTurn, team))
                {
                    this.PickTurn = newPickTurn;
                    break;
                }

                newPickTurn = team;
            }

            if (this.PickTurn == null)
            {
                this.PickTurn = this.Game.Teams.First();
            }

            var unpickedPlayers = this.Queue.Where(p => p.Team == null).ToArray();
            if (unpickedPlayers.Length > 2)
            {
                var playedAdded = string.Format(this.settings.Localization[Keys.PickPhasePlayedAdded], oldTurn, this.PickTurn);
                await this.messenger.SendMessage($"<@{pickedPlayer.Id}> " + playedAdded + " <@" + this.PickTurn.Captain.Id + "> \n " + this.settings.Localization[Keys.PickPhaseUnpicked] + " \n" + string.Join("\n", this.PlayerList()));
            }
            else if (unpickedPlayers.Length == 1)
            {
                var fatKid = unpickedPlayers.Single();
                fatKid.Team = this.PickTurn;
                await this.settings.Data.AddFatKid(fatKid);
            }

            // if all players have been picked show the teams and reset bot status
            unpickedPlayers = this.Queue.Where(p => p.Team == null).ToArray();
            if (!unpickedPlayers.Any())
            {
                await this.BeginGame();
            }

            return new TaskResult(true, ErrorCode.None, Keys.PickPhasePlayedAdded);
        }
    }
}
