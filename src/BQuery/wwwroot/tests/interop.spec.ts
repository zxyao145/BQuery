import { expect, test, type Page } from "@playwright/test";
import { readFileSync } from "node:fs";
import { fileURLToPath } from "node:url";
import type {} from "../src/global";

const bundle = fileURLToPath(new URL("../dist/bQuery.min.js", import.meta.url));
const initializer = readFileSync(
	new URL("../BQuery.lib.module.js", import.meta.url),
	"utf8",
);

async function fixture(page: Page, html = "") {
	await page.route("http://bquery.test/", (route) =>
		route.fulfill({
			contentType: "text/html",
			body: `<!doctype html><html><body>${html}</body></html>`,
		}),
	);
	await page.goto("http://bquery.test/");
	await page.addScriptTag({ path: bundle });
}

test("CSS removal clears ordinary and custom properties", async ({ page }) => {
	await fixture(page);
	const result = await page.evaluate(() => {
		const element = document.createElement("div");
		element.style.cssText = "color:red;--color:red";
		window.bQuery.domHelper.css(element, "color", null);
		window.bQuery.domHelper.css(element, "--color", null);
		return [element.style.color, element.style.getPropertyValue("--color")];
	});
	expect(result).toEqual(["", ""]);
});

test("reset before the first drag preserves inline styles", async ({
	page,
}) => {
	await fixture(page);
	const result = await page.evaluate(() => {
		const element = document.createElement("div");
		element.style.cssText =
			"color:red;width:123px;height:45px;touch-action:pan-x";
		document.body.append(element);
		window.bQuery.drag.bindDrag(element);
		window.bQuery.drag.resetDraggableElePosition(element);
		const touchAction = element.style.touchAction;
		window.bQuery.drag.removeDraggable(element);
		return {
			width: element.style.width,
			color: element.style.color,
			touchAction,
			restoredTouchAction: element.style.touchAction,
		};
	});
	expect(result).toEqual({
		width: "123px",
		color: "red",
		touchAction: "none",
		restoredTouchAction: "pan-x",
	});
});

for (const scrolled of [false, true]) {
	test(`drag preserves its initial position inside a positioned parent (scrolled=${scrolled})`, async ({
		page,
	}) => {
		await fixture(
			page,
			'<div style="position:absolute;left:100px;top:180px;width:500px;height:400px;border:4px solid;overflow:auto"><div style="width:700px;height:600px"><div id="drag" style="width:120px;height:60px;margin-left:80px;margin-top:90px;background:blue">Drag</div></div></div>',
		);
		await page.evaluate((scroll) => {
			const element = document.querySelector<HTMLElement>("#drag")!;
			if (scroll) {
				element.parentElement!.parentElement!.scrollTo(30, 40);
			}
			window.bQuery.drag.bindDrag(element, { inViewport: false });
		}, scrolled);
		const before = (await page.locator("#drag").boundingBox())!;
		await page.mouse.move(before.x + 10, before.y + 10);
		await page.mouse.down();
		const after = (await page.locator("#drag").boundingBox())!;
		expect(after.x).toBeCloseTo(before.x, 0);
		expect(after.y).toBeCloseTo(before.y, 0);
		await page.mouse.move(before.x + 60, before.y + 40);
		await page.mouse.up();
		const moved = (await page.locator("#drag").boundingBox())!;
		expect(moved.x).toBeCloseTo(before.x + 50, 0);
		expect(moved.y).toBeCloseTo(before.y + 30, 0);
	});
}

test("unbinding cancels pending drag callbacks", async ({ page }) => {
	await page.clock.install({ time: new Date("2026-01-01T00:00:00Z") });
	await fixture(
		page,
		'<div id="drag" style="width:100px;height:50px">Drag</div>',
	);
	await page.clock.pauseAt(new Date("2026-01-01T00:01:00Z"));
	await page.evaluate(() =>
		window.bQuery.drag.bindDrag(document.querySelector<HTMLElement>("#drag")!),
	);
	await page.locator("#drag").click();
	await page.evaluate(() => {
		window.dispatchEvent(new Event("resize"));
		window.dispatchEvent(new Event("resize"));
		const element = document.querySelector<HTMLElement>("#drag")!;
		window.bQuery.drag.removeDraggable(element);
		element.style.transform = "translate3d(777px, 0px, 0px)";
	});
	await page.clock.runFor(100);
	expect(
		await page.locator("#drag").evaluate((element) => element.style.transform),
	).toBe("translate3d(777px, 0px, 0px)");
});

test("reset after dragging restores the original position and padding", async ({
	page,
}) => {
	await fixture(
		page,
		'<div id="drag" style="position:relative;width:120px;height:60px;padding-bottom:17px;margin:20px;touch-action:pan-x">Drag</div>',
	);
	const before = (await page.locator("#drag").boundingBox())!;
	await page.evaluate(() =>
		window.bQuery.drag.bindDrag(document.querySelector<HTMLElement>("#drag")!),
	);
	await page.mouse.move(before.x + 10, before.y + 10);
	await page.mouse.down();
	await page.mouse.move(before.x + 90, before.y + 60);
	await page.mouse.up();
	await page.evaluate(() =>
		window.bQuery.drag.resetDraggableElePosition(
			document.querySelector<HTMLElement>("#drag")!,
		),
	);
	expect(await page.locator("#drag").boundingBox()).toEqual(before);
	await expect(page.locator("#drag")).toHaveCSS("padding-bottom", "17px");
	await expect(page.locator("#drag")).toHaveCSS("touch-action", "none");
	await page.evaluate(() =>
		window.bQuery.drag.removeDraggable(
			document.querySelector<HTMLElement>("#drag")!,
		),
	);
	await expect(page.locator("#drag")).toHaveCSS("touch-action", "pan-x");
});

test("drag remains in the viewport after movement and resize", async ({
	page,
}) => {
	await fixture(
		page,
		'<div id="drag" style="position:absolute;left:100px;top:100px;width:200px;height:150px">Drag</div>',
	);
	await page.evaluate(() =>
		window.bQuery.drag.bindDrag(document.querySelector<HTMLElement>("#drag")!),
	);
	await page.mouse.move(110, 110);
	await page.mouse.down();
	await page.mouse.move(1800, 1400);
	await page.mouse.up();
	const moved = (await page.locator("#drag").boundingBox())!;
	expect(moved.x + moved.width).toBeLessThanOrEqual(page.viewportSize()!.width);
	expect(moved.y + moved.height).toBeLessThanOrEqual(
		page.viewportSize()!.height,
	);
	await page.setViewportSize({ width: 400, height: 300 });
	await expect
		.poll(async () => {
			const box = (await page.locator("#drag").boundingBox())!;
			return (
				box.x >= 0 &&
				box.y >= 0 &&
				box.x + box.width <= 400 &&
				box.y + box.height <= 300
			);
		})
		.toBe(true);
});

test("a new scope does not receive an old throttled event", async ({
	page,
}) => {
	await page.clock.install({ time: new Date("2026-01-01T00:00:00Z") });
	await fixture(page);
	await page.clock.pauseAt(new Date("2026-01-01T00:01:00Z"));
	await page.evaluate(() => {
		const events = window.bQuery.windowEvents;
		events.addWindowEventListener({ name: "mousemove" }, "old", {
			invokeMethodAsync: async () => {},
		});
		window.dispatchEvent(new MouseEvent("mousemove", { clientX: 314 }));
		window.dispatchEvent(new MouseEvent("mousemove", { clientX: 314 }));
		events.disposeWindowEventsListener("old");
		events.addWindowEventListener({ name: "mousemove" }, "new", {
			invokeMethodAsync: async () => {
				document.body.dataset.received = "true";
			},
		});
	});
	await page.clock.runFor(100);
	expect(await page.locator("body").getAttribute("data-received")).toBeNull();
});

test("removing one scope keeps the other scope subscribed", async ({
	page,
}) => {
	await fixture(page);
	const result = await page.evaluate(() => {
		let first = 0,
			second = 0;
		const events = window.bQuery.windowEvents;
		events.addWindowEventListener({ name: "click" }, "first", {
			invokeMethodAsync: async () => {
				first++;
			},
		});
		events.addWindowEventListener({ name: "click" }, "second", {
			invokeMethodAsync: async () => {
				second++;
			},
		});
		events.disposeWindowEventsListener("first");
		window.dispatchEvent(new MouseEvent("click"));
		return { first, second };
	});
	expect(result).toEqual({ first: 0, second: 1 });
});

test("initialization waits for the script and shares concurrent requests", async ({
	page,
}) => {
	let release!: () => void;
	const gate = new Promise<void>((resolve) => {
		release = resolve;
	});
	let requests = 0;
	await page.route("http://bquery.test/**", async (route) => {
		const path = new URL(route.request().url()).pathname;
		if (path.endsWith("BQuery.lib.module.js")) {
			await route.fulfill({
				contentType: "text/javascript",
				body: initializer,
			});
		} else if (path.endsWith("bQuery.min.js")) {
			requests++;
			await gate;
			await route.fulfill({ contentType: "text/javascript", path: bundle });
		} else {
			await route.fulfill({
				contentType: "text/html",
				body: "<!doctype html><body></body>",
			});
		}
	});
	await page.goto("http://bquery.test/");
	await page.evaluate(async () => {
		const initializerPath = "/BQuery.lib.module.js";
		const { beforeWebStart } = await import(initializerPath);
		Promise.all([beforeWebStart(), beforeWebStart()]).then(() => {
			document.body.dataset.ready = "true";
		});
	});
	await expect.poll(() => requests).toBe(1);
	expect(await page.locator("body").getAttribute("data-ready")).toBeNull();
	release();
	await expect(page.locator("body")).toHaveAttribute("data-ready", "true");
	expect(await page.evaluate(() => typeof window.bQuery)).toBe("object");
});

test("initialization can retry after a script load failure", async ({
	page,
}) => {
	let attempts = 0;
	await page.route("http://bquery.test/**", async (route) => {
		const path = new URL(route.request().url()).pathname;
		if (path.endsWith("BQuery.lib.module.js")) {
			await route.fulfill({
				contentType: "text/javascript",
				body: initializer,
			});
		} else if (path.endsWith("bQuery.min.js")) {
			if (++attempts === 1) await route.abort();
			else
				await route.fulfill({ contentType: "text/javascript", path: bundle });
		} else
			await route.fulfill({
				contentType: "text/html",
				body: "<!doctype html><body></body>",
			});
	});
	await page.goto("http://bquery.test/");
	const result = await page.evaluate(async () => {
		const initializerPath = "/BQuery.lib.module.js";
		const { beforeWebStart } = await import(initializerPath);
		let failed = false;
		try {
			await beforeWebStart();
		} catch {
			failed = true;
		}
		await beforeWebStart();
		return { failed, ready: typeof window.bQuery };
	});
	expect(result).toEqual({ failed: true, ready: "object" });
	expect(attempts).toBe(2);
});
