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

export const attr = (
	selector: string | Element,
	key: string,
	value: string | null = null,
) => {
	const dom = getDom(selector);
	if (dom) {
		if (value) {
			dom.setAttribute(key, value);
			return value;
		} else {
			return dom.getAttribute(key);
		}
	}
	return null;
};

export const setAttr = (
	selector: string | Element,
	key: string,
	value: string,
) => {
	const dom = getDom(selector);
	if (dom) {
		dom.setAttribute(key, value);
	}
};

export const getAttr = (selector: string | Element, key: string) => {
	const dom = getDom(selector);
	if (dom) {
		return dom.getAttribute(key);
	}
	return null;
};

export const removeAttr = (selector: string | Element, key: string) => {
	const dom = getDom(selector);
	if (dom) {
		dom.removeAttribute(key);
	}
};

export const addCls = (
	selector: Element | string,
	className: string | Array<string>,
) => {
	const element = getDom(selector);
	if (element) {
		if (typeof className === "string") {
			element.classList.add(className);
		} else {
			element.classList.add(...className);
		}
	}
};

export const removeCls = (
	selector: Element | string,
	clsName: string | Array<string>,
) => {
	const element = getDom(selector);
	if (element) {
		if (typeof clsName === "string") {
			element.classList.remove(clsName);
		} else {
			element.classList.remove(...clsName);
		}
	}
};

export const css = (
	element: HTMLElement,
	name: string | any,
	value: string | null = null,
) => {
	if (typeof name === "string") {
		if (value === null) {
			const style = name;
			const cssAttributes = style.split(";");
			for (let i = 0; i < cssAttributes.length; i++) {
				const cssAttribute = cssAttributes[i];
				if (!cssAttribute) continue;
				const attribute = cssAttribute.split(":");
				element.style.setProperty(attribute[0], attribute[1]);
			}
			return;
		}
		element.style.setProperty(name, value);
	} else {
		for (const key in name) {
			if (Object.hasOwn(name, key)) {
				element.style.setProperty(key, name[key]);
			}
		}
	}
};
