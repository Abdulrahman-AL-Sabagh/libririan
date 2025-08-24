import { css, html, LitElement } from "lit";
import { customElement, property } from "lit/decorators.js";

@customElement("app-modal")
export class Modal extends LitElement {
  @property({ attribute: "title" })
  title: string = "";

  @property({ type: Boolean })
  isOpen: boolean = false;

  render() {
    if (!this.isOpen) return;
    console.log("Rendering the MODAL");
    return html`
      <div class="shade">
        <div class="modal">
          <h2>${this.title}</h2>
          <slot></slot>
        </div>
      </div>
    `;
  }

  static styles = css`
    .shade {
      position: fixed;
      top: 0;
      left: 0;
      width: 100vw;
      height: 100vh;
      background-color: rgba(0, 0, 0, 0.3);

      display: flex;
      align-items: center;
      justify-content: center;
    }

    .modal {
      background: white;
      z-index: 10;
      padding: 2rem;
      top: 15%;
      border-radius: 15px;
    }
  `;
}
