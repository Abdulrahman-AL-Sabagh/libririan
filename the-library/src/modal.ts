import { css, html, LitElement } from "lit";
import { customElement, property } from "lit/decorators.js";

@customElement("app-modal")
export class Modal extends LitElement {
  @property({ attribute: "title" })
  title: string = "";

  @property({ type: Boolean })
  public isOpen: boolean = false;
  private eventName: string = "visible";
  private async _notify(detail: boolean) {
    this.dispatchEvent(
      new CustomEvent(this.eventName, {
        bubbles: true,
        composed: true,
        detail,
      })
    );
  }

  render() {
    console.log("IS open has changed");
    if (!this.isOpen) return;
    console.log("Rendering the MODAL");

    return html`
      <div class="shade">
        <div
          class="modal"
          tabindex="0"
          @blur=${() => {
            this._notify(false);
          }}
        >
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
