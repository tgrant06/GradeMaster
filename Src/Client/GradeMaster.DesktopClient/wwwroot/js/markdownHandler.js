let listeners = [];

window.attachLinkInterceptor = (dotNetHelper) => {
    const anchors = document.querySelectorAll("#markdownContent a");

    removeScriptTags();
    wrapTableElementInDivContainer();

    anchors.forEach(anchor => {
        const href = anchor.getAttribute("href");

        if (href && href.startsWith("/") && !href.startsWith("//")) {

            const handler = function (e) {
                e.preventDefault();
                dotNetHelper.invokeMethodAsync("NavigateFromJs", href);
            };

            anchor.addEventListener("click", handler);
            listeners.push({ anchor, handler });
        }
    });
};

function removeScriptTags() {
    const markdownContent = document.getElementById("markdownContent");
    if (markdownContent) {
        const scripts = markdownContent.getElementsByTagName("script");
        const scriptsArray = Array.from(scripts);

        scriptsArray.forEach(script => {
            script.parentNode.removeChild(script);
        });
    }
}

function wrapTableElementInDivContainer() {
    const tables = document.querySelectorAll("#markdownContent table");
    tables.forEach(table => {
        const wrapper = document.createElement("div");
        wrapper.className = "gm-md-table-container-responsive";
        table.style = "margin-bottom: 0;";
        table.parentNode.insertBefore(wrapper, table);
        wrapper.appendChild(table);
    });
}

window.detachLinkInterceptor = () => {
    listeners.forEach(({ anchor, handler }) => {
        anchor.removeEventListener("click", handler);
    });
    listeners = [];
};