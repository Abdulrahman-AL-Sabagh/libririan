import { LitElement, type CSSResultGroup } from "lit";
import { resetStyles } from "./reset";

export class Component extends LitElement {
  private static _styles: CSSResultGroup;

  static get styles(): CSSResultGroup {
    const derivedStyles = this._styles || [];
    return [
      resetStyles,
      ...(Array.isArray(derivedStyles) ? derivedStyles : [derivedStyles]),
    ];
  }

  static set styles(styles: CSSResultGroup) {
    this._styles = styles;
  }
}
