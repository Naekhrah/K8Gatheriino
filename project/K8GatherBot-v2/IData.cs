namespace K8GatherBotv2
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using K8GatherBotv2.Domain;

    /// <summary>
    /// The data interface.
    /// </summary>
    public interface IData
    {
        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task<string> GetPlayerId(string username);

        /// <summary>
        /// Determines whether <paramref name="player"/> is a new kid based on the <paramref name="threshold"/>.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="threshold">The thresh hold.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task<bool> IsNewKid(Player player, int threshold);

        /// <summary>
        /// Determines whether <paramref name="players"/> are all new kids based on the <paramref name="threshold"/>.
        /// </summary>
        /// <param name="players">The players.</param>
        /// <param name="threshold">The threshold.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task<bool> AreAllNewKids(IEnumerable<Player> players, int threshold);

        /// <summary>
        /// Adds the captains.
        /// </summary>
        /// <param name="players">The players.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task AddCaptains(IEnumerable<Player> players);

        /// <summary>
        /// Adds the thin kid.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task AddThinKid(Player player);

        /// <summary>
        /// Adds the fat kid.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task AddFatKid(Player player);

        /// <summary>
        /// Adds the high scores.
        /// </summary>
        /// <param name="players">The players.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task AddHighScores(IEnumerable<Player> players);

        /// <summary>
        /// Adds the captain.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task AddCaptain(Player player);

        /// <summary>
        /// Removes the captain.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task RemoveCaptain(Player player);

        /// <summary>
        /// Gets the fat kid information.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="response">The response.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task<string> GetFatKidInfo(Player player, string response);

        /// <summary>
        /// Gets the thin kid information.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="response">The response.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task<string> GetThinKidInfo(Player player, string response);

        /// <summary>
        /// Gets the high score information.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="response">The response.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task<string> GetHighScoreInfo(Player player, string response);

        /// <summary>
        /// Gets the captain information.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="response">The response.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task<string> GetCaptainInfo(Player player, string response);

        /// <summary>
        /// Gets the fat kid top10.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task<string> GetFatKidTop10();

        /// <summary>
        /// Gets the thin kid top10.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task<string> GetThinKidTop10();

        /// <summary>
        /// Gets the high score top10.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task<string> GetHighScoreTop10();

        /// <summary>
        /// Gets the captain top10.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task<string> GetCaptainTop10();
    }
}
