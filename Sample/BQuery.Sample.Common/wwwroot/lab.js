// Apparatus only. All demonstrated operations use the public bQuery API.
export const version = () => window.bQuery.version;
export const inspect = (element) =>
	JSON.stringify(
		{
			attribute: element.getAttribute("data-message"),
			classes: element.className,
			style: element.getAttribute("style"),
			focused: document.activeElement?.getAttribute("aria-label"),
		},
		null,
		2,
	);
export const scrollWindowTop = () => window.scrollTo(0, 0);
export const scrollElementTop = (element) => element.scrollTo(0, 0);
export function timing(pad, reference, interval, generation) {
	let alive = true;
	const timers = new Set();
	const start = performance.now();
	const emit = (name) => {
		if (alive)
			reference
				.invokeMethodAsync(
					"Tick",
					generation,
					name,
					Math.round(performance.now() - start),
				)
				.catch((error) => {
					if (alive) console.error(error);
				});
	};
	const throttled = window.bQuery.throttle(() => emit("throttle"), interval);
	const debounced = window.bQuery.debounce(() => emit("debounce"), interval);
	const input = () => {
		emit("raw");
		throttled();
		debounced();
	};
	pad.addEventListener("pointermove", input);
	return {
		burst() {
			for (let i = 0; i < 12; i++) {
				const timer = setTimeout(() => {
					timers.delete(timer);
					if (alive) input();
				}, i * 30);
				timers.add(timer);
			}
		},
		cancel() {
			throttled.cancel();
		},
		dispose() {
			alive = false;
			throttled.cancel();
			for (const timer of timers) clearTimeout(timer);
			timers.clear();
			pad.removeEventListener("pointermove", input);
		},
	};
}
