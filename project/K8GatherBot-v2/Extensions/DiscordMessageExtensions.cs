namespace K8GatherBotv2.Extensions
{
    using System;

    using Discore;

    /// <summary>
    /// The discord message extensions.
    /// </summary>
    public static class DiscordMessageExtensions
    {
        /// <summary>
        /// Parses the identifier and username.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The <see cref="Tuple{T1, T2}"/>.</returns>
        public static Tuple<string, string> ParseIdAndUsername(this DiscordMessage message)
        {
            if (message == null) 
            {
                return Tuple.Create<string, string>(null, null);
            }

            string id = null;
            string userName = null;

            var msg = message.Content;
            if (message.Mentions.Count > 0)
            {
                id = message.Mentions[0].Id.Id.ToString();
            }
            else if (msg.Split(null).Length < 2)
            {
                id = message.Author.Id.Id.ToString();
            }

            if (msg.Split(' ').Length > 1 && id == null)
            {
                userName = msg.Substring(msg.Split(' ')[0].Length + 1);
            }

            return Tuple.Create(id, userName);
        }
    }
}
