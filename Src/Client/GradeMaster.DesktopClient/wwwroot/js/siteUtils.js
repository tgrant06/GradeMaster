//window.scrollToTop = () => {
//    window.scrollTo({ top: 0, behavior: 'smooth' });
//};

window.scrollToTopInstant = () => {
    window.scrollTo({ top: 0, behavior: "auto" }); // Instant scroll
};

const scrollListener = () => {
    const contentElement = document.querySelector(".content");
    const scrollButton = document.getElementById("scrollToTopButton");

    const rect = contentElement.getBoundingClientRect();
    const isAtTop = rect.top >= -10; // Checks if the element is at the top

    if (!scrollButton) return;

    if (isAtTop) {
        scrollButton.classList.remove("show"); // Smoothly fades out
    } else {
        scrollButton.classList.add("show"); // Smoothly fades in
    }
};

window.initializeScrollButton = function () {
    const mainScrollElement = document.querySelector("main");
    const contentElement = document.querySelector(".content");
    const scrollButton = document.getElementById("scrollToTopButton");

    //console.log("Content element:", contentElement);
    //console.log("Scroll button:", scrollButton);

    if (!contentElement || !scrollButton) {
        console.warn("Missing elements! Scroll button will not work.");
        return;
    }


    mainScrollElement.addEventListener("scroll", scrollListener);
};

window.removeMainScrollListener = function () {
    const mainScrollElement = document.querySelector("main");
    if (mainScrollElement) {
        mainScrollElement.removeEventListener("scroll", scrollListener);
    }
};

window.scrollToTop = function () {
    const contentElement = document.querySelector(".content"); // Select the scrollable container
    if (contentElement) {
        contentElement.scrollIntoView({ block: "start", behavior: "smooth" });
    }
};

window.scrollToContentSection = function () {
    const contentElement = document.querySelector("#topPageSubHeaderSticky"); // Select the scrollable container was #noteContentHeader
    if (contentElement) {
        contentElement.scrollIntoView({ block: "start", behavior: "smooth" });
    }
}