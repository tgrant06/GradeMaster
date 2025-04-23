window.checkDescriptionHeight = (elementId, maxHeight) => {
    let element = document.getElementById(elementId);
    if (!element) return false;
    return element.scrollHeight > maxHeight;
};

window.getMaxDescriptionHeight = (elementId) => {
    let element = document.getElementById(elementId);
    if (!element) return 0;
    return element.scrollHeight + 60; // was 32
}

// not used
// use when the description is expanded
//window.unsetDescriptionHeight = (elementId) => {
//    let element = document.getElementById(elementId);
//    if (!element) return;
//    element.style.maxHeight = "none";
//}

//window.setDescriptionHeight = (elementId) => {
//    let element = document.getElementById(elementId);
//    if (!element) return;
//    element.style.maxHeight = `${window.getMaxDescriptionHeight()}px`;
//}

// use when the description is expanded

const descriptionListener = () => {
    let descriptionArea = document.getElementById("description-area");
    let descriptionText = document.getElementById("description-text");

    if (descriptionArea) {
        descriptionArea.style.height = `${descriptionText.scrollHeight + 60}px`; // was 32
    }
}

window.addDescriptionAreaEventListener = function() {
    window.addEventListener("resize", descriptionListener);
};

window.removeDescriptionAreaEventListener = function () {
    window.removeEventListener("resize", descriptionListener);
}