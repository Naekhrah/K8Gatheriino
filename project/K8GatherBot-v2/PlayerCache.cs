namespace K8GatherBotv2
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Discore;

    using K8GatherBotv2.Domain;

    /// <summary>
    /// The player cache.
    /// </summary>
    public class PlayerCache
    {
        private readonly ISettings settings;
        private readonly IData data;
        private readonly ConcurrentDictionary<string, Player> cache = new ConcurrentDictionary<string, Player>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerCache"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public PlayerCache(ISettings settings)
        {
            this.settings = settings;
            this.data = settings.Data;
        }

        /// <summary>
        /// Playerses this instance.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        public IEnumerable<Player> Players()
        {
            return this.cache.OrderBy(i => i.Key).Select(i => i.Value);
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(Player item)
        {
            if (this.cache.ContainsKey(item.Id))
            {
                return;    
            }

            this.cache.TryAdd(item.Id, item);
        }

        /// <summary>
        /// Gets a player by <paramref name="id"/> and <paramref name="name"/>.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<Player> Get(string id, string name)
        {
            if (!this.cache.TryGetValue(id, out var player))
            {
                player = new Player(id, name);
                this.cache.TryAdd(player.Id, player);
            }

            player.Name = name; // Name might have changed, so always update name.
            player.IsNewKid = await this.settings.Data.IsNewKid(player, this.settings.NewKidThreshold);

            return player;
        }

        /// <summary>
        /// Gets a player by <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        public async Task<Player> GetPlayer(DiscordMessage message)
        {
            var msg = message.Content;
            if (string.IsNullOrEmpty(msg))
            {
                return null;
            }

            if (!msg.Contains(' '))
            {
                return await this.Get(message.Author.Id.Id.ToString(), message.Author.Username);
            }

            if (message.Mentions.Count > 0)
            {
                return await this.Get(message.Mentions[0].Id.Id.ToString(), null);
            }

            var msgItems = msg.Split(' ');
            if (msgItems.Length > 1)
            {
                var id = await this.data.GetPlayerId(msgItems[1]);
                if (!string.IsNullOrEmpty(id))
                {
                    return await this.Get(id, msgItems[1]);
                }
            }

            return null;
        }
    }
}
