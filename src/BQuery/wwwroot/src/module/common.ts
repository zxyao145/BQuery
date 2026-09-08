export const autoDebug = (invoke: () => void) => {
	var isDebug = sessionStorage.getItem("isDebug") === "true";
	if (isDebug) {
		invoke();
	}
};

/**
 * 防抖，适合多次事件一次响应的情况
 * 应用场合：提交按钮的点击事件。
 * @param fn
 * @param wait
 */
export function debounce<TThis, TArgs extends unknown[]>(
	fn: (this: TThis, ...args: TArgs) => unknown,
	wait = 1000,
) {
	var timer: number | null = null;
	return function (this: TThis, ...args: TArgs) {
		if (timer) {
			clearTimeout(timer);
		}
		timer = window.setTimeout(() => {
			//var arr = Array.prototype.slice.call(args);
			fn.apply(this, args);
		}, wait);
	};
}

/**
 * 节流，强制函数以固定的速率执行
 * 会用在比input, keyup更频繁触发的事件中，如resize, touchmove, mousemove, scroll。
 * throttle 会强制函数以固定的速率执行。因此这个方法比较适合应用于动画相关的场景。
 * @param fn
 * @param threshold
 */
export function throttle<TThis, TArgs extends unknown[]>(
	fn: (this: TThis, ...args: TArgs) => unknown,
	threshold = 160,
) {
	let timeout: number | null = null;
	var start = Date.now();

	const throttled = function (this: TThis, ...args: TArgs) {
		const curTime = Date.now();
		//总是干掉事件回调
		if (timeout !== null) {
			window.clearTimeout(timeout);
		}
		if (curTime - start >= threshold) {
			//只执行一部分方法，这些方法是在某个时间段内执行一次
			fn.apply(this, args);
			start = curTime;
		} else {
			//让方法在脱离事件后也能执行一次
			timeout = window.setTimeout(() => {
				timeout = null;
				fn.apply(this, args);
			}, threshold);
		}
	};

	throttled.cancel = () => {
		if (timeout !== null) {
			window.clearTimeout(timeout);
			timeout = null;
		}
	};

	return throttled;
}
