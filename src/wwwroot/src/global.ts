
declare global {
    interface Window {
        bQuery: any;
        bq: any;
        bqInit: (() => void) | null;
        DotNet: any;
        autoDebug: Function;
    }
}

export {  }