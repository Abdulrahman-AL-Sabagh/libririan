import { LitElement, css, html } from "lit";
import { customElement, property } from "lit/decorators.js";
import type { Modal } from "./modal";

/**
 * An example element.
 *
 * @slot - This element has a slot
 * @csspart button - The button
 */
@customElement("my-element")
export class MyElement extends LitElement {
  /**
   * Copy for the read the docs hint.
   */
  @property()
  docsHint = "Click on the Vite and Lit logos to learn more";

  /**
   * The number of times the button has been clicked.
   */
  @property({ type: Number })
  count = 0;

  render() {
    return html`
      <main>
        <app-modal title="create a Modal">
          <p>Please show me what is inside my modal</p>
        </app-modal>
      </main>
    `;
  }

  static styles = css`
    :host {
      width: 100%;
      height: 100%
      text-align: center;
      background: purple;
    }
    main {
      width: 100%;
      height: 100%;
    }

  `;
}

declare global {
  interface HTMLElementTagNameMap {
    "my-element": MyElement;
    "app-modal": Modal;
  }
}
