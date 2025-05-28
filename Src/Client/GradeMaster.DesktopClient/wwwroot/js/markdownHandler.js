let listeners = [];

window.attachLinkInterceptor = (dotNetHelper) => {
    const anchors = document.querySelectorAll("#markdownContent a");
    console.log("[intercept] Found", anchors.length, "anchor(s)");

    anchors.forEach(anchor => {
        const href = anchor.getAttribute("href");
        console.log("[intercept] href:", href);

        // CORRECTED: This should match internal links like /notes/3
        if (href && href.startsWith("/") && !href.startsWith("//")) {
            console.log("[intercept] Attaching listener to", href);

            const handler = function (e) {
                e.preventDefault();
                console.log("[intercept] Intercepted click for", href);
                dotNetHelper.invokeMethodAsync("NavigateFromJs", href);
            };

            anchor.addEventListener("click", handler);
            listeners.push({ anchor, handler });
        }
    });

    if (anchors.length === 0) {
        console.warn("[intercept] No anchor tags found inside #markdownContent.");
    }
};

window.detachLinkInterceptor = () => {
    listeners.forEach(({ anchor, handler }) => {
        anchor.removeEventListener("click", handler);
    });
    console.log("[intercept] Removed", listeners.length, "event listener(s)");
    listeners = [];
};