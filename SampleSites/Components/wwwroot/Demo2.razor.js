export const draw = (canvas) => {
    const ctx = canvas.getContext("2d");
    ctx.fillStyle = "white";
    ctx.fillRect(0, 0, 200, 200);

    for (var i = 0; i <= 200; i += 10) {
        ctx.beginPath();
        ctx.moveTo(0, i);
        ctx.lineTo(200 - i, 0);
        ctx.stroke();

        ctx.beginPath();
        ctx.moveTo(200, i);
        ctx.lineTo(200 - i, 200);
        ctx.stroke();

        ctx.beginPath();
        ctx.moveTo(0, i);
        ctx.lineTo(i, 200);
        ctx.stroke();

        ctx.beginPath();
        ctx.moveTo(200, i);
        ctx.lineTo(i, 0);
        ctx.stroke();
    }
};

export const toDataURL = (canvas, type) => {
    return canvas.toDataURL(type);
}