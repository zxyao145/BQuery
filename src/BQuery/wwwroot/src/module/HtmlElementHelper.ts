const ElementWidthAndHeightHelper = {
	//#region width height

	getWidth(element: HTMLElement, outer: boolean) {
		if (outer) {
			return element.offsetWidth;
		} else {
			return element.clientWidth;
		}
	},

	getHeight(element: HTMLElement, outer: boolean) {
		if (outer) {
			return element.offsetHeight;
		} else {
			return element.clientHeight;
		}
	},

	getWidthAndHeight(element: HTMLElement, outer: boolean) {
		return [
			ElementWidthAndHeightHelper.getWidth(element, outer),
			ElementWidthAndHeightHelper.getHeight(element, outer),
		];
	},

	//#endregion
};

const ElementScrollHelper = {
	//#region element's Scroll Width and Height

	getScrollWidth(element: HTMLElement) {
		return element.scrollWidth;
	},

	getScrollHeight(element: HTMLElement) {
		return element.scrollHeight;
	},

	getScrollWidthAndHeight(element: HTMLElement) {
		return [
			ElementScrollHelper.getScrollWidth(element),
			ElementScrollHelper.getScrollHeight(element),
		];
	},

	//#endregion

	//#region Scroll Left and top

	getScrollLeft(element: HTMLElement) {
		return element.scrollLeft;
	},

	getScrollTop(element: HTMLElement) {
		return element.scrollTop;
	},

	getScrollLeftAndTop(element: HTMLElement) {
		return [
			ElementScrollHelper.getScrollLeft(element),
			ElementScrollHelper.getScrollTop(element),
		];
	},

	//#endregion
};

const ElementPositionHelper = {
	//#region element's position

	getPositionInViewport(element: HTMLElement) {
		var rect = element.getBoundingClientRect();
		return {
			x: rect.left,
			y: rect.top,
			width: rect.width,
			height: rect.height,
		};
	},

	getElementLeftInDoc(element: HTMLElement) {
		var actualLeft = element.offsetLeft;
		var current = element.offsetParent as HTMLElement;

		while (typeof current !== "undefined" && current !== null) {
			actualLeft += current.offsetLeft;
			current = current.offsetParent as HTMLElement;
		}

		return actualLeft;
	},

	getElementTopInDoc(element: HTMLElement) {
		var actualTop = element.offsetTop;
		var current = element.offsetParent as HTMLElement;
		while (typeof current !== "undefined" && current !== null) {
			actualTop += current.offsetTop;
			current = current.offsetParent as HTMLElement;
		}

		return actualTop;
	},

	getPositionInDoc(element: HTMLElement) {
		if (!element) {
			console.warn("getPositionInDoc: element is null or undefined");
			return {
				x: 0,
				y: 0,
				width: 0,
				height: 0,
			};
		}
		var rect = {
			x: ElementPositionHelper.getElementLeftInDoc(element),
			y: ElementPositionHelper.getElementTopInDoc(element),
			width: ElementWidthAndHeightHelper.getWidth(element, true),
			height: ElementWidthAndHeightHelper.getHeight(element, true),
		};
		return rect;
	},

	//#endregion position
};

const ElementHelper = {
	// [obsolete]: Blazor has a native implementation from .net6
	focus(element: HTMLElement) {
		element.focus();
	},
};

const htmlElementHelper = {
	...ElementWidthAndHeightHelper,
	...ElementScrollHelper,
	...ElementPositionHelper,
	...ElementHelper,
};

export default htmlElementHelper;
