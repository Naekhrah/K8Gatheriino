namespace K8GatherBotv2.Domain
{
    using K8GatherBotv2.Locales;

    /// <summary>
    /// The task result.
    /// </summary>
    public class TaskResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskResult"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="code">The code.</param>
        /// <param name="key">The key.</param>
        public TaskResult(bool success, ErrorCode code, Keys key)
        {
            this.Success = success;
            this.Code = code;
            this.Key = key;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="TaskResult"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public ErrorCode Code { get; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Keys Key { get; }
    }
}
