import type { getBq } from "./index";
import type { autoDebug } from "./module/common";

declare global {
	interface Window {
		bQuery: ReturnType<typeof getBq>;
		bq: unknown;
		DotNet: unknown;
		autoDebug: typeof autoDebug;
	}
}
