﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Rides.Tests
{
    public class CityCarStateTests
    {
        public CityCarStateTests(ITestOutputHelper outputHelper)
        {
            Trace.Listeners.Add(new TestTraceListener(outputHelper));
        }

        [Fact]
        public void GetAvailableActionsTest()
        {
            var problem = ProblemBuilder.Build(File.ReadAllLines(@"..\..\..\Resources\b_should_be_easy.in"));
            var availableRides = new HashSet<Ride>();
            availableRides.Add(new Ride(0, new Point(), new Point(10, 10), 0, 110));
            availableRides.Add(new Ride(1, new Point(1,1), new Point(10, 10), 0, 110));
            availableRides.Add(new Ride(2, new Point(2,2), new Point(3, 3), 0, 110));

            var sut = new CityCarState(problem, new Car(0, new Point(),0), availableRides, 0);
            var actual = sut.GetAvailableActions();

            foreach (var makeRideAction in actual)
            {
                Trace.WriteLine(makeRideAction);
            }
        }
    }
}