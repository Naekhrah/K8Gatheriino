namespace K8GatherBotv2.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Discore.Http;

    /// <summary>
    /// The test messenger, writes messages to trace.
    /// </summary>
    /// <seealso cref="IMessenger" />
    public class TestMessenger : IMessenger
    {
        /// <inheritdoc />
        public Task SendMessage(string message)
        {
            Trace.WriteLine(message);
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task SendMessage(CreateMessageOptions createMessageOptions)
        {
            Trace.WriteLine(createMessageOptions.Content);
            return Task.FromResult(0);
        }

        public Task SendFormattedMessage(string title, IEnumerable<Tuple<string, string>> fields)
        {
            Trace.WriteLine(title);

            foreach (var field in fields)
            {
                Trace.WriteLine($"  {field.Item1} {field.Item2}");
            }

            return Task.FromResult(0);
        }
    }
}
