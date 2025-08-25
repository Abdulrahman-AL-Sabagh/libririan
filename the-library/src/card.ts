import { css, html } from "lit";
import { customElement, property } from "lit/decorators.js";
import { Component } from "./component";

@customElement("app-card")
export class Card extends Component {
  @property({ attribute: true })
  public title: string = "";

  @property({ attribute: true })
  public content: string = "";

  @property({ attribute: false })
  public handleClick: VoidFunction = () => {};

  render() {
    return html`
      <div class="card" @click="${this.handleClick}">
        <slot name="image"></slot>
        <h2>${this.title}</h2>
        <p>${this.content}</p>
      </div>
    `;
  }
  static styles = css`
    .card {
      border: 2px solid white;
      padding: 0.5rem;
    }
  `;
}
