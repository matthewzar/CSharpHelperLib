using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CollectionOfHelpers.Threading
{
    /// <summary>
    /// This class aims to be a self-maintaining thread-indepentant counter.
    /// For example, one instance of this might get created and shared between 10 threads
    /// (each of which is doing a similar task), each thread would then invoke an increment-event
    /// method.
    /// When instantiated, this class will create it's own thread, and hold both a reference to itself and it's thread - 
    /// that self reference allows invoking classes to not worry about tracking an extra thread. Thus the 
    /// ThreadedEventCounter maintains it's own state, and self-terminates when a certain predicate returns true.
    /// 
    /// How to use:
    /// declare a single instance that will be shared by all the threads being monitored.
    /// augment those threads to know about the instance (a static field is easiest)
    /// inside those threads, add IncrementEvent("eventname") calls at approriate locations
    /// run the code to be monitored
    /// </summary>
    public class ThreadedEventCounter
    {
        /// <summary>
        /// The events being tracked, a name and a count
        /// </summary>
        ConcurrentDictionary<string, int> EventCounts;

        /// <summary>
        /// The worker thread that gets spawned in the constructor. Doesn't need to be externally closed
        /// as monitors exist that will auto close this thread when either too much time has passed between event increments
        /// or a certain number of events have been logged.
        /// </summary>
        private Thread WorkerThread;

        /// <summary>
        /// When true the worker thread has been told it is time to stop, it will complete it's current run and then end.
        /// </summary>
        private bool Terminated = false;

        /// <summary>
        /// How frequently to output the current event counts in milliseconds
        /// </summary>
        private int _outputInterval = 5000;

        /// <summary>
        /// Once the worker thread starts, if no increment is invoked in this number of second, the worker will self-terminate
        /// </summary>
        private int _terminateAfterInactiveSeconds = int.MaxValue;

        /// <summary>
        /// Once this value is reached in total increments, the eventCounter and total events fields will reset to 0
        /// This is a nice feature as you can say something like "I'm only interested in seeing outputs once you reach
        /// a certain critical size"
        /// </summary>
        private int _resetCountersAfter = Int32.MaxValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys">A list of strings that represent the various events we are counting, whatever those events are</param>
        /// <param name="terminateAfterInactiveSeconds"></param>
        /// <param name="outputInterval"></param>
        /// <param name="resetCountersAfter"></param>
        public ThreadedEventCounter(IList<string> keys, int terminateAfterInactiveSeconds, int outputInterval, int resetCountersAfter)
        {
            _terminateAfterInactiveSeconds = terminateAfterInactiveSeconds;
            _outputInterval = outputInterval;
            _resetCountersAfter = resetCountersAfter;

            ResetEventCounters(keys);

            //WorkerThreadStart start = WorkerThreadStart;
            WorkerThread = new Thread(WorkerThreadStart);
            WorkerThread.Start();
        }

        /// <summary>
        /// Resets the tracked events and total counters to default state.
        /// </summary>
        /// <param name="keys"></param>
        private void ResetEventCounters(ICollection<string> keys)
        {
            var temp = new ConcurrentDictionary<string, int>();
            foreach (var eventKey in keys)
            {
                temp.TryAdd(eventKey, 0);
            }
            EventCounts = temp;
            _currentEventTotal = 0;
        }

        /// <summary>
        /// The total number of increment calls logged.
        /// </summary>
        private int _currentEventTotal;

        /// <summary>
        /// When the last increment call was made. Used for determining time-outs
        /// </summary>
        private DateTime LastIncrement = DateTime.Now;

        private static Mutex incrementMutex = new Mutex();
        /// <summary>
        /// Increment the event in question by 1
        /// </summary>
        /// <param name="eventName"></param>
        public void IncrementEvent(string eventName)
        {
            _currentEventTotal++;
            LastIncrement = DateTime.Now;
            EventCounts[eventName]++;

            if (_currentEventTotal >= _resetCountersAfter)
            {
                //I place the mutex within the reset/display portion to decrease load (we don't want it locking an dunlocking on every increment)
                incrementMutex.WaitOne();

                //Then we have to perform the same check again - becuase WaitOne() doesn't cause a blocked thread to return, just to wait for it's turn to continue into the restricted area
                //as the first thread to 'simultaneously' enter will reset _currentEventTotal, this second check prevents queued threads from re-displaying
                if (_currentEventTotal >= _resetCountersAfter)
                {
                    outputCounts();
                    ResetEventCounters(EventCounts.Keys.ToArray());
                }
                incrementMutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Display the current counts to the console.
        /// </summary>
        private void outputCounts()
        {
            string output = EventCounts.Aggregate("", (current, kvPair) => current + $"{kvPair.Key}:{kvPair.Value} , ");
            Console.WriteLine($"{output}   TOTAL:{_currentEventTotal}");
        }

        /// <summary>
        /// This is the thread-start which gets begins the contant update/output loop
        /// </summary>
        private void WorkerThreadStart()
        {
            while (!Terminated)
            {
                Thread.Sleep(_outputInterval);
                outputCounts();

                if (DateTime.Now - LastIncrement > TimeSpan.FromSeconds(_terminateAfterInactiveSeconds))
                {
                    Terminated = true;
                }
            }
        }

        /// <summary>
        /// Aborts the worker thread (provided it exists). 
        /// </summary>
        public void Close()
        {
            if (WorkerThread != null && WorkerThread.IsAlive)
            {
                WorkerThread.Abort();
            }
        }
    }
}
