export function invokeDownload(fileName: string, contentType: string, contents: string | Uint8Array): void {

    // Convert base64 string to Uint8Array array if the type of contents is string.
    const uint8Array = typeof (contents) === 'string' ? new Uint8Array(atob(contents).split('').map(c => c.charCodeAt(0))) : contents;

    // Wrap it by Blob object.
    const blob = new Blob([uint8Array], { type: contentType });

    // Create "object URL" that is linked to the Blob object.
    const url = URL.createObjectURL(blob);

    // Invoke download.
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();

    // At last, release unused resources.
    URL.revokeObjectURL(url);
}