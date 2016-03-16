using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;


namespace Acr.Utilities.Tests
{
    [TestFixture]
    public class ActionQueueTests
    {

        [Test]
        public async Task Basic()
        {
            var one = false;
            var two = true;

            var queue = new ActionQueue();
            queue.Add(
                () =>
                {
                    one = true;
                    queue.Stop();
                },
                () =>
                {
                    two = false;
                }
            );
            queue.Start();
            await Task.Delay(2000);

            one.Should().BeTrue("One should be true");
            two.Should().BeTrue("Two should be true");
        }
    }
}
