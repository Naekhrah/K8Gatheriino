namespace K8GatherBotv2.Domain
{
    /// <summary>
    /// The error code enum.
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,

        /// <summary>
        /// The pick phase in progress
        /// </summary>
        PickPhaseInProgress,

        /// <summary>
        /// The player already added
        /// </summary>
        PlayerAlreadyAdded,

        /// <summary>
        /// The queue full
        /// </summary>
        QueueFull,

        /// <summary>
        /// The queue is empty
        /// </summary>
        QueueIsEmpty,

        /// <summary>
        /// The not in queue
        /// </summary>
        NotInQueue,

        /// <summary>
        /// The player already ready
        /// </summary>
        PlayerAlreadyReady,

        /// <summary>
        /// The player not in queue
        /// </summary>
        PlayerNotInQueue,

        /// <summary>
        /// The not your turn
        /// </summary>
        NotYourTurn,

        /// <summary>
        /// The not captain
        /// </summary>
        NotCaptain,

        /// <summary>
        /// The already picked
        /// </summary>
        AlreadyPicked,

        /// <summary>
        /// The everyone not ready
        /// </summary>
        EveryoneNotReady,

        /// <summary>
        /// The unknown index
        /// </summary>
        UnknownIndex,
    }
}
