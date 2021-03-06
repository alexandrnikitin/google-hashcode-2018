﻿using System.Diagnostics;
using Xunit.Abstractions;

namespace Rides.Silo.Tests
{
    public class TestTraceListener : TraceListener
    {
        private readonly ITestOutputHelper _output;

        public TestTraceListener(ITestOutputHelper output)
        {
            _output = output;
        }

        public override void Write(string message)
        {
            _output.WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            _output.WriteLine(message);
        }
    }
}