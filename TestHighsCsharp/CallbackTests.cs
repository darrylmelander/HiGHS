using HighsTemp;

namespace TestHighsCsharp
{
    [TestClass]
    public class CallbackTests
    {
        [TestMethod]
        public void TestLoggingCallback()
        {
            var solver = new TestableHighsSolver();

            int log_call_count = 0;

            // Verify that we get no logging events if we aren't subscribed to them
            solver.TriggerTestEvents();
            Assert.AreEqual(0, log_call_count,
                            "Got logging events when we aren't subscribed to them");

            // Setup our event handler, and variables to verify calls to the handler
            string[] expected_messages = { "Message 1", "Msg 2" };
            int expected_log_call_count = expected_messages.Length;
            System.EventHandler<LoggingEventArgs> loggingHandler =
                (o, e) =>
                {
                    log_call_count++;
                    Assert.IsTrue(log_call_count <= expected_log_call_count,
                                  "More logging events received than expected");

                    Assert.AreEqual(expected_messages[log_call_count], e.message);
                };
            solver.LogReceived += loggingHandler;

            // Trigger events again, verify our event handler was called this time
            solver.TriggerTestEvents();

            Assert.AreEqual(expected_log_call_count, log_call_count,
                            "Got an unexpected number of logging messages");

            // Unsubscribe from events, verify handler is no longer called
            log_call_count = 0;
            solver.LogReceived -= loggingHandler;
            solver.TriggerTestEvents();
            Assert.AreEqual(0, log_call_count,
                           "Log handler called after unsubscribing from logging events");
        }

        [TestMethod]
        public void TestMipImprovedCallback()
        {
            var solver = new TestableHighsSolver();

            int event_call_count = 0;

            // Verify that we get no logging events if we aren't subscribed to them
            solver.TriggerTestEvents();
            Assert.AreEqual(0, event_call_count,
                            "Got events before we subscribed to them");

            // Setup variables to verify calls to the event handler
            double[] expected_obj_values = { 3.3, 2.2, 1.1 };
            int expected_log_call_count = expected_obj_values.Length;

            // Setup the event handler
            System.EventHandler<MipEventArgs> improvingHandler =
                (o, e) =>
                {
                    event_call_count++;
                    Assert.IsTrue(event_call_count <= expected_log_call_count,
                                    "More events received than expected");

                    Assert.AreEqual(expected_obj_values[event_call_count], e.objective_function_value);
                };
            solver.MipImprovingSolutionFound += improvingHandler;

            // Trigger events again, verify our event handler was called this time
            solver.TriggerTestEvents();

            Assert.AreEqual(expected_log_call_count, event_call_count,
                            "Got an unexpected number of events");

            // Unsubscribe from events, verify handler is no longer called
            event_call_count = 0;
            solver.MipImprovingSolutionFound -= improvingHandler;
            solver.TriggerTestEvents();
            Assert.AreEqual(0, event_call_count,
                            "Log handler called after unsubscribing from events");
        }

        private void TestCallback<EventArgsT>(
            int expected_event_count,
            ref int actual_event_count,
            System.EventHandler<EventArgsT> eventHandler,
            Action<EventHandler<EventArgsT>> subscribe,
            Action<EventHandler<EventArgsT>> unsubscribe)
        {
            var solver = new TestableHighsSolver();

            int event_call_count = 0;

            // Verify that we get no logging events if we aren't subscribed to them
            solver.TriggerTestEvents();
            Assert.AreEqual(0, event_call_count,
                            "Got events before we subscribed to them");


            // Subscribe then trigger events again, verify our event handler was called this time
            subscribe(eventHandler);
            solver.TriggerTestEvents();
            Assert.AreEqual(expected_event_count, event_call_count,
                            "Got an unexpected number of events");

            // Unsubscribe from events, verify handler is no longer called
            event_call_count = 0;
            unsubscribe(eventHandler);
            solver.TriggerTestEvents();
            Assert.AreEqual(0, event_call_count,
                            "Log handler called after unsubscribing from events");
        }
    }
}