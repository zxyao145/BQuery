using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BQuery
{
    public class BqEvents
    {
        private BqEvents()
        {
            
        }
        private static BqEvents _instance;
        private static int _lock = 0;
        internal static BqEvents CreateInstance()
        {
            if (_instance == null)
            {
                if (Interlocked.CompareExchange(ref _lock, 1, 0) == 0)
                {
                    _instance = new BqEvents();
                    Interlocked.Increment(ref _lock);
                }

                while (Interlocked.CompareExchange(ref _lock, 1, 1) == 1)
                {
                    Thread.Sleep(100);
                }
            }

            return _instance;
        }


        /// <summary>
        /// window.onresize event
        /// </summary>
        public event Action<int, int> OnResize;

        /// <summary>
        /// async window.onresize event 
        /// </summary>
        public event Func<int, int, Task> OnResizeAsync;

        /// <summary>
        /// window.onscroll event
        /// </summary>
        public event Action<EventArgs> OnScroll;

        /// <summary>
        /// async window.onscroll event
        /// </summary>
        public event Func<EventArgs, Task> OnScrollAsync;


        internal void InvokeOnResize(int width, int height)
        {
            OnResize?.Invoke(width, height);
            OnResizeAsync?.Invoke(width, height);
        }

        internal void InvokeOnScroll(EventArgs e)
        {
            OnScroll?.Invoke(e);
            OnScrollAsync?.Invoke(e);
        }
    }
}
