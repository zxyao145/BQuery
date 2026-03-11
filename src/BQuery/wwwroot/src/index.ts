import "./global";

import { debounce, throttle } from "./module/common";
import * as dragHelper from "./module/DragHelper";
import * as domHelper from "./module/domHelper";
import * as windowEvents from "./module/eventHelper";
import htmlElementHelper from "./module/HtmlElementHelper";
import Viewport from "./module/Viewport";

const version = "4.0.0";

const bQuery = {
	version: version,

	viewport: Viewport,
	drag: dragHelper,
	windowEvents: windowEvents,
	domHelper: domHelper,
	elementExtensions: htmlElementHelper,

	navigator: {
		getUserAgent: () => {
			return navigator.userAgent;
		},
	},

	throttle,
	debounce,
};

console.log("bQuery Init");
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
