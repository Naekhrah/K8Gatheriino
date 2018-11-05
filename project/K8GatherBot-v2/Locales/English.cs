namespace K8GatherBotv2.Locales
{
    using System.Collections.Generic;

    /// <summary>
    /// The english localization.
    /// </summary>
    /// <seealso cref="ILocalization" />
    public class English : ILocalization
    {
        private readonly Dictionary<Keys, string> localizations;

        /// <summary>
        /// Initializes a new instance of the <see cref="English"/> class.
        /// </summary>
        public English()
        {
            this.localizations = new Dictionary<Keys, string>
                {
                    { Keys.Ok, "Ok." },
                    { Keys.PickPhaseAlreadyInProcess, "Please wait until the previous queue is handled." },
                    { Keys.QueuePhaseAdded, "Added!" },
                    { Keys.ReadyPhaseStarted, "Queue is now full, proceed to mark yourself ready with ***!ready*** \n You have 60 seconds!" },
                    { Keys.QueuePhaseAlreadyInQueue, "You're already in the queue!" },
                    { Keys.PickPhaseCannotRemove, "Too late to back down now! Wait until the queue is handled." },
                    { Keys.QueuePhaseRemoved, "Removed!" },
                    { Keys.QueuePhaseNotInQueue, "You are not in the queue right now." },
                    { Keys.QueuePhaseNotReadyYet, "Queue is not finished yet!" },
                    { Keys.ReadyPhaseReady, "Ready!" },
                    { Keys.PickPhaseStarted, "Readycheck complete, starting picking phase! First picking turn: Team 1" },
                    { Keys.PickPhaseTeamXCaptain, "{0} Captain: {1}" },
                    { Keys.PickPhaseInstructions, "Pick players using ***!pick NUMBER***" },
                    { Keys.ReadyPhaseNotInQueue, "It seems you're not in the queue." },
                    { Keys.PickPhasePlayedAdded, "Player added to {0}!\n {1} Turn to pick!" },
                    { Keys.PickPhaseUnpicked, "***Remaining players:***" },
                    { Keys.PickPhaseAlreadyPicked, "That player is already in a team!" },
                    { Keys.PickPhaseUnknownIndex, "Couldn't place a player with that index" },
                    { Keys.PickPhaseEveryoneNotReady, "All players are not yet ready!" },
                    { Keys.PickPhaseNotYourTurn, "Not your turn to pick right now!" },
                    { Keys.PickPhaseNotCaptain, "You are not the captain of either teams. Picking is restricted to captains." },
                    { Keys.QueuePhaseEmptyQueue, "Nobody in the queue! use ***!add***  to start queue!" },
                    { Keys.AdminResetSuccessful, "All lists emptied successfully." },
                    { Keys.StatusPickedTeams, "Selected teams" },
                    { Keys.StatusQueueStatus, "current queue" },
                    { Keys.InfoPurposeAnswer, "Get people to gather and play together" },
                    { Keys.InfoFunFactAnswer, "Only a droplet of coffee was used to develop this bot. :thinking:" },
                    { Keys.InfoDeveloper, "Developer" },
                    { Keys.InfoPurpose, "Purpose" },
                    { Keys.InfoFunFact, "Fun fact" },
                    { Keys.InfoCommands, "Commands" },
                    { Keys.StatusQueuePlayers, "Players" },
                    { Keys.StatusNotReady, "NOT READY YET:" },
                    { Keys.ReadyPhaseTimeout, "Not all players were ready during the readycheck. Returning to queue with players that were ready." },
                    { Keys.ReadyPhaseCannotAdd, "Wait until the picking phase is over." },
                    { Keys.ReadyPhaseAlreadyMarkedReady, "You have already readied!" },
                    { Keys.FatKidHeader, "Fat Kid" },
                    { Keys.FatKidTop10, "Top 10 Fat Kids" },
                    { Keys.FatKidStatusSingle, "{0} has been the fat kid {1} times ({2}/{3})" },
                    { Keys.HighScoresHeader, "Games played" },
                    { Keys.HighScoresTop10, "Top 10 Gathering LEGENDS" },
                    { Keys.HighScoresStatusSingle, "{0} has played {1} games ({2}/{3})" },
                    { Keys.ThinKidHeader, "Thin kid" },
                    { Keys.ThinKidTop10, "Top10 Thin Kids" },
                    { Keys.ThinKidStatusSingle, "{0} has been the thin kid {1} times ({2}/{3}" },
                    { Keys.CaptainHeader, "Captain" },
                    { Keys.CaptainTop10, "Top10 Captains" },
                    { Keys.CaptainStatusSingle, "{0} has been selected captain {1} times ({2}/{3}" },
                    { Keys.PlayerStats, "Player statistics" },
                    { Keys.RelinqPickPhaseStarted, "You have already picked a player, too late to drop out of picking phase" },
                };
        }

        /// <inheritdoc />
        public string this[Keys key] => this.localizations.TryGetValue(key, out var text) ? text : key.ToString();
    }
}
