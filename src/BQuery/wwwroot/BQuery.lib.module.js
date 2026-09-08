let scriptLoading;

export function beforeWebStart() {
    if (window.bQuery) {
        return Promise.resolve();
    }
    if (scriptLoading) {
        return scriptLoading;
    }

    const source = new URL('_content/BQuery/dist/bQuery.min.js', document.baseURI).href;
    const existingScript = Array.from(document.scripts).find(script => script.src === source);
    const script = existingScript ?? document.createElement('script');
    scriptLoading = new Promise((resolve, reject) => {
        const cleanup = () => {
            script.removeEventListener('load', onLoad);
            script.removeEventListener('error', onError);
        };
        const onError = () => {
            cleanup();
            script.remove();
            reject(new Error(`Failed to load BQuery script: ${source}`));
        };
        const onLoad = () => {
            if (!window.bQuery) {
                onError();
                return;
            }
            cleanup();
            resolve();
        };
        script.addEventListener('load', onLoad);
        script.addEventListener('error', onError);
        if (!existingScript) {
            script.src = source;
            document.body.appendChild(script);
        }
    }).catch(error => {
        scriptLoading = undefined;
        throw error;
    });
    return scriptLoading;
}
