namespace K8GatherBotv2
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Discore.Http;

    /// <summary>
    /// The messenger interface.
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task SendMessage(string message);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="createMessageOptions">The create message options.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task SendMessage(CreateMessageOptions createMessageOptions);

        /// <summary>
        /// Sends the formatted message.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="fields">The fields</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task SendFormattedMessage(string title, IEnumerable<Tuple<string, string>> fields);
    }
}
