const ctx: Worker = self as any;
ctx.addEventListener('message', ({ data }) => {
    const response = `my worker response to ${data}`;
    ctx.postMessage(response);
});
