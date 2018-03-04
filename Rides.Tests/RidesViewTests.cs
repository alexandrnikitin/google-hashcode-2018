using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Rides.Tests
{
    public class RidesViewTests
    {
        public RidesViewTests(ITestOutputHelper outputHelper)
        {
            Trace.Listeners.Add(new TestTraceListener(outputHelper));
        }

        [Fact]
        public void GetTest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\b_should_be_easy.in"));
            var sut = new RidesView(problem.Rides);
            var actual = sut.GetRides();
            Trace.WriteLine(sut.GetEarliestFinish());
        }
    }
}