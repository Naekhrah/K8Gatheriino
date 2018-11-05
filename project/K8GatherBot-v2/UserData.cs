namespace K8GatherBotv2
{
    using System;

    /// <summary>
    /// The user data.
    /// </summary>
    /// <seealso cref="System.IComparable{T}" />
    /// <seealso cref="System.IEquatable{T}" />
    public class UserData : IComparable<UserData>, IEquatable<UserData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserData"/> class.
        /// </summary>
        public UserData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserData"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="username">The username.</param>
        public UserData(string id, string username)
        {
            this.Id = id;
            this.UserName = username;
            this.Count = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserData"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="count">The count.</param>
        public UserData(string id, string username, int count)
        {
            this.Id = id;
            this.UserName = username;
            this.Count = count;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; set; }

        /// <inheritdoc />
        public int CompareTo(UserData other)
        {
            return this.Count == other.Count 
                ? string.Compare(this.UserName, other.UserName, StringComparison.OrdinalIgnoreCase) 
                : other.Count.CompareTo(this.Count);
        }

        /// <inheritdoc />
        public bool Equals(UserData other)
        {
            return this.Id.Equals(other.Id);
        }

        /// <summary>
        /// Adds this instance.
        /// </summary>
        internal void Add()
        {
            this.Count++;
        }

        /// <summary>
        /// Minuses this instance.
        /// </summary>
        internal void Minus()
        {
            this.Count--;
        }
    }
}
