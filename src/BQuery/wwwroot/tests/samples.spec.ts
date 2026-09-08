import { expect, test, type Page } from "@playwright/test";

const routes = [
	"/",
	"/getting-started",
	"/dom",
	"/elements",
	"/viewport",
	"/events",
	"/drag",
	"/utilities",
];
const ready = async (page: Page) =>
	expect(page.getByTestId("runtime").first()).toHaveAttribute(
		"data-interactive",
		"true",
		{ timeout: 30000 },
	);

for (const [name, port, renderer] of [
	["Server", 5301, "Server"],
	["WebAssembly", 5302, "WebAssembly"],
	["Auto", 5303, "Server"],
] as const) {
	test(`${name}: all routes, DOM, geometry, timing, source and responsive layout`, async ({
		page,
		context,
	}) => {
		test.setTimeout(120000);
		const errors: string[] = [];
		page.on("pageerror", (e) => errors.push(e.message));
		page.on("console", (m) => {
			if (m.type() === "error") errors.push(m.text());
		});
		const base = `http://127.0.0.1:${port}`;
		await context.grantPermissions(["clipboard-read", "clipboard-write"]);
		await page.goto(base);
		await ready(page);
		await expect(page.getByTestId("runtime").first()).toHaveAttribute(
			"data-renderer",
			renderer,
		);
		await expect(page.getByTestId("user-agent")).toContainText("Mozilla");
		for (const route of routes) {
			const response = await page.goto(base + route);
			expect(response?.status()).toBe(200);
			await ready(page);
			await expect(page.locator("h1")).toBeVisible();
			await expect(page.locator(".error")).toHaveText(
				Array(await page.locator(".error").count()).fill(""),
			);
			expect(
				await page.evaluate(
					() => document.documentElement.scrollWidth <= innerWidth,
				),
			).toBe(true);
		}
		await page.goto(base + "/dom");
		await ready(page);
		const operation = page.getByLabel("Operation");
		const run = async (op: string) => {
			await operation.selectOption(op);
			await page
				.getByRole("button", { name: "Run operation", exact: true })
				.click();
		};
		await run("Set attribute");
		await expect(page.locator("#dom-specimen")).toHaveAttribute(
			"data-message",
			"Hello, browser",
		);
		await run("Get attribute");
		await expect(page.getByTestId("dom-result")).toHaveText("Hello, browser");
		await run("Remove attribute");
		await expect(page.locator("#dom-specimen")).not.toHaveAttribute(
			"data-message",
		);
		await run("Add class");
		await expect(page.locator("#dom-specimen")).toHaveClass(/highlight/);
		await run("Remove class");
		await expect(page.locator("#dom-specimen")).not.toHaveClass(/highlight/);
		await run("Add classes");
		await expect(page.locator("#dom-specimen")).toHaveClass(/rounded outlined/);
		await run("Remove classes");
		await expect(page.locator("#dom-specimen")).not.toHaveClass(/rounded/);
		for (const op of [
			"Add CSS",
			"Set CSS",
			"Remove CSS",
			"Custom property",
			"CSS object (JS)",
			"CSS declaration (JS)",
			"Selector attribute (JS)",
			"Focus",
			"Legacy focus (JS)",
		]) {
			await run(op);
			await expect(page.locator(".error")).toBeEmpty();
		}
		await expect(page.getByLabel("Focus target")).toBeFocused();
		await page
			.locator("summary")
			.filter({ hasText: "View complete source" })
			.click();
		await page.getByRole("button", { name: "Copy source" }).click();
		await expect(page.getByRole("status")).toHaveText("Copied");
		expect(await page.evaluate(() => navigator.clipboard.readText())).toContain(
			'@page "/dom"',
		);
		await page.goto(base + "/elements");
		await ready(page);
		await expect(page.getByTestId("measurement")).toContainText("320");
		for (const method of await page
			.getByRole("combobox", { name: "API", exact: true })
			.locator("option")
			.allTextContents()) {
			await page
				.getByRole("combobox", { name: "API", exact: true })
				.selectOption(method);
			await page.getByRole("button", { name: "Measure", exact: true }).click();
			await expect(page.locator(".error")).toBeEmpty();
			await expect(page.getByTestId("measurement")).not.toBeEmpty();
		}
		await page.goto(base + "/viewport");
		await ready(page);
		for (const method of await page
			.getByRole("combobox", { name: "API", exact: true })
			.locator("option")
			.allTextContents()) {
			await page
				.getByRole("combobox", { name: "API", exact: true })
				.selectOption(method);
			await page
				.getByRole("button", { name: "Measure", exact: true })
				.first()
				.click();
			await expect(page.locator(".error")).toBeEmpty();
		}
		await page.evaluate(() => window.scrollTo(0, 500));
		await page
			.getByRole("combobox", { name: "API", exact: true })
			.selectOption("GetScrollTopAsync");
		// The lower button keeps the document scrolled while measuring.
		await page
			.getByRole("button", { name: "Measure", exact: true })
			.last()
			.click();
		await expect
			.poll(async () =>
				Number(await page.getByTestId("measurement").textContent()),
			)
			.toBeGreaterThan(0);
		await page.goto(base + "/utilities");
		await ready(page);
		await page.getByRole("button", { name: "Run burst" }).click();
		await expect(page.getByTestId("timing-raw")).toHaveText("12");
		await expect(page.getByTestId("timing-debounce")).toHaveText("1");
		expect(
			Number(await page.getByTestId("timing-throttle").textContent()),
		).toBeLessThan(12);
		await page.getByRole("button", { name: "Reset", exact: true }).click();
		await expect(page.getByTestId("timing-raw")).toHaveText("0");
		await page.getByRole("button", { name: "Run burst" }).click();
		await page
			.getByRole("navigation")
			.getByRole("link", { name: "Browser Info", exact: true })
			.click();
		await expect(page.getByTestId("user-agent")).toContainText("Mozilla");
		await page.goto(base + "/focus");
		await expect(page).toHaveURL(base + "/drag");
		await ready(page);
		await page.setViewportSize({ width: 390, height: 844 });
		for (const route of routes) {
			await page.goto(base + route);
			await ready(page);
			expect(
				await page.evaluate(
					() => document.documentElement.scrollWidth <= innerWidth,
				),
			).toBe(true);
		}
		await page.getByRole("button", { name: "Toggle navigation" }).click();
		await expect(page.getByRole("navigation")).toBeVisible();
		await page
			.getByRole("navigation")
			.getByRole("link", { name: "DOM", exact: true })
			.click();
		await expect(page.locator("h1")).toHaveText("DOM");
		await page.setViewportSize({ width: 1280, height: 900 });
		await page.goto(base + "/dom");
		await ready(page);
		await page.getByRole("link", { name: "Skip to content" }).focus();
		await expect(
			page.getByRole("link", { name: "Skip to content" }),
		).toBeFocused();
		await page.keyboard.press("Enter");
		await expect(page.locator("main")).toBeFocused();
		expect(errors).toEqual([]);
	});
	test(`${name}: all event conversions, independent unsubscribe and drag cleanup`, async ({
		page,
		context,
	}) => {
		test.setTimeout(120000);
		const errors: string[] = [];
		page.on("pageerror", (e) => errors.push(e.message));
		page.on("console", (m) => {
			if (m.type() === "error") errors.push(m.text());
		});
		const base = `http://127.0.0.1:${port}`;
		await page.goto(base);
		await ready(page);
		const session = await context.newCDPSession(page);
		const listeners = async () =>
			(
				await session.send("Runtime.evaluate", {
					expression:
						"Object.fromEntries(['pointermove','click','resize'].map(k=>[k,(getEventListeners(window)[k]||[]).filter(e=>!e.listener.toString().includes('_hitTargetInterceptor')).length]))",
					includeCommandLineAPI: true,
					returnByValue: true,
				})
			).result.value;
		// Exclude only Playwright's known hit-target interceptor, which is installed lazily.
		const baseline = await listeners();
		await page.goto(base + "/events");
		await ready(page);
		await page.getByRole("button", { name: "Start A", exact: true }).click();
		await page.getByRole("button", { name: "Start B", exact: true }).click();
		await page.getByTestId("event-pad").click();
		await expect
			.poll(async () => Number(await page.getByTestId("count-B").textContent()))
			.toBeGreaterThan(0);
		await page.getByRole("button", { name: "Pause A", exact: true }).click();
		const a = await page.getByTestId("count-A").textContent();
		const b = Number(await page.getByTestId("count-B").textContent());
		await page.getByTestId("event-pad").click();
		await expect
			.poll(async () => Number(await page.getByTestId("count-B").textContent()))
			.toBeGreaterThan(b);
		await expect(page.getByTestId("count-A")).toHaveText(a!);
		const names = [
			"mousedown",
			"mouseup",
			"click",
			"dblclick",
			"mouseover",
			"mouseout",
			"mousemove",
			"contextmenu",
			"resize",
			"scroll",
			"focus",
			"blur",
			"touchstart",
			"touchmove",
			"touchend",
			"touchcancel",
			"keydown",
			"keypress",
			"keyup",
		];
		for (const strategy of [
			"Async events",
			"Sync events",
			"Delegate API",
			"JS wildcard",
		]) {
			await page.getByLabel("Subscription").selectOption(strategy);
			await page.getByRole("button", { name: "Start all events" }).click();
			await expect(
				page.locator("p.tag").filter({ hasText: "Listening to all 19 events" }),
			).toBeVisible();
			for (const name of names) {
				await page.evaluate((name) => {
					const init = { bubbles: true };
					let event: Event;
					if (name.startsWith("key"))
						event = new KeyboardEvent(name, {
							...init,
							key: "k",
							code: "KeyK",
						});
					else if (name.startsWith("touch")) {
						const touch = new Touch({
							identifier: 1,
							target: document.body,
							clientX: 100,
							clientY: 120,
						});
						event = new TouchEvent(name, {
							...init,
							touches: [touch],
							changedTouches: [touch],
							targetTouches: [touch],
						});
					} else if (["focus", "blur"].includes(name))
						event = new FocusEvent(name, init);
					else if (["resize", "scroll"].includes(name))
						event = new Event(name, init);
					else
						event = new MouseEvent(name, {
							...init,
							clientX: 123,
							clientY: 234,
						});
					window.dispatchEvent(event);
				}, name);
				await expect(page.getByTestId("event-log")).toContainText(
					`  ${name}  `,
				);
			}
			await page.getByRole("button", { name: "Pause all events" }).click();
			await expect(page.locator(".error")).toHaveText(["", "", ""]);
		}
		await page.getByRole("button", { name: "Dispose scope" }).click();
		for (let i = 0; i < 3; i++) {
			await page
				.getByRole("navigation")
				.getByRole("link", { name: "Drag", exact: true })
				.click();
			await expect(page.getByTestId("drag-state")).toContainText("Bound");
			expect((await listeners()).pointermove).toBe(1);
			for (const api of ["C# BqDrag", "JS bindDrag", "JS addDraggable"]) {
				await page
					.getByRole("combobox", { name: "API", exact: true })
					.selectOption(api);
				const card = page.getByTestId("drag-card");
				await card.scrollIntoViewIfNeeded();
				const before = (await card.boundingBox())!;
				const handle = (await page.getByTestId("drag-handle").boundingBox())!;
				await page.mouse.move(handle.x + 40, handle.y + 20);
				await page.mouse.down();
				await page.mouse.move(handle.x + 110, handle.y + 75, { steps: 8 });
				await page.mouse.up();
				await expect
					.poll(async () => (await card.boundingBox())!.x)
					.toBeGreaterThan(before.x + 40);
				await page.getByRole("button", { name: "Reset position" }).click();
				await expect
					.poll(async () => Math.abs((await card.boundingBox())!.x - before.x))
					.toBeLessThan(2);
			}
			await page
				.getByRole("navigation")
				.getByRole("link", { name: "Browser Info", exact: true })
				.click();
			await expect(page.getByTestId("user-agent")).toContainText("Mozilla");
			await expect.poll(async () => (await listeners()).pointermove).toBe(0);
			await expect.poll(listeners).toEqual(baseline);
		}
		expect(errors).toEqual([]);
	});
}

test("Auto uses WebAssembly on a subsequent visit after resources download", async ({
	page,
}) => {
	test.setTimeout(90000);
	await page.goto("http://127.0.0.1:5303/");
	await ready(page);
	await expect(page.getByTestId("runtime").first()).toHaveAttribute(
		"data-renderer",
		"Server",
	);
	await expect
		.poll(
			async () =>
				page.evaluate(
					() =>
						performance
							.getEntriesByType("resource")
							.filter(
								(e) => e.name.includes("dotnet") && e.name.includes("wasm"),
							).length,
				),
			{ timeout: 60000 },
		)
		.toBeGreaterThan(0);
	await page.waitForTimeout(2000);
	await page.reload();
	await ready(page);
	await expect(page.getByTestId("runtime").first()).toHaveAttribute(
		"data-renderer",
		"WebAssembly",
		{ timeout: 30000 },
	);
});

test("Touch input reaches the event observatory", async ({ browser }) => {
	const context = await browser.newContext({
		hasTouch: true,
		isMobile: true,
		viewport: { width: 390, height: 844 },
	});
	const page = await context.newPage();
	await page.goto("http://127.0.0.1:5301/events");
	await ready(page);
	await page.getByRole("button", { name: "Start all events" }).click();
	await expect(
		page.locator("p.tag").filter({ hasText: "Listening to all 19 events" }),
	).toBeVisible();
	const pad = (await page.getByTestId("event-pad").boundingBox())!;
	const session = await context.newCDPSession(page);
	await session.send("Input.dispatchTouchEvent", {
		type: "touchStart",
		touchPoints: [{ x: pad.x + 50, y: pad.y + 30 }],
	});
	await expect(page.getByTestId("event-log")).toContainText("touchstart");
	await session.send("Input.dispatchTouchEvent", {
		type: "touchMove",
		touchPoints: [{ x: pad.x + 90, y: pad.y + 50 }],
	});
	await expect(page.getByTestId("event-log")).toContainText("touchmove");
	await session.send("Input.dispatchTouchEvent", {
		type: "touchEnd",
		touchPoints: [],
	});
	await expect(page.getByTestId("event-log")).toContainText("touchend");
	await context.close();
});

for (const [name, port] of [
	["Server", 5301],
	["WebAssembly", 5302],
	["Auto", 5303],
] as const) {
	test(`${name}: viewport demo crosses its starting area and stops at browser edges`, async ({
		page,
	}, testInfo) => {
		await page.setViewportSize({ width: 1280, height: 1000 });
		await page.goto(`http://127.0.0.1:${port}/drag`);
		await ready(page);
		await expect(page.getByTestId("drag-state")).toContainText("Bound");
		const card = page.getByTestId("drag-card");
		const handle = page.getByTestId("drag-handle");
		const dragTo = async (x: number, y: number) => {
			await handle.scrollIntoViewIfNeeded();
			const start = (await handle.boundingBox())!;
			await page.mouse.move(start.x + 40, start.y + 20);
			await page.mouse.down();
			await page.mouse.move(x, y, { steps: 10 });
			await page.mouse.up();
		};
		for (const [x, y] of [
			[0, 300],
			[1279, 300],
			[650, 0],
			[650, 999],
		]) {
			await dragTo(x, y);
			await expect
				.poll(
					async () => {
						const b = (await card.boundingBox())!;
						return Math.abs(
							x === 0
								? b.x
								: x === 1279
									? b.x + b.width - 1279
									: y === 0
										? b.y
										: b.y + b.height - 999,
						);
					},
					{ message: `edge ${x},${y}` },
				)
				.toBeLessThanOrEqual(2);
			const box = (await card.boundingBox())!;
			expect(box.x).toBeGreaterThanOrEqual(-1);
			expect(box.y).toBeGreaterThanOrEqual(-1);
			expect(box.x + box.width).toBeLessThanOrEqual(1280);
			expect(box.y + box.height).toBeLessThanOrEqual(1000);
			if (x === 0) {
				expect(box.x).toBeLessThan(
					(await page.getByTestId("drag-stage").boundingBox())!.x,
				);
				// The card must remain visible even where it overlaps the sidebar outside its parent.
				expect(
					await page.evaluate(
						({ x, y }) =>
							!!document
								.elementFromPoint(x, y)
								?.closest('[data-testid="drag-card"]'),
						{ x: box.x + 10, y: box.y + box.height / 2 },
					),
				).toBe(true);
				if (name === "Server")
					await page.screenshot({
						path: testInfo.outputPath("viewport-boundary.png"),
					});
			}
			await page
				.getByRole("button", { name: "Reset position", exact: true })
				.click();
		}
		await page
			.getByRole("checkbox", { name: "Keep inside browser viewport" })
			.uncheck();
		await expect(page.locator(".viewport-guide")).toHaveCount(0);
		await expect(page.locator(".call")).toContainText("InViewport = false");
		await dragTo(0, 300);
		expect((await card.boundingBox())!.x).toBeLessThan(0);
		await page
			.getByRole("button", { name: "Reset position", exact: true })
			.click();
		await expect
			.poll(async () => (await card.boundingBox())!.x)
			.toBeGreaterThan(0);
		await page
			.getByRole("combobox", { name: "Stage", exact: true })
			.selectOption("scrollable");
		await expect(page.getByTestId("drag-stage")).toHaveCSS("overflow", "auto");
		await expect(page.locator("#drag-instructions")).toContainText(
			"clips overflowing content",
		);
		await page
			.getByRole("navigation")
			.getByRole("link", { name: "Browser Info", exact: true })
			.click();
		await expect(page.locator(".viewport-guide")).toHaveCount(0);
	});
}

test("Samples use a plain white layout with demos first", async ({
	page,
}, testInfo) => {
	await page.goto("http://127.0.0.1:5301/");
	await ready(page);
	await expect(page.locator("html")).toHaveCSS(
		"background-color",
		"rgb(255, 255, 255)",
	);
	await expect(page.locator("h1")).toHaveText("Browser Info");
	await expect(page.getByTestId("user-agent")).toBeVisible();
	await expect(page.locator(".hero, .sidebar-note, footer")).toHaveCount(0);
	await page.screenshot({ path: testInfo.outputPath("samples-desktop.png") });
	await page.setViewportSize({ width: 390, height: 844 });
	await expect(page.getByTestId("user-agent")).toBeVisible();
	await page.screenshot({ path: testInfo.outputPath("samples-mobile.png") });
});
