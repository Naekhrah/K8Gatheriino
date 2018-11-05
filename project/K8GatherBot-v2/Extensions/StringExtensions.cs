namespace K8GatherBotv2.Extensions
{
    using K8GatherBotv2.Domain;

    /// <summary>
    /// The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the <paramref name="messageBody"/> string to <see cref="Command"/>.
        /// </summary>
        /// <param name="messageBody">The message body.</param>
        /// <returns>The <see cref="Command"/>.</returns>
        public static Command AsCommand(this string messageBody)
        {
            if (string.IsNullOrEmpty(messageBody))
            {
                return Command.None;
            }

            switch (messageBody)
            {
                case "!abb":    // haHAA!
                case "!asd":    // haHAA!
                case "!fap":    // haHAA!
                case "!ab":     // haHAA!
                case "!kohta":  // haHAA!
                case "!dab":    // haHAA!
                case "!sad":    // haHAA!
                case "!abs":    // hahaa!
                case "!bad":    // haHAA!
                case "!ad":     // haHAA!
                case "!mad":    // haHAA!
                case "!grand":  // haHAA!
                case "!add":
                    return Command.Add;

                case "!remove":
                case "!rm":
                    return Command.Remove;

                case "!ready":
                case "!r":
                    return Command.Ready;

                case "!wimp":
                case "!nofuckingway":
                case "!noob":
                case "!relinquish":
                    return Command.Wimp;

                case "!pick":
                case "!p":
                    return Command.Pick;

                case "!pstats":
                    return Command.Stats;

                case "!fatkid":
                    return Command.FatKid;

                case "!f10":
                case "!fat10":
                    return Command.FatTopTen;

                case "!highscore":
                case "!hs":
                    return Command.HighScore;

                case "!topten":
                case "!top10":
                    return Command.TopTen;

                case "!thinkid":
                    return Command.ThinKid;

                case "!tk10":
                    return Command.ThinTopTen;

                case "!captain":
                    return Command.Captain;

                case "!c10":
                    return Command.CaptainTopTen;

                case "!gstatus":
                case "!gs":
                    return Command.Status;

                case "!resetbot":
                    return Command.Reset;

                case "!gatherinfo":
                case "!gi":
                    return Command.Info;

                default:
                    return Command.None;
            }
        }
    }
}
