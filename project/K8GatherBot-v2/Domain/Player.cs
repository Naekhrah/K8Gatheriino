namespace K8GatherBotv2.Domain
{
    using System;

    /// <summary>
    /// The player.
    /// </summary>
    /// <seealso cref="System.IEquatable{T}" />
    public class Player : IEquatable<Player>
    {
        /// <summary>
        /// The team
        /// </summary>
        private Team team;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public Player(string id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.Team = null;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the team.
        /// </summary>
        /// <value>
        /// The team.
        /// </value>
        public Team Team
        {
            get
            {
                return this.team;
            }
            set
            {
                this.team = value;
                this.team?.Members.Add(this);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is new kid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is new kid; otherwise, <c>false</c>.
        /// </value>
        public bool IsNewKid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Player"/> is ready.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ready; otherwise, <c>false</c>.
        /// </value>
        public bool Ready { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Player other)
        {
            return string.Equals(this.Id, other.Id);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Player player && this.Equals(player);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Id != null ? this.Id.GetHashCode() : 0;
        }
    }
}
