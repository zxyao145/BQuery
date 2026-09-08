const Viewport = {
	getWidth() {
		return document.documentElement.clientWidth;
	},

	getHeight() {
		return document.documentElement.clientHeight;
	},

	getWidthAndHeight() {
		return [Viewport.getWidth(), Viewport.getHeight()];
	},

	getScrollWidth() {
		return document.documentElement.scrollWidth;
	},

	getScrollHeight() {
		return document.documentElement.scrollHeight;
	},

	getScrollWidthAndHeight() {
		return [Viewport.getScrollWidth(), Viewport.getScrollHeight()];
	},

	getScrollLeft() {
		return document.documentElement.scrollLeft;
	},

	getScrollTop() {
		return document.documentElement.scrollTop;
	},

	getScrollLeftAndTop() {
		return [Viewport.getScrollLeft(), Viewport.getScrollTop()];
	},

	getScrollDistToTop() {
		const scrollTop =
			document.documentElement.scrollTop || document.body.scrollTop || 0;
		return Math.round(scrollTop);
	},

	getScrollDistToBottom() {
		const dist =
			document.documentElement.scrollHeight -
			document.documentElement.scrollTop -
			document.documentElement.clientHeight;
		return Math.round(dist);
	},
};

export default Viewport;
