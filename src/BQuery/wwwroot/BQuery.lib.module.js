export function beforeWebStart() {
    loadScriptAndStyle();
}

var beforeStartCalled = false;
function loadScriptAndStyle() {
    if (beforeStartCalled) {
        return;
    }

    beforeStartCalled = true;
    if (!document.querySelector('[src$="_content/BQuery/dist/bQuery.min.js"]')) {
        var customScript = document.createElement('script');
        customScript.setAttribute('src', '_content/BQuery/dist/bQuery.min.js');
        document.body.appendChild(customScript);
    }
}