import "./global";

import Viewport from "./module/Viewport";
import htmlElementHelper from "./module/HtmlElementHelper";
import * as dragHelper from "./module/DragHelper";
import { bindWindowEvents, EventInfo } from "./module/eventHelper";
import { debounce, throttle } from "./module/common";
import * as domHelper from "./module/domHelper";
const version = "10.0.0";


const bQuery = {
  version: version,

  domHelper: domHelper,
  viewport: Viewport,
  elementExtensions: htmlElementHelper,
  drag: dragHelper,

  getUserAgent: () => {
    return navigator.userAgent;
  },

  throttle,
  debounce,

  addWindowEventsListener: bindWindowEvents,

 
};

window.bQuery = bQuery;

export function getBq() {
  return bQuery;
}

export function getViewport() {
  return Viewport;
}

export function getElementExtensions() {
  return htmlElementHelper;
}


