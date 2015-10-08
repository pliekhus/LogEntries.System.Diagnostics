using Diagnostics.Listeners;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagnostics.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                for (int n = 0; n < 5000; n++)
                {
                    Debug.WriteLine(string.Format("This is test {0}", n));
                }

                throw new Exception("Test Exception");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            LogEntriesListener.ShutdownLogging();
        }
    }
}
