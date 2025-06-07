export function drawSpriteOnCanvas(canvasId, pixelArray) {
    const canvas = document.getElementById(canvasId);
    if (canvas != null) {
        const ctx = canvas.getContext('2d');
        const img = ctx.createImageData(16, 16);
        img.data.set(pixelArray);
        ctx.putImageData(img, 0, 0);
    }
}