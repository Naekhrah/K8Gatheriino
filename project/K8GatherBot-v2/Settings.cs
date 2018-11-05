namespace K8GatherBotv2
{
    using System.IO;

    using K8GatherBotv2.Locales;

    using Newtonsoft.Json;

    /// <summary>
    /// The settings.
    /// </summary>
    public class Settings : ISettings
    { 
        /// <inheritdoc />
        [JsonIgnore]
        public int QueueSize => this.TeamCount * this.PlayersInTeam;

        /// <inheritdoc />
        public int TeamCount { get; set; }

        /// <inheritdoc />
        public int PlayersInTeam { get; set; }

        /// <inheritdoc />
        public int Readytimer { get; set; }

        /// <inheritdoc />
        public int CaptainThreshold { get; set; }

        /// <inheritdoc />
        public string Language { get; set; }

        /// <inheritdoc />
        public string AllowedChannel { get; set; }

        /// <inheritdoc />
        public string BotToken { get; set; }

        /// <inheritdoc />
        public string Version { get; set; }

        /// <inheritdoc />
        public ulong ChannelId { get; set; }

        /// <inheritdoc />
        public int NewKidThreshold { get; set; } = 10;  //2018-10, Threshold of games played until you can become captain, defaults at 10, overridden by appsettings.json.

        /// <inheritdoc />
        public int GiveupThreshold { get; set; } = 100; //2018-10, Threshold to prevent infinite loop, 100 should be sufficient always.

        /// <inheritdoc />
        [JsonIgnore]
        public IMessenger Messenger { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public ILocalization Localization { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public IData Data { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            var serializer = new JsonSerializer();
            using (var textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, this);
                return textWriter.ToString();

            }
        }
    }
}
