using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestHighsCsharp
{
    public class TestableHighsSolver : HighsTemp.HighsLpSolver
    {
        private const string testsupportlibname = "highs_csharp_testing";

        /// <summary>Makes a predefined set of event callbacks</summary>
        /// <param name="callback_func">Pointer to the callback function to be called</param>
        /// <returns>Returns the number of times interrupt was set to true by the callback function.</returns>
        [DllImport(testsupportlibname)]
        private static extern int test_callbacks(IntPtr callback_func);

        /// <summary>Makes a predefined set of event callbacks for testing purposes.</summary>
        /// <returns>Returns the number of times interrupt was set to true by an event handler.</returns>
        public int TriggerTestEvents()
        {
            var cbFunc = this.CallbackFunctionPtr;
            return test_callbacks(cbFunc);
        }
    }
}
