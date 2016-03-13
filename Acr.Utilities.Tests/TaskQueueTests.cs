using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;


namespace Acr.Utilities.Tests
{
    [TestFixture]
    public class TaskQueueTests
    {

        [Test]
        public void Executes_Multiple_Tasks()
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
            one.Should().BeTrue("One isn't running");
            two.Should().BeTrue("Two isn't running");
        }


        [Test]
        public void Executes_Up_To_MaxExecutions()
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
            queue.Stop();
            one.Should().BeTrue("One didn't start");
            two.Should().BeFalse("Two did start");
        }


        [Test]
        public void Cancels_All_Tasks()
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
            queue.Stop();
            oneStarted.Should().BeTrue("One didn't start");
            oneFinished.Should().BeFalse("One finished and shouldn't have");
            twoStarted.Should().BeTrue("Two didn't start");
            twoFinished.Should().BeFalse("Two finished and shouldn't have");
        }
    }
}
