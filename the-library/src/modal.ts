import { css, html } from "lit";
import { customElement, property } from "lit/decorators.js";
import { Component } from "./component";

@customElement("app-modal")
export class Modal extends Component {
  @property({ type: Boolean }) isOpen = false;
  @property({ attribute: "title" }) title = "";

  private _notify(detail: boolean) {
    this.dispatchEvent(
      new CustomEvent("visible", {
        bubbles: true,
        composed: true,
        detail,
      })
    );
  }

  _handleKeyDown = (e: KeyboardEvent) => {
    if (e.key === "Escape" && this.isOpen) this._notify(false);
  };

  connectedCallback() {
    super.connectedCallback();
    window.addEventListener("keydown", this._handleKeyDown);
  }

  disconnectedCallback() {
    super.disconnectedCallback();
    window.removeEventListener("keydown", this._handleKeyDown);
  }

  render() {
    if (!this.isOpen) return;

    return html`
      <div class="shade" @click=${() => this._notify(false)}>
        <div class="modal" @click=${(e: Event) => e.stopPropagation()}>
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
      z-index: 10;
      padding: 2rem;
      border-radius: 15px;
    }
  `;
}
