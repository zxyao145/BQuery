using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BQuery
{
    /// <summary>
    /// window events
    /// </summary>
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
        
        #region window.onresize

        /// <summary>
        /// window.onresize event
        /// first parameter is width,
        /// second first parameter is height
        /// </summary>
        public event Action<double, double> OnResize;
        
        /// <summary>
        /// async window.onresize event
        /// first parameter is width,
        /// second first parameter is height, 
        /// </summary>
        public event Func<double, double, Task> OnResizeAsync;

        internal void InvokeOnResize(double width, double height)
        {
            OnResize?.Invoke(width, height);
            OnResizeAsync?.Invoke(width, height);
        }
        
        #endregion
        
        #region window.onscroll

        /// <summary>
        /// window.onscroll event
        /// </summary>
        public event Action<EventArgs> OnScroll;
        
        /// <summary>
        /// async window.onscroll event
        /// </summary>
        public event Func<EventArgs, Task> OnScrollAsync;
        
        internal void InvokeOnScroll(EventArgs e)
        {
            OnScroll?.Invoke(e);
            OnScrollAsync?.Invoke(e);
        }

        #endregion

        #region OnMouseOver

        /// <summary>
        /// window event
        /// </summary>
        public event Action<MouseEventArgs> OnMouseOver;

        /// <summary>
        /// async window event
        /// </summary>
        public event Func<MouseEventArgs, Task> OnMouseOverAsync;

        internal void InvokeOnMouseOver(MouseEventArgs e)
        {
            OnMouseOver?.Invoke(e);
            OnMouseOverAsync?.Invoke(e);
        }

        #endregion

        #region OnMouseOut

        /// <summary>
        /// window event
        /// </summary>
        public event Action<MouseEventArgs> OnMouseOut;

        /// <summary>
        /// async window event
        /// </summary>
        public event Func<MouseEventArgs, Task> OnMouseOutAsync;

        internal void InvokeOnMouseOut(MouseEventArgs e)
        {
            OnMouseOut?.Invoke(e);
            OnMouseOutAsync?.Invoke(e);
        }

        #endregion

        #region OnContextMenu

        /// <summary>
        /// window event
        /// </summary>
        public event Action<MouseEventArgs> OnContextMenu;

        /// <summary>
        /// async window event
        /// </summary>
        public event Func<MouseEventArgs, Task> OnContextMenuAsync;

        internal void InvokeOnContextMenu(MouseEventArgs e)
        {
            OnContextMenu?.Invoke(e);
            OnContextMenuAsync?.Invoke(e);
        }

        #endregion

        #region OnMouseDown

        /// <summary>
        /// window event
        /// </summary>
        public event Action<MouseEventArgs> OnMouseDown;

        /// <summary>
        /// async window event
        /// </summary>
        public event Func<MouseEventArgs, Task> OnMouseDownAsync;

        internal void InvokeOnMouseDown(MouseEventArgs e)
        {
            OnMouseDown?.Invoke(e);
            OnMouseDownAsync?.Invoke(e);
        }

        #endregion

        #region OnMouseUp

        /// <summary>
        /// window event
        /// </summary>
        public event Action<MouseEventArgs> OnMouseUp;

        /// <summary>
        /// async window event
        /// </summary>
        public event Func<MouseEventArgs, Task> OnMouseUpAsync;

        internal void InvokeOnMouseUp(MouseEventArgs e)
        {
            OnMouseUp?.Invoke(e);
            OnMouseUpAsync?.Invoke(e);
        }

        #endregion

        #region window.OnMouseMove

        /// <summary>
        /// window.mousemove event
        /// </summary>
        public event Action<MouseEventArgs> OnMouseMove;

        /// <summary>
        /// async window.mousemove event
        /// </summary>
        public event Func<MouseEventArgs, Task> OnMouseMoveAsync;

        internal void InvokeOnMouseMove(MouseEventArgs e)
        {
            OnMouseMove?.Invoke(e);
            OnMouseMoveAsync?.Invoke(e);
        }


        #endregion

        #region OnDbClick

        /// <summary>
        /// window.ondbClick event
        /// </summary>
        public event Action<MouseEventArgs> OnDbClick;

        /// <summary>
        /// async window.ondbClick event
        /// </summary>
        public event Func<MouseEventArgs, Task> OnDbClickAsync;

        public void InvokeOnDbClick(MouseEventArgs mouseEventArgs)
        {
            OnDbClick?.Invoke(mouseEventArgs);
            OnDbClickAsync?.Invoke(mouseEventArgs);
        }

        #endregion

        #region OnDbClick

        /// <summary>
        /// window.ondbClick event
        /// </summary>
        public event Action<MouseEventArgs> OnClick;

        /// <summary>
        /// async window.ondbClick event
        /// </summary>
        public event Func<MouseEventArgs, Task> OnClickAsync;

        internal void InvokeOnClick(MouseEventArgs e)
        {
            OnClick?.Invoke(e);
            OnClickAsync?.Invoke(e);
        }
        #endregion


        #region OnClose

        /// <summary>
        /// 
        /// </summary>
        public event Action<EventArgs> OnClose;

        /// <summary>
        /// async event
        /// </summary>
        public event Func<EventArgs, Task> OnCloseAsync;

        internal void InvokeOnClose(EventArgs e)
        {
            OnClose?.Invoke(e);
            OnCloseAsync?.Invoke(e);
        }

        #endregion

        #region OnFocus

        /// <summary>
        /// 
        /// </summary>
        public event Action<FocusEventArgs> OnFocus;

        /// <summary>
        /// async event
        /// </summary>
        public event Func<FocusEventArgs, Task> OnFocusAsync;

        internal void InvokeOnFocus(FocusEventArgs e)
        {
            OnFocus?.Invoke(e);
            OnFocusAsync?.Invoke(e);
        }

        #endregion

        #region OnBlur

        /// <summary>
        /// 
        /// </summary>
        public event Action<FocusEventArgs> OnBlur;

        /// <summary>
        /// async event
        /// </summary>
        public event Func<FocusEventArgs, Task> OnBlurAsync;

        internal void InvokeOnBlur(FocusEventArgs e)
        {
            OnBlur?.Invoke(e);
            OnBlurAsync?.Invoke(e);
        }

        #endregion
        
        #region OnTouchStart

        /// <summary>
        /// 
        /// </summary>
        public event Action<TouchEventArgs> OnTouchStart;

        /// <summary>
        /// async event
        /// </summary>
        public event Func<TouchEventArgs, Task> OnTouchStartAsync;

        internal void InvokeOnTouchStart(TouchEventArgs e)
        {
            OnTouchStart?.Invoke(e);
            OnTouchStartAsync?.Invoke(e);
        }

        #endregion

        #region OnTouchMove

        /// <summary>
        /// 
        /// </summary>
        public event Action<TouchEventArgs> OnTouchMove;

        /// <summary>
        /// async event
        /// </summary>
        public event Func<TouchEventArgs, Task> OnTouchMoveAsync;

        internal void InvokeOnTouchMove(TouchEventArgs e)
        {
            OnTouchMove?.Invoke(e);
            OnTouchMoveAsync?.Invoke(e);
        }

        #endregion

        #region OnTouchEnd

        /// <summary>
        /// 
        /// </summary>
        public event Action<TouchEventArgs> OnTouchEnd;

        /// <summary>
        /// async event
        /// </summary>
        public event Func<TouchEventArgs, Task> OnTouchEndAsync;

        internal void InvokeOnTouchEnd(TouchEventArgs e)
        {
            OnTouchEnd?.Invoke(e);
            OnTouchEndAsync?.Invoke(e);
        }

        #endregion

        #region OnTouchCancel

        /// <summary>
        /// 
        /// </summary>
        public event Action<TouchEventArgs> OnTouchCancel;

        /// <summary>
        /// async event
        /// </summary>
        public event Func<TouchEventArgs, Task> OnTouchCancelAsync;

        internal void InvokeOnTouchCancel(TouchEventArgs e)
        {
            OnTouchCancel?.Invoke(e);
            OnTouchCancelAsync?.Invoke(e);
        }

        #endregion

        #region OnKeyDown

        /// <summary>
        /// 
        /// </summary>
        public event Action<KeyboardEventArgs> OnKeyDown;

        /// <summary>
        /// async event
        /// </summary>
        public event Func<KeyboardEventArgs, Task> OnKeyDownAsync;

        internal void InvokeOnKeyDown(KeyboardEventArgs e)
        {
            OnKeyDown?.Invoke(e);
            OnKeyDownAsync?.Invoke(e);
        }

        #endregion

        #region OnKeyPress

        /// <summary>
        /// 
        /// </summary>
        public event Action<KeyboardEventArgs> OnKeyPress;

        /// <summary>
        /// async event
        /// </summary>
        public event Func<KeyboardEventArgs, Task> OnKeyPressAsync;

        internal void InvokeOnKeyPress(KeyboardEventArgs e)
        {
            OnKeyPress?.Invoke(e);
            OnKeyPressAsync?.Invoke(e);
        }

        #endregion

        #region OnKeyUp

        /// <summary>
        /// 
        /// </summary>
        public event Action<KeyboardEventArgs> OnKeyUp;

        /// <summary>
        /// async event
        /// </summary>
        public event Func<KeyboardEventArgs, Task> OnKeyUpAsync;

        internal void InvokeOnKeyUp(KeyboardEventArgs e)
        {
            OnKeyUp?.Invoke(e);
            OnKeyUpAsync?.Invoke(e);
        }

        #endregion
        
        #region Drag & drop

        //#region OnDragEnter

        ///// <summary>
        ///// 
        ///// </summary>
        //public event Action<DragEventArgs> OnDragEnter;

        ///// <summary>
        ///// async event
        ///// </summary>
        //public event Func<DragEventArgs, Task> OnDragEnterAsync;

        //internal void InvokeOnDragEnter(DragEventArgs e)
        //{
        //    OnDragEnter?.Invoke(e);
        //    OnDragEnterAsync?.Invoke(e);
        //}

        //#endregion

        //#region OnDragLeave

        ///// <summary>
        ///// 
        ///// </summary>
        //public event Action<DragEventArgs> OnDragLeave;

        ///// <summary>
        ///// async event
        ///// </summary>
        //public event Func<DragEventArgs, Task> OnDragLeaveAsync;

        //internal void InvokeOnDragLeave(DragEventArgs e)
        //{
        //    OnDragLeave?.Invoke(e);
        //    OnDragLeaveAsync?.Invoke(e);
        //}

        //#endregion

        //#region OnDragOver

        ///// <summary>
        ///// 
        ///// </summary>
        //public event Action<DragEventArgs> OnDragOver;

        ///// <summary>
        ///// async event
        ///// </summary>
        //public event Func<DragEventArgs, Task> OnDragOverAsync;

        //internal void InvokeOnDragOver(DragEventArgs e)
        //{
        //    OnDragOver?.Invoke(e);
        //    OnDragOverAsync?.Invoke(e);
        //}

        //#endregion

        //#region OnDrop

        ///// <summary>
        ///// 
        ///// </summary>
        //public event Action<DragEventArgs> OnDrop;

        ///// <summary>
        ///// async event
        ///// </summary>
        //public event Func<DragEventArgs, Task> OnDropAsync;


        //internal void InvokeOnDrop(DragEventArgs e)
        //{
        //    OnDrop?.Invoke(e);
        //    OnDropAsync?.Invoke(e);
        //}

        //#endregion

        #endregion
    }
}
