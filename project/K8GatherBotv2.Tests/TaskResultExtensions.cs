namespace K8GatherBotv2.Tests
{
    using K8GatherBotv2.Domain;
    using K8GatherBotv2.Locales;

    using Xunit;

    /// <summary>
    /// The task result extensions.
    /// </summary>
    public static class TaskResultExtensions
    {
        /// <summary>
        /// Verifies the specified success.
        /// </summary>
        /// <param name="taskResult">The task result.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="erroCode">The erro code.</param>
        /// <param name="key">The key.</param>
        public static void Verify(this TaskResult taskResult, bool success, ErrorCode? erroCode = null, Keys? key = null)
        {
            Assert.NotNull(taskResult);
            Assert.Equal(success, taskResult.Success);

            if (erroCode.HasValue)
            {
                Assert.Equal(erroCode.Value, taskResult.Code);
            }

            if (key.HasValue)
            {
                Assert.Equal(key.Value, taskResult.Key);
            }
        }
    }
}
