window.checkDescriptionHeight = (elementId, maxHeight) => {
    let element = document.getElementById(elementId);
    if (!element) return false;
    return element.scrollHeight > maxHeight;
};

window.getMaxDescriptionHeight = (elementId) => {
    let element = document.getElementById(elementId);
    if (!element) return 0;
    return element.scrollHeight + 32;
}