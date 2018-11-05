namespace K8GatherBotv2.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using K8GatherBotv2.Extensions;
    using K8GatherBotv2.Locales;

    /// <summary>
    /// The gather.
    /// </summary>
    public partial class Gather
    {
        /// <summary>
        /// The messenger
        /// </summary>
        private readonly IMessenger messenger;

        /// <summary>
        /// The settings
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// The queue
        /// </summary>
        private readonly List<Player> queue;

        /// <summary>
        /// The gather number
        /// </summary>
        private int gatherNumber;

        /// <summary>
        /// The ready check timer
        /// </summary>
        private Timer readyCheckTimer;

        /// <summary>
        /// The ready check automatic event
        /// </summary>
        private AutoResetEvent readyCheckAutoEvent;

        /// <summary>
        /// The ready check tick counter
        /// </summary>
        private int readyCheckTickCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gather"/> class.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="gathersPlayed">The gathers played.</param>
        public Gather(IMessenger messenger, ISettings settings, int gathersPlayed)
        {
            this.messenger = messenger;
            this.settings = settings;
            this.gatherNumber = gathersPlayed;
            this.queue = new List<Player>(this.settings.QueueSize);
            this.Game = new Game(this.gatherNumber++, this.settings);
            this.PickTurn = this.Game.Teams.First();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is full.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is full; otherwise, <c>false</c>.
        /// </value>
        public bool IsFull => this.Queue.Count == this.settings.QueueSize;

        /// <summary>
        /// Gets the queue.
        /// </summary>
        /// <value>
        /// The queue.
        /// </value>
        public IReadOnlyList<Player> Queue => this.queue.AsReadOnly();

        /// <summary>
        /// Gets or sets the pick turn.
        /// </summary>
        /// <value>
        /// The pick turn.
        /// </value>
        public Team PickTurn { get; set; }

        /// <summary>
        /// Gets the game.
        /// </summary>
        /// <value>
        /// The game.
        /// </value>
        public Game Game { get; private set; }

        /// <summary>
        /// Players the list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> PlayerList()
        {
            var count = 0;
            return this.queue.Select(item => string.Format("{2}{0} - {1}{2}", count++, item.Id, item.Team != null ? "~~" : string.Empty));
        }

        /// <summary>
        /// Begins the ready check.
        /// </summary>
        /// <returns></returns>
        public async Task BeginReadyCheck()
        {
            await Task.Delay(250);
            await this.messenger.SendMessage(this.settings.Localization[Keys.ReadyPhaseStarted] + " \n" + string.Join("\t", this.Queue.Select(p => p.Id)));
          
            // Timer init.
            this.readyCheckAutoEvent = new AutoResetEvent(false);
            this.readyCheckTimer = new Timer(this.ReadyCheckOnTick, this.readyCheckAutoEvent, 1000, 1000);

            Trace.WriteLine("RDYCHECK ACTIVATED --- " + DateTime.Now);
        }

        /// <summary>
        /// Begins the pick phase.
        /// </summary>
        /// <returns></returns>
        public async Task BeginPickPhase()
        {
            await Task.Delay(250);

            this.readyCheckTimer.Dispose();
            this.readyCheckTickCounter = 0;

            this.Game.Reset();

            var players = this.queue.ToArray();
            var allAreNewKids = players.All(p => p.IsNewKid);
            var captainCandidates = players.Where(p => allAreNewKids || !p.IsNewKid).ToList();

            foreach (var team in this.Game.Teams)
            {
                AssignCaptain(team, captainCandidates);
            }

            await this.settings.Data.AddCaptains(this.Game.Teams.Select(t => t.Captain));

            var formatString = this.settings.Localization[Keys.PickPhaseTeamXCaptain];
            var captainsString = string.Join("\n", this.Game.Teams.Select(t => string.Format(formatString, t.Name, "<@" + t.Captain.Id + ">")));

            await this.messenger.SendMessage(
                this.settings.Localization[Keys.PickPhaseStarted] + "\n"
                + captainsString + "\n" 
                + this.settings.Localization[Keys.PickPhaseInstructions] + "\n\n"
                + string.Join("\n", this.PlayerList()));
        }

        /// <summary>
        /// Begins the game.
        /// </summary>
        /// <returns></returns>
        public async Task BeginGame()
        {
            await Task.Delay(250);
            await this.settings.Data.AddHighScores(this.Game.Teams.SelectMany(t => t.Members));
            await this.messenger.SendFormattedMessage(
                this.settings.Localization[Keys.StatusPickedTeams],
                this.Game.Teams.Select(t => Tuple.Create($"{t.Name}: ", string.Join("\n", t.Members.Select(p => p.Name)))));

            this.Reset();
        }

        /// <summary>
        /// Readies the check on tick.
        /// </summary>
        /// <param name="stateInfo">The state information.</param>
        public async void ReadyCheckOnTick(object stateInfo)
        {
            if (this.readyCheckTickCounter < this.settings.Readytimer)
            {
                this.readyCheckTickCounter++;
                return;
            }

            // Timer is up, check who have not readied.
            var notReadyPlayers = this.Queue.Where(p => !p.Ready).ToArray();
            if (notReadyPlayers.Any())
            {
                foreach (var player in notReadyPlayers)
                {
                    var taskResult = await this.TryRemove(player, true);
                    if (!taskResult.Success)
                    {
                        taskResult.Log();
                    }

                    Trace.WriteLine("RDYCHECK-REMOVED --- " + player.Id);
                }

                foreach (var player in this.Queue)
                {
                    player.Ready = false;
                }


                Trace.WriteLine(DateTime.Now + $" #! NOT READY ANNOUNCE START !#");
                // Wait for the shard to end before closing the program.
                await this.messenger.SendMessage(this.settings.Localization[Keys.ReadyPhaseTimeout]);
                Trace.WriteLine(DateTime.Now + $" -- Attempted announce message -------------------------------------------");
                Trace.WriteLine(DateTime.Now + $" #! NOT READY ANNOUNCE END !#");
            }

            // Dispose of the current timer.
            this.readyCheckTimer.Dispose();
            this.readyCheckTickCounter = 0;

            Trace.WriteLine("RDYCHECK EXPIRED --- " + DateTime.Now);
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        private void Reset()
        {
            this.Game.Start();
         
            foreach (var player in this.Queue)
            {
                player.Ready = false;
            }

            this.queue.Clear();
            this.Game = new Game(this.gatherNumber++, this.settings);
        }

        /// <summary>
        /// Assigns the captain.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <param name="candidates">The candidates.</param>
        private static void AssignCaptain(Team team, List<Player> candidates)
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            var index = rnd.Next(candidates.Count);

            team.Captain = candidates.ToArray()[index];
            candidates.Remove(team.Captain);
        }
    }
}
