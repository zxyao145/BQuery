import "./global";

import Viewport from "./module/ViewPort"
import htmlElementHelper from "./module/HtmlElementHelper"
import * as dragHelper from "./module/DragHelper"
import { bindEvent } from "./module/eventHelper"
import { debounce, throttle } from "./module/common";
import * as domHelper from "./module/domHelper"

(() => {
    var version = "3.0.2";

    //#region viewport
    var bq = {
        version: version,

        ...htmlElementHelper,
        ...dragHelper,

        domHelper,
        viewport: Viewport,
        throttle,
        debounce,

        getUserAgent: () => {
            return navigator.userAgent;
        },

    }

    let hasInited = false;
    var bQueryReady = () => {
        if (!hasInited) {
            hasInited = true;
            window.bqInit = null;
            console.log("bQuery is Ready");

            window.bQuery = bq;
            bindEvent();
        }
    }

    window.bqInit = bQueryReady;
})();

