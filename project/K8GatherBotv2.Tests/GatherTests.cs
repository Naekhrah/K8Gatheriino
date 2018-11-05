namespace K8GatherBotv2.Tests
{
    using System;
    using System.Linq;

    using K8GatherBotv2.Domain;
    using K8GatherBotv2.Locales;

    using Moq;

    using Xunit;

    /// <summary>
    /// The gather tests.
    ///  </summary>
    /// <seealso cref="IDisposable" />
    public class GatherTests : IDisposable
    {
        private readonly PlayerCache playerCache;
        private readonly TestMessenger testMessenger;
        private readonly Mock<IData> mockedData;
        private readonly Mock<ISettings> settings;
        private const int MaxPlayerCount = 12;

        /// <summary>
        /// Initializes a new instance of the <see cref="GatherTests"/> class.
        /// </summary>
        public GatherTests()
        {
            this.testMessenger = new TestMessenger();

            this.mockedData = new Mock<IData>();
            this.mockedData.Setup(p => p.IsNewKid(It.IsAny<Player>(), 50));

            this.settings = new Mock<ISettings>();
            this.settings.Setup(p => p.Language).Returns("fi");
            this.settings.Setup(p => p.TeamCount).Returns(2);
            this.settings.Setup(p => p.PlayersInTeam).Returns(6);
            this.settings.Setup(p => p.QueueSize).Returns(12);
            this.settings.Setup(p => p.Data).Returns(this.mockedData.Object);
            this.settings.Setup(p => p.Localization).Returns(new Finnish());

            this.playerCache = new PlayerCache(this.settings.Object);
            this.playerCache.Add(new Player("0", "Naekhrah"));
            this.playerCache.Add(new Player("1", "Kitsun8"));
            this.playerCache.Add(new Player("2", "pirate_patch"));
            this.playerCache.Add(new Player("3", "KAT"));
            this.playerCache.Add(new Player("4", "FaRmaS"));
            this.playerCache.Add(new Player("5", "Chupe"));
            this.playerCache.Add(new Player("6", "M4dny"));
            this.playerCache.Add(new Player("7", "X1mebox"));
            this.playerCache.Add(new Player("8", "Karjis"));
            this.playerCache.Add(new Player("9", "Nasse"));
            this.playerCache.Add(new Player("10", "Oakki"));
            this.playerCache.Add(new Player("11", "Chip"));
            this.playerCache.Add(new Player("12", "rCK"));
            this.playerCache.Add(new Player("13", "Emali"));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Verifies that user can add to queue if queue is empty.
        /// </summary>
        [Fact]
        public async void CanAddIfEmpty()
        {
            var gather = new Gather(this.testMessenger, this.settings.Object, 0);
            var player = await this.playerCache.Get("0", "Naekhrah");
            var result = await gather.TryAdd(player);
            result.Verify(true, key: Keys.QueuePhaseAdded);

            Assert.False(gather.IsFull);
            Assert.True(gather.Queue.Count == 1);
        }

        /// <summary>
        /// Verifies that user can add to queue if queue contains players but not yet full.
        /// </summary>
        [Fact]
        public async void CanAddIfNotYetFull()
        {
            var gather = new Gather(this.testMessenger, this.settings.Object, 0);
            var player = await this.playerCache.Get("0", "Naekhrah");

            var result = await gather.TryAdd(player);
            result.Verify(true, key: Keys.QueuePhaseAdded);

            Assert.False(gather.IsFull);
            Assert.True(gather.Queue.Count == 1);

            var player2 = await this.playerCache.Get("1", "Kitsun8");
            result = await gather.TryAdd(player2);
            result.Verify(true, key: Keys.QueuePhaseAdded);

            Assert.False(gather.IsFull);
            Assert.True(gather.Queue.Count == 2);
        }

        /// <summary>
        /// Verifies that user can add if empty and remove if not full.
        /// </summary>
        [Fact]
        public async void CanAddIfEmptyAndRemoveIfNotFull()
        {
            var gather = new Gather(this.testMessenger, this.settings.Object, 0);
            var player = await this.playerCache.Get("0", "Naekhrah");
            var result = await gather.TryAdd(player);
            result.Verify(true, key: Keys.QueuePhaseAdded);

            Assert.False(gather.IsFull);
            Assert.True(gather.Queue.Count == 1);

            result = await gather.TryRemove(player, false);
            result.Verify(true, key: Keys.QueuePhaseRemoved);

            Assert.False(gather.IsFull);
            Assert.True(gather.Queue.Count == 0);
        }

        /// <summary>
        /// Verifies that user can not add twice to queue.
        /// </summary>
        [Fact]
        public async void CannotAddTwice()
        {
            var gather = new Gather(this.testMessenger, this.settings.Object, 0);
            var player = await this.playerCache.Get("0", "Naekhrah");

            var result = await gather.TryAdd(player);
            result.Verify(true, key: Keys.QueuePhaseAdded);

            Assert.False(gather.IsFull);
            Assert.True(gather.Queue.Count == 1);

            result = await gather.TryAdd(player);
            result.Verify(false, ErrorCode.PlayerAlreadyAdded);
        }

        /// <summary>
        /// Verifies that user can not add if the queue is full.
        /// </summary>
        [Fact]
        public async void CanNotAddIfFull()
        {
            var gather = new Gather(this.testMessenger, this.settings.Object, 0);
            TaskResult result;

            var playerPool = this.playerCache.Players().Take(MaxPlayerCount + 1).ToList();
            foreach (var p in playerPool.Take(MaxPlayerCount))
            {
                result = await gather.TryAdd(p);
                result.Verify(true, key: Keys.QueuePhaseAdded);
            }
            Assert.True(gather.IsFull);
            Assert.True(gather.Queue.Count == MaxPlayerCount);

            var player = playerPool.LastOrDefault();
            result = await gather.TryAdd(player);
            result.Verify(false, ErrorCode.QueueFull);

            Assert.True(gather.IsFull);
            Assert.True(gather.Queue.Count == MaxPlayerCount);
        }

        /// <summary>
        /// Verifies that user can remove if queue is not yet full.
        /// </summary>
        [Fact]
        public async void CanRemoveIfNotFull()
        {
            var gather = new Gather(this.testMessenger, this.settings.Object, 0);
            TaskResult result;

            var playerPool = this.playerCache.Players().Take(MaxPlayerCount - 1).ToList();
            foreach (var p in playerPool.Take(MaxPlayerCount - 1))
            {
                result = await gather.TryAdd(p);
                result.Verify(true, key: Keys.QueuePhaseAdded);
            }
            Assert.False(gather.IsFull);
            Assert.True(gather.Queue.Count == MaxPlayerCount - 1);

            var player = playerPool.LastOrDefault();
            result = await gather.TryRemove(player, false);
            result.Verify(true, key: Keys.QueuePhaseRemoved);

            Assert.False(gather.IsFull);
            Assert.True(gather.Queue.Count == 10);
        }

        /// <summary>
        /// Verifies that user can not remove if queue is full.
        /// </summary>
        [Fact]
        public async void CanNotRemoveIfFull()
        {
            var gather = new Gather(this.testMessenger, this.settings.Object, 0);
            TaskResult result;

            var playerPool = this.playerCache.Players().Take(MaxPlayerCount).ToList();
            foreach (var p in playerPool.Take(MaxPlayerCount))
            {
                result = await gather.TryAdd(p);
                result.Verify(true, key: Keys.QueuePhaseAdded);
            }

            Assert.True(gather.IsFull);
            Assert.True(gather.Queue.Count == MaxPlayerCount);

            var player = playerPool.LastOrDefault();
            result = await gather.TryRemove(player, false);
            result.Verify(false, ErrorCode.PickPhaseInProgress);

            Assert.True(gather.IsFull);
            Assert.True(gather.Queue.Count == MaxPlayerCount);
        }

        /// <summary>
        /// Verifies that user can ready when queue is full.
        /// </summary>
        [Fact]
        public async void CanReadyWhenFull()
        {
            var gather = new Gather(this.testMessenger, this.settings.Object, 0);
            TaskResult result;

            var playerPool = this.playerCache.Players().Take(MaxPlayerCount).ToList();
            foreach (var player in playerPool.Take(MaxPlayerCount))
            {
                result = await gather.TryAdd(player);
                result.Verify(true, key: Keys.QueuePhaseAdded);
            }
            Assert.True(gather.IsFull);
            Assert.True(gather.Queue.Count == MaxPlayerCount);

            foreach (var player in playerPool.Take(MaxPlayerCount))
            {
                result = await gather.TrySetReady(player);
                result.Verify(true, key: Keys.ReadyPhaseReady);
            }
        }

        /// <summary>
        /// Verifies that user can pick when queue is full and everyone is ready.
        /// </summary>
        [Fact]
        public async void CanPickWhenFullAndReady()
        {
            var gather = new Gather(this.testMessenger, this.settings.Object, 0);
            TaskResult result;

            var playerPool = this.playerCache.Players().Take(MaxPlayerCount).ToList();
            foreach (var player in playerPool.Take(MaxPlayerCount))
            {
                result = await gather.TryAdd(player);
                result.Verify(true, key: Keys.QueuePhaseAdded);
            }
            Assert.True(gather.IsFull);
            Assert.True(gather.Queue.Count == MaxPlayerCount);

            foreach (var player in playerPool.Take(MaxPlayerCount))
            {
                result = await gather.TrySetReady(player);
                result.Verify(true, key: Keys.ReadyPhaseReady);
            }

            var firstTeam = gather.Game.Teams.First();
            var secondTeam = gather.Game.Teams.Last();

            Assert.NotNull(firstTeam.Captain);
            Assert.NotNull(secondTeam.Captain);
            Assert.Equal(firstTeam, gather.PickTurn);

            var p1 = gather.Queue.FirstOrDefault(p => p.Team == null);
            var index = gather.Queue.ToList().IndexOf(p1);
        
            result = await gather.TryPick(gather.PickTurn.Captain, index);
            result.Verify(true, key: Keys.PickPhasePlayedAdded);

            Assert.Equal(2, firstTeam.Members.Count);
            Assert.Single(secondTeam.Members);
            Assert.Equal(9, gather.Queue.Count(p => p.Team == null));
            Assert.Equal(secondTeam, gather.PickTurn);
        }

        /// <summary>
        /// Verifies that user can not pick when queue is full and everyone is not yet ready.
        /// </summary>
        [Fact]
        public async void CanNotPickWhenNotReady()
        {
            var gather = new Gather(this.testMessenger, this.settings.Object, 0);
            TaskResult result;

            var playerPool = this.playerCache.Players().Take(MaxPlayerCount).ToList();
            foreach (var player in playerPool.Take(MaxPlayerCount))
            {
                result = await gather.TryAdd(player);
                result.Verify(true, key: Keys.QueuePhaseAdded);
            }
            Assert.True(gather.IsFull);
            Assert.True(gather.Queue.Count == MaxPlayerCount);

            var p1 = gather.Queue.FirstOrDefault(p => p.Team == null);
            var index = gather.Queue.ToList().IndexOf(p1);
        
            result = await gather.TryPick(gather.PickTurn.Captain, index);
            result.Verify(false, ErrorCode.EveryoneNotReady);

            var firstTeam = gather.Game.Teams.First();
            var secondTeam = gather.Game.Teams.Last();
            Assert.Empty(firstTeam.Members);
            Assert.Empty(secondTeam.Members);
            Assert.Equal(12, gather.Queue.Count(p => p.Team == null));
        }

        /// <summary>
        /// Verifies that user can not pick when is not their turn.
        /// </summary>
        [Fact]
        public async void CanNotPickWhenNotInTurn()
        {
            var gather = new Gather(this.testMessenger, this.settings.Object, 0);
            TaskResult result;

            var playerPool = this.playerCache.Players().Take(MaxPlayerCount).ToList();
            foreach (var player in playerPool.Take(MaxPlayerCount))
            {
                result = await gather.TryAdd(player);
                result.Verify(true, key: Keys.QueuePhaseAdded);
            }
            Assert.True(gather.IsFull);
            Assert.True(gather.Queue.Count == MaxPlayerCount);

            foreach (var player in playerPool.Take(MaxPlayerCount))
            {
                result = await gather.TrySetReady(player);
                result.Verify(true, key: Keys.ReadyPhaseReady);
            }

            var firstTeam = gather.Game.Teams.First();
            var secondTeam = gather.Game.Teams.Last();

            Assert.NotNull(firstTeam.Captain);
            Assert.NotNull(secondTeam.Captain);
            Assert.Equal(gather.Game.Teams.First(), gather.PickTurn);

            var p1 = gather.Queue.FirstOrDefault(p => p.Team == null);
            var index = gather.Queue.ToList().IndexOf(p1);

            result = await gather.TryPick(secondTeam.Captain, index);
            result.Verify(false, ErrorCode.NotYourTurn);

            Assert.Single(firstTeam.Members);
            Assert.Single(secondTeam.Members);
            Assert.Equal(10, gather.Queue.Count(p => p.Team == null));
        }

        /// <summary>
        /// Verifies that user can not pick when they are not the captain.
        /// </summary>
        [Fact]
        public async void CanNotPickWhenNotCaptain()
        {
            var gather = new Gather(this.testMessenger, this.settings.Object, 0);
            TaskResult result;

            var playerPool = this.playerCache.Players().Take(MaxPlayerCount).ToList();
            foreach (var player in playerPool.Take(MaxPlayerCount))
            {
                result = await gather.TryAdd(player);
                result.Verify(true, key: Keys.QueuePhaseAdded);
            }
            Assert.True(gather.IsFull);
            Assert.True(gather.Queue.Count == MaxPlayerCount);

            foreach (var player in playerPool.Take(MaxPlayerCount))
            {
                result = await gather.TrySetReady(player);
                result.Verify(true, key: Keys.ReadyPhaseReady);
            }

            var firstTeam = gather.Game.Teams.First();
            var secondTeam = gather.Game.Teams.Last();

            Assert.NotNull(firstTeam.Captain);
            Assert.NotNull(secondTeam.Captain);
            Assert.Equal(firstTeam, gather.PickTurn);

            var picker = gather.Queue.FirstOrDefault(p => p.Team == null);
            var pickedPlayer = gather.Queue.LastOrDefault(p => p.Team == null);
            var index = gather.Queue.ToList().IndexOf(pickedPlayer);

            result = await gather.TryPick(picker, index);
            result.Verify(false, ErrorCode.NotCaptain);

            Assert.Single(firstTeam.Members);
            Assert.Single(secondTeam.Members);
            Assert.Equal(10, gather.Queue.Count(p => p.Team == null));
        }

        /// <summary>
        /// Verifies that all players in the queue can be picked when queue is full and everyone is ready.
        /// </summary>
        [Fact]
        public async void CanPickAllPlayers()
        {
            var gather = new Gather(this.testMessenger, this.settings.Object, 0);
            TaskResult result;

            var playerPool = this.playerCache.Players().Take(MaxPlayerCount).ToList();
            foreach (var player in playerPool.Take(MaxPlayerCount))
            {
                result = await gather.TryAdd(player);
                result.Verify(true, key: Keys.QueuePhaseAdded);
            }
            Assert.True(gather.IsFull);
            Assert.True(gather.Queue.Count == MaxPlayerCount);

            foreach (var player in playerPool.Take(MaxPlayerCount))
            {
                result = await gather.TrySetReady(player);
                result.Verify(true, key: Keys.ReadyPhaseReady);
            }

            var firstTeam = gather.Game.Teams.First();
            var secondTeam = gather.Game.Teams.Last();

            Assert.NotNull(firstTeam.Captain);
            Assert.NotNull(secondTeam.Captain);
            Assert.Equal(firstTeam, gather.PickTurn);

            Player p1;
            while((p1 = gather.Queue.FirstOrDefault(p => p.Team == null)) != null)
            {
                var index = gather.Queue.ToList().IndexOf(p1);
                result = await gather.TryPick(gather.PickTurn.Captain, index);
                result.Verify(true, key: Keys.PickPhasePlayedAdded);
            }

            Assert.NotEmpty(firstTeam.Members);
            Assert.NotEmpty(secondTeam.Members);
            Assert.Equal(0, gather.Queue.Count(p => p.Team == null));
        }
    }
}
