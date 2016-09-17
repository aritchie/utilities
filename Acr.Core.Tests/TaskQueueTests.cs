using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;


namespace Acr.Core.Tests
{
    public class TaskQueueTests
    {
        [Fact]
        public async Task Executes_Multiple_Tasks()
        {
            var one = false;
            var two = false;

            var queue = new TaskQueue { MaxExecutions = 2 };
            queue.Add(
                token => Task.Run(async () =>
                {
                    one = true;
                    await Task.Delay(5000);
                }),
                token => Task.Run(async () =>
                {
                    two = true;
                    await Task.Delay(5000);
                })
            );
            queue.Start();
            await Task.Delay(1000); // let everything catch up
            one.Should().BeTrue("One isn't running");
            two.Should().BeTrue("Two isn't running");
        }


        [Test]
        public async Task Executes_Up_To_MaxExecutions()
        {
            var one = false;
            var two = false;

            var queue = new TaskQueue { MaxExecutions = 1 };
            queue.Add(
                token => Task.Run(async () =>
                {
                    one = true;
                    await Task.Delay(5000, token);
                }),
                token => Task.Run(async () =>
                {
                    two = true;
                    await Task.Delay(5000);
                })
            );
            queue.Start();
            await Task.Delay(1000);
            queue.Stop();
            one.Should().BeTrue("One didn't start");
            two.Should().BeFalse("Two did start");
        }


        [Test]
        public async Task Cancels_All_Tasks()
        {
            var oneStarted = false;
            var oneFinished = false;
            var twoStarted = false;
            var twoFinished = false;

            var queue = new TaskQueue { MaxExecutions = 2 };
            queue.Add(
                token => Task.Run(async () =>
                {
                    oneStarted = true;
                    await Task.Delay(5000, token);
                    oneFinished = true;
                }),
                token => Task.Run(async () =>
                {
                    twoStarted = true;
                    await Task.Delay(5000);
                    twoFinished = true;
                })
            );
            queue.Start();
            await Task.Delay(1000);
            queue.Stop();
            oneStarted.Should().BeTrue("One didn't start");
            oneFinished.Should().BeFalse("One finished and shouldn't have");
            twoStarted.Should().BeTrue("Two didn't start");
            twoFinished.Should().BeFalse("Two finished and shouldn't have");
        }


        [Test]
        public async Task Resume_Tasks()
        {
            var one = false;
            var two = false;

            var queue = new TaskQueue { MaxExecutions = 1 };
            queue.Add(
                token => Task.Run(async () =>
                {
                    one = true;
                    queue.Stop();
                    await Task.Delay(5000, token);
                }),
                token => Task.Run(async () =>
                {
                    two = true;
                    await Task.Delay(5000);
                })
            );
            queue.Start();
            await Task.Delay(1000);

            one.Should().BeTrue("One didn't execute");
            two.Should().BeFalse("Two should not have executed");

            queue.Start();
            await Task.Delay(1000);
            two.Should().BeTrue("Two did not execute");
        }
    }
}
