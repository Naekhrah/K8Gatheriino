namespace K8GatherBotv2
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Text;
    using CsvHelper;

    using K8GatherBotv2.Domain;

    /// <summary>
    /// The persisted data.
    /// </summary>
    /// <seealso cref="IData" />
    public class PersistedData : IData
    {
        private readonly List<UserData> fatKids = new List<UserData>();
        private readonly List<UserData> highScores = new List<UserData>();
        private readonly List<UserData> thinKids = new List<UserData>();
        private readonly List<UserData> captains = new List<UserData>();
        private const string FatkidFileName = "fatkid.csv";
        private const string HighScoreFileName = "highscores.csv";
        private const string ThinkidFileName = "thinkid.csv";
        private const string CaptainsFileName = "captains.csv";

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistedData"/> class.
        /// </summary>
        public PersistedData()
        {
            this.InitData();
        }

        /// <inheritdoc />
        public async Task<string> GetPlayerId(string username)
        {
            foreach (var datalist in this.DataLists())
            {
                var userData = datalist.SingleOrDefault(f => f.UserName == username);
                if (userData != null)
                {
                    return await Task.FromResult(userData.Id);
                }
            }

            return null;
        }

        /// <inheritdoc />
        public async Task AddFatKid(Player player)
        {
            Add(this.fatKids, player.Id, player.Name);
            await PersistList(this.fatKids, FatkidFileName);
        }

        /// <inheritdoc />
        public async Task AddThinKid(Player player)
        {
            Add(this.thinKids, player.Id, player.Name);
            await PersistList(this.thinKids, ThinkidFileName);
        }

        /// <inheritdoc />
        public async Task AddHighScores(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                Add(this.highScores, player.Id, player.Name);
            }

            await PersistList(this.highScores, HighScoreFileName);
        }

        /// <inheritdoc />
        public async Task AddCaptains(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                Add(this.captains, player.Id, player.Name);
            }

            await PersistList(this.captains, CaptainsFileName);
        }

        /// <inheritdoc />
        public async Task AddCaptain(Player player)
        {
            Add(this.captains, player.Id, player.Name);
            await PersistList(this.captains, CaptainsFileName);
        }

        /// <inheritdoc />
        public async Task RemoveCaptain(Player player)
        {
            Minus(this.captains, player.Id, player.Name);
            await PersistList(this.captains, CaptainsFileName);
        }

        /// <inheritdoc />
        public async Task<string> GetFatKidInfo(Player player, string response)
        {
            return await Task.FromResult(GetInfo(this.fatKids, player, response));
        }

        /// <inheritdoc />
        public async Task<string> GetThinKidInfo(Player player, string response)
        {
            return await Task.FromResult(GetInfo(this.thinKids, player, response));
        }

        /// <inheritdoc />
        public async Task<string> GetHighScoreInfo(Player player, string response)
        {
            return await Task.FromResult(GetInfo(this.highScores, player, response));
        }

        /// <inheritdoc />
        public async Task<string> GetCaptainInfo(Player player, string response)
        {
            return await Task.FromResult(GetInfo(this.captains, player, response));
        }

        /// <inheritdoc />
        public async Task<string> GetFatKidTop10()
        {
            return await Task.FromResult(GetTop10Info(this.fatKids));
        }

        /// <inheritdoc />
        public async Task<string> GetThinKidTop10()
        {
            return await Task.FromResult(GetTop10Info(this.thinKids));
        }

        /// <inheritdoc />
        public async Task<string> GetHighScoreTop10()
        {
            return await Task.FromResult(GetTop10Info(this.highScores));
        }

        /// <inheritdoc />
        public async Task<string> GetCaptainTop10()
        {
            return await Task.FromResult(GetTop10Info(this.captains));
        }

        private static string GetTop10Info(IReadOnlyList<UserData> data)
        {
            if (data.Count == 0)
            {
                return ":(";
            }

            var list = "";
            for (var i = 0; i < 10 && i != data.Count; i++)
            {
                var entry = data[i];
                list += i + 1 + ". " + entry.UserName + " / " + entry.Count + "\n";
            }
            return list;
        }

        public async Task<bool> IsNewKid(Player player, int newKidThreshold)
        {
            var count = 0;
            var fkentry = this.fatKids.Find(x => x.Id.Equals(player.Id));
            if (fkentry != null)
            {
                count += fkentry.Count;
            }

            var tkentry = this.thinKids.Find(x => x.Id.Equals(player.Id));
            if (tkentry != null)
            {
                count += tkentry.Count;
            }

            return await Task.FromResult(count < newKidThreshold);
        }

        public async Task<bool> AreAllNewKids(IEnumerable<Player> players, int threshold)
        {
            foreach(var player in players)
            {
                if (!await this.IsNewKid(player, threshold))
                {
                    return await Task.FromResult(false);
                }
            }

            return await Task.FromResult(true);
        }

        private static void InitList(List<UserData> data, string fileName)
        {
            if (!File.Exists(fileName))
            {
                return;
            }
            using (var fileReader = File.OpenText(fileName))
            {
                var csvFile = new CsvReader(fileReader);
                csvFile.Configuration.HasHeaderRecord = false;
                csvFile.Read();
                var records = csvFile.GetRecords<UserData>();
                data.AddRange(records);
            }
        }

        private static async Task PersistList(List<UserData> data, string fileName)
        {
            using (var writer = new StreamWriter(File.Open(fileName, FileMode.Create)))
            {
                var csv = new CsvWriter(writer);
                csv.Configuration.Encoding = Encoding.UTF8;
                data.Sort();
                csv.WriteRecords(data);
                await writer.FlushAsync();
            }
        }

        private static string GetInfo(List<UserData> data, Player player, string response)
        {
            if (player == null)
            {
                return null;
            }

            var entry = data.Find(x => x.Id.Equals(player.Id));

            return entry == null
                ? string.Format(response, player.Name, 0, data.Count, data.Count)
                : string.Format(response, entry.UserName, entry.Count, data.IndexOf(entry) + 1, data.Count);
        }

        private static void Minus(List<UserData> data, string id, string userName)
        {
            var entry = data.Find(x => x.Id.Equals(id));
            entry?.Minus();
        }

        private static void Add(List<UserData> data, string id, string userName)
        {
            var entry = data.Find(x => x.Id.Equals(id));

            if (entry == null)
            {
                entry = new UserData(id, userName);
                data.Add(entry);
            }
            else
            {
                entry.UserName = userName;
            }
            entry.Add();
        }

        /// <summary>
        /// Initializes the data.
        /// </summary>
        private void InitData()
        {
            InitList(this.fatKids, FatkidFileName);
            InitList(this.thinKids, ThinkidFileName);
            InitList(this.highScores, HighScoreFileName);
            InitList(this.captains, CaptainsFileName);
        }

        /// <summary>
        /// Enumerates the data lists.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{T}"/>.</returns>
        private IEnumerable<List<UserData>> DataLists()
        {
            yield return this.fatKids;
            yield return this.thinKids;
            yield return this.highScores;
            yield return this.captains;
        }
    }
}
