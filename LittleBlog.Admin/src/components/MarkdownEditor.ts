

type PreviewConfig {
    StyleUri: string
}
class MarkPreviewHelper{
    config: PreviewConfig

    constructor(cfg: PreviewConfig) {
        this.config = cfg
    }

    getStyleCSS(): string{

    }

    renderHtmlPage(content: string): string {

        return `
        <!DOCTYPE html>
        <html lang="zh-hans">

        <head>
          <meta charset="UTF-8" />
          <link rel="icon" href="/favicon.ico" />
          <meta name="viewport" content="width=device-width, initial-scale=1.0" />
          <link rel="stylesheet" href="${this.config.StyleUri}">
        </head>

        <body>
          <div id="markdowncontent">
    ${content}
          </div>
        </body>

        </html>
        `;
    };
}
