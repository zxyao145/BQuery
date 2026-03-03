
export const getDom = (element: string | Element) => {
  let ele: Element | null = null;
  if (typeof element === "string") {
    ele = document.querySelector(element);
  } else {
    ele = element;
  }
  if (ele == null) {
    console.warn(`Element ${element} not found`);
    throw new Error(`Element ${element} not found`);
  }
  return ele;
};

export const attr = (selector: string | Element, key: string, value: string | null = null) => {
    let dom = getDom(selector);
    if (dom) {
        if (value) {
            dom.setAttribute(key, value);
            return value;
        } else {
            return dom.getAttribute(key);
        }
    }
    return null;
}

export const addCls = (selector: Element | string, className: string | Array<string>) => {
    let element = getDom(selector);
    if (element) {
        if (typeof className === "string") {
            element.classList.add(className);
        } else {
            element.classList.add(...className);
        }
    }
}

export const removeCls = (selector: Element | string, clsName: string | Array<string>) => {
    let element = getDom(selector);
    if (element) {
        if (typeof clsName === "string") {
            element.classList.remove(clsName);
        } else {
            element.classList.remove(...clsName);
        }
    }
}

export const css = (element: HTMLElement, name: string | any, value: string | null = null) => {
    if (typeof name === 'string') {
        if (value === null) {
            let style = name;
            let cssAttributes = style.split(";");
            for (let i = 0; i < cssAttributes.length; i++) {
                let cssAttribute = cssAttributes[i];
                if (!cssAttribute) continue;
                let attribute = cssAttribute.split(":");
                element.style.setProperty(attribute[0], attribute[1]);
            }
            return;
        }
        element.style.setProperty(name, value);
    } else {
        for (let key in name) {
            if (name.hasOwnProperty(key)) {
                element.style.setProperty(key, name[key]);
            }
        }
    }
}
