namespace K8GatherBotv2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Discore;
    using Discore.Http;

    /// <summary>
    /// The channel messenger.
    /// </summary>
    /// <seealso cref="IMessenger" />
    public class ChannelMessenger : IMessenger
    {
        /// <summary>
        /// The message factory
        /// </summary>
        private readonly MessageFactory messageFactory;

        /// <summary>
        /// The HTTP
        /// </summary>
        private readonly DiscordHttpClient http;

        /// <summary>
        /// The snowflake
        /// </summary>
        private readonly Snowflake snowflake;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelMessenger"/> class.
        /// </summary>
        /// <param name="messageFactory">The message factory.</param>
        /// <param name="snowFlake">The snow flake.</param>
        /// <param name="botToken">The bot token.</param>
        public ChannelMessenger(MessageFactory messageFactory, Snowflake snowFlake, string botToken)
        {
            this.messageFactory = messageFactory;
            this.snowflake = snowFlake;
            this.http = new DiscordHttpClient(botToken);
        }

        /// <inheritdoc />
        public async Task SendMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            Trace.WriteLine(message); 
            await this.http.CreateMessage(this.snowflake, message);
        }

        /// <inheritdoc />
        public async Task SendMessage(CreateMessageOptions createMessageOptions)
        {
            if (createMessageOptions == null)
            {
                return;
            }

            Trace.WriteLine(createMessageOptions.Content); 
            await this.http.CreateMessage(this.snowflake, createMessageOptions);
        }

        /// <inheritdoc />
        public async Task SendFormattedMessage(string title, IEnumerable<Tuple<string, string>> fields)
        {
            await this.SendMessage(this.messageFactory.GetFormattedMessage(title, fields));
        }
    }
}
