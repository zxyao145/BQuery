# BQuery Samples

Eight shared demo pages with a plain white UI and three independently runnable .NET 10 Blazor Web App hosts.

## Start

```sh
pnpm --dir src/BQuery/wwwroot install --frozen-lockfile
pnpm --dir src/BQuery/wwwroot run build
dotnet run --project Sample/BQuery.Sample.Server # http://localhost:5301
dotnet run --project Sample/BQuery.Sample.Wasm   # http://localhost:5302
dotnet run --project Sample/BQuery.Sample.Auto   # http://localhost:5303
```

Choose one host or use separate terminals. Debug builds do not regenerate BQuery JavaScript; run the JS build after library changes. Release builds generate it automatically.

## Five projects

```text
Server ──────────────────> Common ──> BQuery
Wasm ──┐                     ↑
       ├──> Client ───────────┘
Auto ──┘
```

- **Common** owns all routes, layout, experiment components, CSS, helper JS, fonts and embedded source.
- **Client** only bootstraps browser services. It has no fixed render mode or standalone `#app` registration.
- **Server** selects InteractiveServer, **Wasm** selects InteractiveWebAssembly, **Auto** selects InteractiveAuto. Each registers Common routes and keeps prerendering enabled.
- All hosts and Client register `AddBQuery()`. Both Wasm and Auto reference Client. Wasm is a Blazor Web App, not a standalone WebAssembly application.

The runtime badge uses `RendererInfo`, independently of the host's configured mode. Auto initially uses Server when browser resources are unavailable. After resources download, a subsequent visit may use WebAssembly; the mounted page never migrates runtimes.

## Experiments and public coverage

| Route | Executable coverage |
| --- | --- |
| `/` | Bq, IJSRuntime extension and JS user agent APIs; library version and actual renderer |
| `/getting-started` | Registration, scripts, routing, lifecycle, five-project references, constants, DTOs, generator metadata and public coverage table |
| `/dom` | Set/Get/RemoveAttr; single/list Add/RemoveCls; AddCss/Css/RemoveCss; custom properties; JS selector, CSS object/declaration; current and legacy focus; obsolete Attr compatibility |
| `/elements` | All 11 C# element measurement methods, inner/outer variants, JS document left/top helpers, real local scroll container and adjustable dimensions |
| `/viewport` | All nine C# viewport methods plus JS top/bottom distance; actual document scrolling |
| `/events` | All 19 sync and async events; typed delegate registration; bulk add/remove, disposal; JS wildcard registration/removal/disposal; two subscribers independently remove their delegates |
| `/drag` | BqDrag bind/remove/reset; full element/handle, viewport options with a visible browser-edge guide, unclipped starting area, optional scrollable positioned parent; JS bindDrag/addDraggable/removeDraggable/resetDraggableElePosition |
| `/utilities` | Actual raw/throttle/debounce callbacks, interval selection, burst, timeline, reset and throttle cancel |

`/focus` redirects to `/drag`; `/events` remains available. The Getting Started page contains the method/overload/event coverage checklist. Complete source panels read embedded **actual source files**, not parallel copies. The helper JS is also embedded and shown in Timing Utilities. IBM Plex Sans and Mono are self-hosted; their OFL licenses are in `BQuery.Sample.Common/wwwroot/fonts`.

## Lifecycle

Interop starts after interactive rendering. Controls stay disabled during prerendering. Routine event subscribers inject Bq and unsubscribe their own delegates. The all-events panel owns a separate BqEvents, so bulk clearing does not affect other widgets. Event logs and timing timelines retain at most 100 entries. Drag bindings are removed on navigation. Timing cleanup removes input listeners, cancels timers/throttle, and invalidates debounce callbacks before releasing the .NET reference.

## Verify

```sh
dotnet build
dotnet build -c Release
dotnet test -c Release --no-build
pnpm --dir src/BQuery/wwwroot run typecheck
pnpm --dir src/BQuery/wwwroot run lint
pnpm --dir src/BQuery/wwwroot exec playwright install chromium
pnpm --dir src/BQuery/wwwroot test
python3 tests/verify-package.py
```

The browser suite owns all three ports and starts/stops the hosts. It covers direct routes, render modes, source copying, responsive overflow, DOM, geometry, event conversions, independent unsubscribe, repeated drag navigation, timing cleanup and actual touch input. Auto is tested both cold and after browser resources download. CI uses this same configuration.
