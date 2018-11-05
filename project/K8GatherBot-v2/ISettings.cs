namespace K8GatherBotv2
{
    using K8GatherBotv2.Locales;

    /// <summary>
    /// The settings interface.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// The queu size.
        /// </summary>
        int QueueSize { get; }

        /// <summary>
        /// The teams count.
        /// </summary>
        int TeamCount { get; set; }

        /// <summary>
        /// The player in team.
        /// </summary>
        int PlayersInTeam { get; set; }

        /// <summary>
        /// The ready timer.
        /// </summary>
        int Readytimer { get; set; }

        /// <summary>
        /// The captain threshold.
        /// </summary>
        int CaptainThreshold { get; set; }

        /// <summary>
        /// The language.
        /// </summary>
        string Language { get; set; }

        /// <summary>
        /// The allowed channel.
        /// </summary>
        string AllowedChannel { get; set; }

        /// <summary>
        /// The bot token.
        /// </summary>
        string BotToken { get; set; }

        /// <summary>
        /// The version.
        /// </summary>
        string Version { get; set; }

        /// <summary>
        /// The channel id.
        /// </summary>
        ulong ChannelId { get; set; }

        /// <summary>
        /// Threshold of games played until you can become captain.
        /// </summary>
        int NewKidThreshold { get; set; }

        /// <summary>
        /// Threshold to prevent infinite loop, 100 should be sufficient always.
        /// </summary>
        int GiveupThreshold { get; set; }

        /// <summary>
        /// The messenger.
        /// </summary>
        IMessenger Messenger { get; set; }

        /// <summary>
        /// The localization interface.
        /// </summary>
        ILocalization Localization { get; set; }

        /// <summary>
        /// The data interface.
        /// </summary>
        IData Data { get; set; }
    }
}
