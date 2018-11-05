namespace K8GatherBotv2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Discore;
    using Discore.Http;

    using K8GatherBotv2.Domain;
    using K8GatherBotv2.Locales;

    /// <summary>
    /// The message factory.
    /// </summary>
    public class MessageFactory
    {
        private readonly ISettings settings;
        private readonly IData data;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageFactory"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public MessageFactory(ISettings settings)
        {
            this.settings = settings;
            this.data = settings.Data;
        }

        /// <summary>
        /// Gets the stats message.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<CreateMessageOptions> GetStatsMessage(Player player)
        {
            try
            {
                Trace.WriteLine($"!Getting stats for user {player.Id} '{player.Name}'");

                var idUsername = Tuple.Create(player.Id, player.Name);
                var highScoreStats = await this.data.GetHighScoreInfo(player, this.settings.Localization[Keys.HighScoresStatusSingle]);
                var fatkidStats = await this.data.GetFatKidInfo(player, this.settings.Localization[Keys.FatKidStatusSingle]);
                var captainStats = await this.data.GetCaptainInfo(player, this.settings.Localization[Keys.CaptainStatusSingle]);
                var thinkidStats = await this.data.GetThinKidInfo(player, this.settings.Localization[Keys.ThinKidStatusSingle]);

                var fields = new[]
                    {
                        Tuple.Create(this.settings.Localization[Keys.HighScoresHeader], highScoreStats),
                        Tuple.Create(this.settings.Localization[Keys.CaptainHeader], captainStats),
                        Tuple.Create(this.settings.Localization[Keys.ThinKidHeader], thinkidStats),
                        Tuple.Create(this.settings.Localization[Keys.FatKidHeader], fatkidStats)
                    };

                return this.GetFormattedMessage(this.settings.Localization[Keys.PlayerStats] + ": " + idUsername.Item2, fields);
            }
            catch (Exception e)
            {
                Trace.WriteLine($"!gatherinfo - EX - {player.Name} - {player.Id} --- {DateTime.Now}");
                Trace.WriteLine("!#DEBUG INFO FOR ERROR: " + e);
            }

            return null;
        }

        /// <summary>
        /// Gets the fat kid message.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<string> GetFatKidMessage(Player player)
        {
            try
            {
                var idUsername = Tuple.Create(player.Id, player.Name);
                Trace.WriteLine("fatkid name split resulted in " + idUsername);
                return await this.settings.Data.GetFatKidInfo(player, this.settings.Localization[Keys.FatKidStatusSingle]);
            }
            catch (Exception e)
            {
                Trace.WriteLine("EX-!fatkid" + " --- " + DateTime.Now);
                Trace.WriteLine("!#DEBUG INFO FOR ERROR: " + e);
            }

            return null;
        }

        /// <summary>
        /// Gets the thin kid message.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<string> GetThinKidMessage(Player player)
        {
            try
            {
                var idUsername = Tuple.Create(player.Id, player.Name);
                Trace.WriteLine("thinkid name split resulted in " + idUsername);
                return await this.settings.Data.GetThinKidInfo(player, this.settings.Localization[Keys.ThinKidStatusSingle]);
            }
            catch (Exception e)
            {
                Trace.WriteLine("EX-!thinkid" + " --- " + DateTime.Now);
                Trace.WriteLine("!#DEBUG INFO FOR ERROR: " + e);
            }

            return null;
        }

        /// <summary>
        /// Gets the captain message.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<string> GetCaptainMessage(Player player)
        {
            try
            {
                var idUsername = Tuple.Create(player.Id, player.Name);
                Trace.WriteLine("captain name split resulted in " + idUsername);
                return await this.settings.Data.GetCaptainInfo(player, this.settings.Localization[Keys.CaptainStatusSingle]);
            }
            catch (Exception e)
            {
                Trace.WriteLine("EX-!captain" + " --- " + DateTime.Now);
                Trace.WriteLine("!#DEBUG INFO FOR ERROR: " + e);
            }

            return null;
        }

        /// <summary>
        /// Gets the high score message.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<string> GetHighScoreMessage(Player player)
        {
            try
            {
                Trace.WriteLine("fatkid name split resulted in " + player);
                return await this.settings.Data.GetHighScoreInfo(player, this.settings.Localization[Keys.HighScoresStatusSingle]);
            }
            catch (Exception e)
            {
                Trace.WriteLine("EX-!hs" + " --- " + DateTime.Now);
                Trace.WriteLine("!#DEBUG INFO FOR ERROR: " + e);
            }

            return null;
        }
        /// <summary>
        /// Gets the fat top ten message.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<CreateMessageOptions> GetFatTopTenMessage()
        {
            try
            {
                var fatKidTop10 = await this.settings.Data.GetFatKidTop10();
                return this.GetFormattedMessage(Keys.FatKidTop10, fatKidTop10);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EX-!f10 --- " + DateTime.Now);
                Trace.WriteLine("!#DEBUG INFO FOR ERROR: " + ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the top ten message.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<CreateMessageOptions> GetTopTenMessage()
        {
            try
            {
                var highScoreTop10 = await this.settings.Data.GetHighScoreTop10();
                return this.GetFormattedMessage(Keys.HighScoresTop10, highScoreTop10);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EX-!top10 --- " + DateTime.Now);
                Trace.WriteLine("!#DEBUG INFO FOR ERROR: " + ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the captain top ten message.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<CreateMessageOptions> GetCaptainTopTenMessage()
        {
            try
            {
                var captainTop10 = await this.settings.Data.GetCaptainTop10();
                return this.GetFormattedMessage(Keys.HighScoresTop10, captainTop10);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EX-!c10 --- " + DateTime.Now);
                Trace.WriteLine("!#DEBUG INFO FOR ERROR: " + ex);
            }
            return null;
        }

        /// <summary>
        /// Gets the thin top ten message.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<CreateMessageOptions> GetThinTopTenMessage()
        {
            try
            {
                var thinKidTop10 = await this.settings.Data.GetThinKidTop10();
                return this.GetFormattedMessage(Keys.HighScoresTop10, thinKidTop10);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EX-!tk10 --- " + DateTime.Now);
                Trace.WriteLine("!#DEBUG INFO FOR ERROR: " + ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the formatted message.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="content">The content.</param>
        /// <returns>The <see cref="CreateMessageOptions"/>.</returns>
        public CreateMessageOptions GetFormattedMessage(Keys key, string content)
        {
            return this.GetFormattedMessage(null, new[] { Tuple.Create(this.settings.Localization[key], content) });
        }

        /// <summary>
        /// Gets the formatted message.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="fields">The fields.</param>
        /// <returns>The <see cref="CreateMessageOptions"/>.</returns>
        public CreateMessageOptions GetFormattedMessage(string title, IEnumerable<Tuple<string, string>> fields)
        {
            var embedOptions = new EmbedOptions()
                .SetTitle(string.IsNullOrEmpty(title) ? Constants.Title : $"{Constants.Title} {title}")
                .SetFooter(Constants.Footer + this.settings.Version)
                .SetColor(DiscordColor.FromHexadecimal(0xff9933));

            foreach(var field in fields)
            {
                embedOptions.AddField(field.Item1, field.Item2, true);
            }

            return new CreateMessageOptions().SetEmbed(embedOptions);
        }
    }
}