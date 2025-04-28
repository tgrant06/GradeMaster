const pageKeybinds = {};

window.addPageKeybinds = (pageName, dotNetHelper) => {
    const handler = function (event) {
        if (event.ctrlKey) {
            switch (pageName) {
            case "EducationsPage":
                handleEducationsPageKeys(event, dotNetHelper);
                break;
            case "SubjectsPage":
                handleSubjectsPageKeys(event, dotNetHelper);
                break;
            case "GradesPage":
                handleGradesPageKeys(event, dotNetHelper);
                break;
            case "MainLayoutPage":
                handleMainLayoutPageKeys(event, dotNetHelper);
                break;
            }
        }
    };

    document.addEventListener("keydown", handler);
    pageKeybinds[pageName] = handler;
};

window.removePageKeybinds = (pageName) => {
    if (pageKeybinds[pageName]) {
        document.removeEventListener("keydown", pageKeybinds[pageName]);
        delete pageKeybinds[pageName];
    }
};

function focusSearchField(searchFieldName) {
    let searchElement = document.getElementById(`searchField${searchFieldName}`);
    if (searchElement) {
        searchElement.focus();
        searchElement.select(); // Selects the text inside
    }
}

// does not currently work
function handleEscape(event) {
    if (event.key === "Escape") {
        event.preventDefault();
        const active = document.activeElement;
        if (active && (active.tagName === "INPUT" || active.tagName === "TEXTAREA" || active.isContentEditable)) {
            active.blur();
        }
    }
}

function handleEducationsPageKeys(event, dotNetHelper) {
    if (!event.ctrlKey) return;

    switch (event.key.toLowerCase()) {
    case "f":
        event.preventDefault();
        focusSearchField("Education");
        break;
    case "n":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToCreate");
        break;
    case "x":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("ClearSearch");
        break;
    }
}

function handleSubjectsPageKeys(event, dotNetHelper) {
    if (!event.ctrlKey) return;

    switch (event.key.toLowerCase()) {
    case "f":
        event.preventDefault();
        focusSearchField("Subject");
        break;
    case "n":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToCreate");
        break;
    case "x":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("ClearSearch");
        break;
    }
}

function handleGradesPageKeys(event, dotNetHelper) {
    if (!event.ctrlKey) return;

    switch (event.key.toLowerCase()) {
    case "f":
        event.preventDefault();
        focusSearchField("Grade");
        break;
    case "n":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToCreate");
        break;
    case "x":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("ClearSearch");
        break;
    }
}

function handleMainLayoutPageKeys(event, dotNetHelper) {
    if (!event.ctrlKey) return;

    switch (event.key.toLowerCase()) {
    case "o":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("ToggleOffCanvas");
        break;
    }
}