export function drawSpriteOnCanvas(canvasId, pixelArray) {
    const canvas = document.getElementById(canvasId);
    if (!canvas) return;
    const ctx = canvas.getContext('2d');

    // 1. Reset any transforms (so scale/rotate from last draw don't accumulate)
    if (typeof ctx.resetTransform === 'function') {
        ctx.resetTransform();
    }

    // 2. Clear the entire canvas
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    // 3. Draw the new image data
    // Option A: create blank ImageData then set its pixels
    // const img = ctx.createImageData(16, 16);
    // img.data.set(pixelArray);
    // ctx.putImageData(img, 0, 0);

    // Option B (more concise): build it directly
    const img = new ImageData(
        new Uint8ClampedArray(pixelArray),
        8,
        8
    );
    ctx.putImageData(img, 0, 0);
}
