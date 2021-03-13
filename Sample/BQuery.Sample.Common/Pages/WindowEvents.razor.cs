using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace BQuery.Sample.Common.Pages
{
    public partial class WindowEvents
    {
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Bq.Events.OnMouseMove += Events_OnMouseMove;
                Bq.Events.OnClick += Events_OnClick;
                Bq.Events.OnDbClick += Events_OnDbClick;
                Bq.Events.OnMouseDown += Events_OnMouseDown;
                Bq.Events.OnMouseUp += Events_OnMouseUp;
                Bq.Events.OnKeyDown += Events_OnKeyDown;
                Bq.Events.OnBlur += Events_OnBlur;
                Bq.Events.OnFocus += Events_OnFocus;
                Bq.Events.OnTouchStart += Events_OnTouchStart;
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private TouchPoint[] _touchPoints;
        private void Events_OnTouchStart(TouchEventArgs obj)
        {
            _touchPoints = obj.Touches;
            StateHasChanged();
        }

        private List<string> _focusLogs = new List<string>();
        private void Events_OnFocus(FocusEventArgs obj)
        {
            _focusLogs.Add("Focus: " + DateTime.Now);
            StateHasChanged();
        }

        private void Events_OnBlur(FocusEventArgs obj)
        {
            _focusLogs.Add("Blur: " + DateTime.Now);
            StateHasChanged();
        }

        private StringBuilder _sb = new StringBuilder();
        private string _curKey;
        private void Events_OnKeyDown(KeyboardEventArgs obj)
        {
            _curKey = obj.Key;
            _sb.Append(_curKey);
            StateHasChanged();
        }

        private double _mouseUpX;
        private double _mouseUpY;
        private void Events_OnMouseUp(MouseEventArgs obj)
        {
            _mouseUpX = obj.ClientX;
            _mouseUpY = obj.ClientY;
            StateHasChanged();
        }

        private double _mouseDownX;
        private double _mouseDownY;
        private void Events_OnMouseDown(MouseEventArgs obj)
        {
            _mouseDownX = obj.ClientX;
            _mouseDownY = obj.ClientY;
            StateHasChanged();
        }




        private double _dbClickX;
        private double _dbClickY;
        private bool _shiftKey;

        private void Events_OnDbClick(MouseEventArgs obj)
        {
            _dbClickX = obj.ClientX;
            _dbClickY = obj.ClientY;
            _shiftKey = obj.ShiftKey;
            StateHasChanged();
        }

        private double _clickX;
        private double _clickY;
        private bool _ctrlKey;
        private void Events_OnClick(MouseEventArgs obj)
        {
            _clickX = obj.ClientX;
            _clickY = obj.ClientY;
            _ctrlKey = obj.CtrlKey;
            StateHasChanged();
        }

        private double _clientX;
        private double _clientY;
        private double _screenX;
        private double _screenY;

        private void Events_OnMouseMove(MouseEventArgs obj)
        {
            _clientX = obj.ClientX;
            _clientY = obj.ClientY;
            _screenX = obj.ScreenX;
            _screenY = obj.ScreenY;
            StateHasChanged();
        }


    }
}
