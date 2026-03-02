using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BQuery.Sample.Common.Pages
{
    public partial class WindowEvents: IAsyncDisposable
    {
        [Inject]
        private BqObject bq {  get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            { 
              bq.Events.OnMouseMoveAsync += Events_OnMouseMove;
              bq.Events.OnClickAsync += Events_OnClick;
              bq.Events.OnDbClickAsync += Events_OnDbClick;
              bq.Events.OnMouseDownAsync += Events_OnMouseDown;
              bq.Events.OnMouseUpAsync += Events_OnMouseUp;
              bq.Events.OnKeyDownAsync += Events_OnKeyDown;
              bq.Events.OnBlurAsync += Events_OnBlur;
              bq.Events.OnFocusAsync += Events_OnFocus;
              bq.Events.OnTouchStartAsync += Events_OnTouchStart;
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private TouchPoint[] _touchPoints;
        private async Task Events_OnTouchStart(TouchEventArgs obj)
        {
            _touchPoints = obj.Touches;
            await InvokeAsync(StateHasChanged);
        }

        private List<string> _focusLogs = new List<string>();
        private async Task Events_OnFocus(FocusEventArgs obj)
        {
            _focusLogs.Add("Focus: " + DateTime.Now);
            await InvokeAsync(StateHasChanged);
        }

        private async Task Events_OnBlur(FocusEventArgs obj)
        {
            _focusLogs.Add("Blur: " + DateTime.Now);
            await InvokeAsync(StateHasChanged);
        }

        private StringBuilder _sb = new StringBuilder();
        private string _curKey;
        private async Task Events_OnKeyDown(KeyboardEventArgs obj)
        {
            _curKey = obj.Key;
            _sb.Append(_curKey);
            await InvokeAsync(StateHasChanged);
        }

        private double _mouseUpX;
        private double _mouseUpY;
        private async Task Events_OnMouseUp(MouseEventArgs obj)
        {
            _mouseUpX = obj.ClientX;
            _mouseUpY = obj.ClientY;
            await InvokeAsync(StateHasChanged);
        }

        private double _mouseDownX;
        private double _mouseDownY;
        private async Task Events_OnMouseDown(MouseEventArgs obj)
        {
            _mouseDownX = obj.ClientX;
            _mouseDownY = obj.ClientY;
            await InvokeAsync(StateHasChanged);
        }




        private double _dbClickX;
        private double _dbClickY;
        private bool _shiftKey;

        private async Task Events_OnDbClick(MouseEventArgs obj)
        {
            _dbClickX = obj.ClientX;
            _dbClickY = obj.ClientY;
            _shiftKey = obj.ShiftKey;
            await InvokeAsync(StateHasChanged);
        }

        private double _clickX;
        private double _clickY;
        private bool _ctrlKey;
        private async Task Events_OnClick(MouseEventArgs obj)
        {
            _clickX = obj.ClientX;
            _clickY = obj.ClientY;
            _ctrlKey = obj.CtrlKey;
            await InvokeAsync(StateHasChanged);
        }

        private double _clientX;
        private double _clientY;
        private double _screenX;
        private double _screenY;

        private async Task Events_OnMouseMove(MouseEventArgs obj)
        {
            _clientX = obj.ClientX;
            _clientY = obj.ClientY;
            _screenX = obj.ScreenX;
            _screenY = obj.ScreenY;
            await InvokeAsync(StateHasChanged);
        }

        public async ValueTask DisposeAsync()
        {
            if(bq != null) 
            {
                await bq.DisposeAsync();
            }
        }
    }
}
