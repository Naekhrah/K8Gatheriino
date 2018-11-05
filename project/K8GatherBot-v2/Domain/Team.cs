namespace K8GatherBotv2.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// The team.
    /// </summary>
    public class Team
    {
        /// <summary>
        /// The captain
        /// </summary>
        private Player captain;

        /// <summary>
        /// Initializes a new instance of the <see cref="Team"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="maxSize">The maximum size.</param>
        public Team(string name, int maxSize)
        {
            this.Name = name;
            this.Members = new List<Player>(maxSize);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the captain.
        /// </summary>
        /// <value>
        /// The captain.
        /// </value>
        public Player Captain
        {
            get
            {
                return this.captain;
            }
            set
            {
                this.captain = value;

                if (this.captain != null)
                {
                    this.captain.Team = this;
                }
            }
        }

        /// <summary>
        /// Gets or sets the members.
        /// </summary>
        /// <value>
        /// The members.
        /// </value>
        public List<Player> Members { get; set; }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.Captain = null;
            this.Members.Clear();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
