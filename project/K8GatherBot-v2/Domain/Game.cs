namespace K8GatherBotv2.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The game.
    /// </summary>
    public class Game
    {
        private readonly ISettings settings;
        private readonly List<Team> teams;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="settings">The settings.</param>
        public Game(int number, ISettings settings)
        {
            this.Number = number;
            this.settings = settings;
            this.teams = new List<Team>(this.settings.TeamCount);

            for (var i = 1; i <= this.settings.TeamCount; i++)
            {
                this.teams.Add(new Team($"Team {i}", this.settings.PlayersInTeam));
            }
        }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        public int Number { get; set; }

        /// <summary>
        /// Gets the teams.
        /// </summary>
        /// <value>
        /// The teams.
        /// </value>
        public IReadOnlyList<Team> Teams => this.teams.AsReadOnly();

        /// <summary>
        /// Gets or sets the thin kid.
        /// </summary>
        /// <value>
        /// The thin kid.
        /// </value>
        public Player ThinKid { get; set; }

        /// <summary>
        /// Gets or sets the fat kid.
        /// </summary>
        /// <value>
        /// The fat kid.
        /// </value>
        public Player FatKid { get; set; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (this.teams.Any(t => t.Members.Count < this.settings.PlayersInTeam))
            {
                return;
            }

            this.ThinKid = this.teams.First().Members.Skip(1).Take(1).Single();
            this.FatKid = this.teams.Last().Members.Skip(this.settings.PlayersInTeam - 1).Take(1).Single();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.teams.ForEach(t => t.Reset());

            this.ThinKid = null;
            this.FatKid = null;
        }
    }
}
