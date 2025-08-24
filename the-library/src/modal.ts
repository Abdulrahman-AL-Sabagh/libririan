import { css, html, LitElement } from "lit";
import { customElement, property } from "lit/decorators.js";

@customElement("app-modal")
export class Modal extends LitElement {
  @property({ attribute: "title" })
  title: string = "";

  render() {
    console.log("Rendering the MODAL");
    return html`
      <style>
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
      </style>
      <div class="shade">
        <div class="modal">
          <h2>${this.title}</h2>
          <slot></slot>
        </div>
      </div>
    `;
  }

  static styles = css``;
}
