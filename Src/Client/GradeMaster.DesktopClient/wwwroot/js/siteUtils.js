//window.scrollToTop = () => {
//    window.scrollTo({ top: 0, behavior: 'smooth' });
//};

window.scrollToTopInstant = () => {
    window.scrollTo({ top: 0, behavior: 'auto' }); // Instant scroll
};

window.initializeScrollButton = function () {
    let mainScrollElement = document.querySelector("main");
    let contentElement = document.querySelector(".content");
    let scrollButton = document.getElementById("scrollToTopButton");

    //console.log("Content element:", contentElement);
    //console.log("Scroll button:", scrollButton);

    if (!contentElement || !scrollButton) {
        console.warn("Missing elements! Scroll button will not work.");
        return;
    }

    mainScrollElement.addEventListener("scroll", function () {
        let rect = contentElement.getBoundingClientRect();
        let isAtTop = rect.top >= -10; // Checks if the element is at the top

        if (isAtTop) {
            scrollButton.classList.remove("show"); // Smoothly fades out
        } else {
            scrollButton.classList.add("show"); // Smoothly fades in
        }

        //if (isAtTop) {
        //    scrollButton.style.display = "none"; // Hide button when at top
        //} else {
        //    scrollButton.style.display = "block"; // Show button when scrolled
        //}
    });
};

window.scrollToTop = function () {
    let contentElement = document.querySelector(".content"); // Select the scrollable container
    if (contentElement) {
        contentElement.scrollIntoView({ block: 'start', behavior: 'smooth' });
    }
};