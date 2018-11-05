namespace K8GatherBotv2
{
    using Discore.WebSocket;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    using K8GatherBotv2.Domain;
    using K8GatherBotv2.Extensions;
    using K8GatherBotv2.Locales;

    using Newtonsoft.Json;

    /// <summary>
    /// The main program.
    /// </summary>
    public class Program
    {
        private readonly PlayerCache knownPlayers;
        private readonly ISettings settings;
        private readonly IMessenger messenger;
        private readonly MessageFactory messageFactory;
        private readonly Gather gather;

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        public Program()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            this.settings = Init();
            this.messageFactory = new MessageFactory(this.settings);
            this.messenger = new ChannelMessenger(this.messageFactory, this.settings.ChannelId, this.settings.BotToken);
            this.settings.Messenger = this.messenger;
            this.knownPlayers = new PlayerCache(this.settings);
            this.gather = new Gather(this.messenger, this.settings, 0);
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {
            using (var shard = new Shard(this.settings.BotToken, 0, 1))
            {
                shard.Gateway.OnMessageCreated += this.Gateway_OnMessageCreated;
                shard.OnConnected += Shard_OnConnected;
                shard.OnReconnected += Shard_OnReconnected;
                shard.OnFailure += Shard_OnFailure;

                await shard.StartAsync();
                Trace.WriteLine(DateTime.Now + $" -- kitsun8's GatherBot Started \n -------------------------------------------");
                await shard.WaitUntilStoppedAsync();
            }
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            var program = new Program();
            program.Run().Wait();
            Trace.WriteLine("#! Reached END OF PROGRAM !#");
        }

        /// <summary>
        /// Handles the OnConnected event of the Shard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ShardEventArgs"/> instance containing the event data.</param>
        private static void Shard_OnConnected(object sender, ShardEventArgs e)
        {
            Trace.WriteLine(DateTime.Now + $" -- #! SHARD CONNECTED !# ----------------------------------------");
        }

        /// <summary>
        /// Handles the OnReconnected event of the Shard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ShardEventArgs"/> instance containing the event data.</param>
        private static void Shard_OnReconnected(object sender, ShardEventArgs e)
        {
            Trace.WriteLine(DateTime.Now + $" -- #! SHARD RECONNECTED !# ----------------------------------------");
        }

        /// <summary>
        /// Handles the OnFailure event of the Shard control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ShardFailureEventArgs"/> instance containing the event data.</param>
        private static void Shard_OnFailure(object sender, ShardFailureEventArgs e)
        {
            switch (e.Reason)
            {
                case ShardFailureReason.Unknown:
                    Trace.WriteLine(DateTime.Now + $" -- #! SHARD UNKNOWN ERROR !# ----------------------------------------");
                    break;

                case ShardFailureReason.ShardInvalid:
                    Trace.WriteLine(DateTime.Now + $" -- #! SHARD INVALID ERROR !# ----------------------------------------");
                    break;

                case ShardFailureReason.ShardingRequired:
                    Trace.WriteLine(DateTime.Now + $" -- #! SHARDING REQUIRED ERROR !# ----------------------------------------");
                    break;

                case ShardFailureReason.AuthenticationFailed:
                    Trace.WriteLine(DateTime.Now + $" -- #! SHARD AUTH ERROR !# ----------------------------------------");
                    break;
            }

            // Shard has failed, need to Run whole auth again!
            // EXPERIMENTAL! Trying to get the existing shard and stop it first (check if this actually exits the whole program), then run another.. Run.
            var shard = e.Shard;
            shard.StopAsync().Wait();

            var program = new Program();
            program.Run().Wait();
        }

        /// <summary>
        /// Handles the OnMessageCreated event of the Gateway control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MessageEventArgs"/> instance containing the event data.</param>
        private async void Gateway_OnMessageCreated(object sender, MessageEventArgs e)
        {
            // Ignore messages created by our bot.
            if (e.Message.Author.Id == e.Shard.UserId)
            {
                return;
            }

            // Prevent DM abuse, only react to messages sent on a set channel.
            if (e.Message.ChannelId.Id != this.settings.ChannelId)
            {
                return;
            }

            var command = e.Message.Content.ToLower().Split(' ')[0].AsCommand();
            var player = await this.knownPlayers.GetPlayer(e.Message);
            TaskResult taskResult = null;

            switch (command)
            {
                case Command.Add:
                    taskResult = await this.gather.TryAdd(player);
                    break;

                case Command.Remove:
                    taskResult = await this.gather.TryRemove(player, false);
                    break;

                case Command.Ready:
                    taskResult = await this.gather.TrySetReady(player);
                    break;

                //case Command.Wimp:
                //    await this.messenger.SendMessage($"<@!{message.Author.Id}>" + " Äijä hei. Älä viitti. Yritä edes. :-)");
                //await CmdRelinquishCaptainship(shard, message);
                //break;

                case Command.Pick:
                    int index;
                    try
                    {
                        if (!int.TryParse(e.Message.Content.Split()[1].Trim(), out index))
                        {
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"Exception in parsing pick index: {ex.Message}");
                        return;
                    }

                    taskResult = await this.gather.TryPick(player, index);
                    break;

                case Command.FatTopTen:
                    await this.messenger.SendMessage(await this.messageFactory.GetFatTopTenMessage());
                    break;

                case Command.ThinTopTen:
                    await this.messenger.SendMessage(await this.messageFactory.GetThinTopTenMessage());
                    break;

                case Command.TopTen:
                    await this.messenger.SendMessage(await this.messageFactory.GetTopTenMessage());
                    break;

                case Command.Stats:
                    await this.messenger.SendMessage(await this.messageFactory.GetStatsMessage(player));
                    break;

                case Command.FatKid:
                    await this.messenger.SendMessage(await this.messageFactory.GetFatKidMessage(player));
                    break;

                case Command.HighScore:
                    await this.messenger.SendMessage(await this.messageFactory.GetHighScoreMessage(player));
                    break;

                case Command.ThinKid:
                    await this.messenger.SendMessage(await this.messageFactory.GetThinKidMessage(player));
                    break;

                case Command.Captain:
                    await this.messenger.SendMessage(await this.messageFactory.GetCaptainMessage(player));
                    break;

                case Command.CaptainTopTen:
                    await this.messenger.SendMessage(await this.messageFactory.GetCaptainTopTenMessage());
                    break;

                case Command.Status:
                    taskResult = await this.gather.Status(player);
                    break;

                case Command.Reset:
                    taskResult = await this.gather.TryReset(player);
                    break;

                case Command.Info:
                    taskResult = await this.gather.Info(player);
                    break;

                default:
                    return;
            }

            if (taskResult != null)
            {
                await this.messenger.SendMessage($"<@!{player.Id}> {this.settings.Localization[taskResult.Key]} {this.gather.Queue.Count}/{this.settings.QueueSize}");

                if (!taskResult.Success)
                {
                    taskResult.Log();
                }
            }

            Trace.WriteLine($"Command executed, gather status: {this.gather.Queue.Count}/{this.settings.QueueSize}");
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>The <see cref="Settings"/>.</returns>
        /// <exception cref="Exception">Thrown if appsettings.json parsing fails.</exception>
        private static Settings Init()
        {
            // Get settings and GO.
            Trace.WriteLine("Reading settings from appsettings.json");
            Settings settings;
            using (var file = File.OpenText(@"appsettings.json"))
            {
                var jsonSerializer = new JsonSerializer();
                settings = jsonSerializer.Deserialize(file, typeof(Settings)) as Settings;
            }

            if (settings == null)
            {
                throw new Exception($"{nameof(settings)} is null after de-serializing json.");
            }

            switch (settings.Language)
            {
                case "fi":
                    settings.Localization = new Finnish();
                    break;

                default:
                    settings.Localization = new English();
                    break;

            }

            settings.Data = new PersistedData();
            settings.ChannelId = Convert.ToUInt64(settings.AllowedChannel);
            Trace.WriteLine(settings.ToString());

            return settings;
        }

        // TODO: WIMP?
        //private async Task CmdRelinquishCaptainship(Shard shard, DiscordMessage message)
        //{

        //    var authorId = message.Author.Id.Id.ToString();
        //    var authorUserName = message.Author.Username;

        //    if (ProgHelpers.captain1id == "" && ProgHelpers.captain2id == "")
        //    {
        //        // no captains
        //        return;
        //    }
        //    var team = ""; //27.1.2018 - default value was "team1", testing empty variable as default might prevent possible bugs involving a non-captain doing !wimp.
        //    if (ProgHelpers.captain1id.Equals(authorId))
        //    {
        //        team = "team1";
        //    }
        //    else if (ProgHelpers.captain2id.Equals(authorId))
        //    {
        //        team = "team2";
        //    }
        //    if (!team.Equals(""))
        //    {
        //        this.ChangeCaptain(team, authorId, authorUserName);
        //    }
        //    else
        //    {
        //        return; //27.1.2018 Just return if !wimp team variable doesn't result to anything, we don't need to announce anything because of non-captains doing !wimp
        //    }
        //}

        //private void ChangeCaptain(string team, string authorId, string authorUseName)
        //{
        //    if ((team.Equals("team1") && ProgHelpers.team1ids.Count > 1)
        //        || (team.Equals("team2") && ProgHelpers.team2ids.Count > 1))
        //    {
        //        http.CreateMessage($"<@{authorId}> " + this.settings.Localization[Keys.RelinqPickPhaseStarted]);
        //        return;
        //    }
        //    var rnd = new Random(); //Random a new captain
        //    var newCap = rnd.Next(ProgHelpers.queueids.Count); //Rnd index from the current playerpool (-2 of total players)
        //    var c1n = "";
        //    var c1i = "";
        //    c1n = ProgHelpers.queue[newCap];
        //    c1i = ProgHelpers.queueids[newCap];

        //    ProgHelpers.queue[newCap] = authorUseName; //Place the old captain in place of the new captain in the playerpool
        //    ProgHelpers.queueids[newCap] = authorId; //Place the old captain in place of the new captain in the playerpool
        //    var draftIndex = ProgHelpers.draftchatids.IndexOf(c1i);
        //    ProgHelpers.draftchatnames[draftIndex] = newCap + " - " + authorUseName;
        //    ProgHelpers.draftchatids[draftIndex] = authorId;

        //    var nextTeam = team;
        //    if (ProgHelpers.pickturn == authorId)
        //    {
        //        nextTeam = team.Equals("team1") ? "team2" : "team1"; //Find out which team is picking
        //        //ProgHelpers.pickturn = authorId; -- This was most likely a false statement
        //        ProgHelpers.pickturn = c1i; //Place new captain in the picking turn
        //    }

        //    await this.settings.Data.RemoveCaptain(authorId, authorUseName); //Statistics manipulation, old cap stats removed
        //    await this.settings.Data.AddCaptain(c1i, c1n); //Statistics manipulation, new cap gets stats

        //    if (team.Equals("team1"))
        //    {
        //        ProgHelpers.captain1 = c1n;
        //        ProgHelpers.captain1id = c1i;
        //        ProgHelpers.team1.Clear();
        //        ProgHelpers.team1ids.Clear();
        //        ProgHelpers.team1.Add(c1n);
        //        ProgHelpers.team1ids.Add(c1i);
        //    }
        //    else
        //    {
        //        ProgHelpers.captain2 = c1n;
        //        ProgHelpers.captain2id = c1i;
        //        ProgHelpers.team2.Clear();
        //        ProgHelpers.team2ids.Clear();
        //        ProgHelpers.team2.Add(c1n);
        //        ProgHelpers.team2ids.Add(c1i);
        //    }
        //        http.CreateMessage($"<@{authorId}> " + this.settings.Localization[Keys.RelinqSuccessful] + $" <@{c1i}>");
        //    if (ProgHelpers.team1ids.Count > 2)
        //    {
        //        http.CreateMessage($"<@{authorId}> " + this.settings.Localization["pickPhase." + (nextTeam) + "Turn"] + " <@" + ProgHelpers.pickturn + "> \n " + this.settings.Localization[Keys.PickPhaseUnpicked] + " \n" + string.Join("\n", ProgHelpers.draftchatnames.Cast<string>().ToArray()));
        //    }
        //    else
        //    {
        //        var phlist = new List<string>();
        //        var count = 0;
        //        foreach (var item in ProgHelpers.queue)
        //        {
        //            phlist.Add(count + " - " + item);
        //            count++;
        //        }
        //        http.CreateMessage(this.settings.Localization[Keys.PickPhaseStarted] + " " + "<@" + ProgHelpers.captain1id + ">" + "\n"
        //                                  + this.settings.Localization[Keys.PickPhaseTeam2Captain] + " " + "<@" + ProgHelpers.captain2id + ">" + "\n" + this.settings.Localization[Keys.PickPhaseInstructions]
        //                                  + "\n \n" + string.Join("\n", phlist.Cast<string>().ToArray()));
        //    }
        //}

    }
}
