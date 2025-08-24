import { LitElement, css, html } from "lit";
import { customElement, property, state } from "lit/decorators.js";
import type { Modal } from "./modal";

/**
 * An example element.
 *
 * @slot - This element has a slot
 * @csspart button - The button
 */
@customElement("my-element")
export class MyElement extends LitElement {
  @state
  modalIsOpen: boolean = false;

  /**
   * Copy for the read the docs hint.
   */

  /**
   * The number of times the button has been clicked.
   */

  handleCreateModal() {}

  render() {
    return html`
      <main>
        <p>
          So this might be where the server wants to connect btw
          ${import.meta.env.VITE_API_URL || "Content not reachable"}
        </p>
        <button @click="${this.handleCreateModal}">Craete Modal</button>
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
