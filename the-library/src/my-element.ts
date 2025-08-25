import { css, html } from "lit";
import { customElement, state } from "lit/decorators.js";
import type { Modal } from "./modal";
import "./modal";
import "./card";
import { Component } from "./component";
/**
 * An example element.
 *
 * @slot - This element has a slot
 * @csspart button - The button
 */
@customElement("my-element")
export class MyElement extends Component {
  @state()
  private modalIsOpen: boolean = false;

  @state()
  private selectedCards: string[] = [];
  /**
   * Copy for the read the docs hint.
   */

  /**
   * The number of times the button has been clicked.
   */

  handleCreateModalIsChanged() {
    this.modalIsOpen = !this.modalIsOpen;
  }

  render() {
    return html`
      <main>
        <p>
          So this might be where the server wants to connect btw
          ${import.meta.env.VITE_API_URL || "Content not reachable"}
        </p>
        <button @click="${this.handleCreateModalIsChanged}">
          Craete Modal
        </button>
        
        <app-modal
          title="create a Modal"
          .isOpen=${this.modalIsOpen}
          @visible=${(e: CustomEvent) => (this.modalIsOpen = e.detail)}
        >
          ${[
            "Integer",
            "Float",
            "Boolean",
            "String",
            "Charcter",
            "Object",
            "File",
            "Relation",
          ].map((t) => {
            return html`
              <app-card
                title="${t}"
                content="Some Some temporary Content for now"
                .handleClick="${() => {
                  this.selectedCards.push(t);
                  console.log(this.selectedCards);
                }}"
              >
                <img slot="image" alt="It does not exist yet" />
              </app-card>
            `;
          })}
        </app-modal>
      </main>
    `;
  }

  static styles = css`
    main {
      width: 100%;
      height: 100%;
      isolation: isolate;
    }
  `;
}

declare global {
  interface HTMLElementTagNameMap {
    "my-element": MyElement;
    "app-modal": Modal;
  }
}
