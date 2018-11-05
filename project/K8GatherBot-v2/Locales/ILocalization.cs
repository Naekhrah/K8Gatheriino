namespace K8GatherBotv2.Locales
{
    /// <summary>
    /// The localization interface.
    /// </summary>
    public interface ILocalization
    {
        /// <summary>
        /// Gets the <see cref="string"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="string"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns>The localized string.</returns>
        string this[Keys key] { get; }
    }
}
