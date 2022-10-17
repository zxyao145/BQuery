
export const autoDebug = (invoke: Function) => {
    var isDebug = sessionStorage.getItem("isDebug") === "true";
    if (isDebug) {
        invoke();
    }
}


/**
 * 防抖，适合多次事件一次响应的情况
 * 应用场合：提交按钮的点击事件。
 * @param fn
 * @param wait
*/
export function debounce(fn: Function, wait = 1000) {
    var timer: number | null = null;
    return function (...args: any[]) {
        var context = this as any;
        if (timer) {
            clearTimeout(timer);
        }
        timer = window.setTimeout(() => {
            //var arr = Array.prototype.slice.call(args);
            fn.apply(context, args);
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
export function throttle(fn: Function, threshold = 160) {
    let timeout: number | null;
    var start = +new Date;
    return function (...args: any[]) {
        let context = this, curTime = +new Date() - 0;
        //总是干掉事件回调
        window.clearTimeout(timeout!);
        if (curTime - start >= threshold) {
            //只执行一部分方法，这些方法是在某个时间段内执行一次
            fn.apply(context, args);
            start = curTime;
        }
        else {
            //让方法在脱离事件后也能执行一次
            timeout = window.setTimeout(() => {
                //@ts-ignore
                fn.apply(this, args);
            }, threshold);
        }
    };
}


export const getClassStaticFunc = (type: object) => {
    const res: {
        [index: string]: { message: Function };
    } = {};
    for (let key in Object.getOwnPropertyDescriptors(type)) {
        if (["length", "name", "prototype"].includes(key)) {
            continue;
        }
        autoDebug(() => {
            console.log(key);
        });
        //@ts-ignore
        if (typeof type[key] === "function") {
            //@ts-ignore
            res[key] = type[key];
        }
    }
    return res;
}