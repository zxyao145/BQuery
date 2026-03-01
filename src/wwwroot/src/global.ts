
declare global {
    interface Window {
        bQuery: any;
        bqInit: (() => void) | null;
        DotNet: any;
        autoDebug: Function;
    }
}

export {  }