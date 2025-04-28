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
            case "EducationDetailPage":
                handleEducationDetailPageKeys(event, dotNetHelper);
                break;
            case "SubjectDetailPage":
                handleSubjectDetailPageKeys(event, dotNetHelper);
                break;
            case "GradeDetailPage":
                handleGradeDetailPageKeys(event, dotNetHelper);
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
function handleEscapeV1(event) {
    if (event.key === "Escape") {
        event.preventDefault();
        const active = document.activeElement;
        if (active && (active.tagName === "INPUT" || active.tagName === "TEXTAREA" || active.isContentEditable)) {
            active.blur();
        }
    }
}

function handleEscape(event) {
    if (event.code === "Escape") {
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
    let shortcut = "";

    if (event.ctrlKey) shortcut += "Ctrl+";
    if (event.altKey) shortcut += "Alt+";

    shortcut += event.key.toLowerCase();

    switch (shortcut) {
    case "Ctrl+o":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("ToggleOffCanvas");
        break;

    case "Ctrl+t":
        event.preventDefault();
        window.scrollToTop();
        break;

    case "Ctrl+b":
        event.preventDefault();
        window.history.back();
        break;

    case "Ctrl+Alt+h":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("GoToHomePage");
        break;

    case "Ctrl+Alt+e":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("GoToEducationsPage");
        break;

    case "Ctrl+Alt+s":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("GoToSubjectsPage");
        break;

    case "Ctrl+Alt+g":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("GoToGradesPage");
        break;

    case "Ctrl+Alt+q":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("GoToSettingsPage");
        break;
    }
}

function handleEducationDetailPageKeys(event, dotNetHelper) {
    if (!event.ctrlKey) return;

    let shortcut = "";

    if (event.ctrlKey) shortcut += "Ctrl+";
    if (event.shiftKey) shortcut += "Shift+";

    shortcut += event.key.toLowerCase();

    switch (shortcut) {
    case "Ctrl+e":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToEdit");
        break;

    case "Ctrl+n":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToCreate");
        break;

    case "Ctrl+Shift+d":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("DeleteObject");
        break;
    }
}

function handleSubjectDetailPageKeys(event, dotNetHelper) {
    if (!event.ctrlKey && !event.altKey) return; // now allow Ctrl **or** Alt combos

    let shortcut = "";

    if (event.ctrlKey) shortcut += "Ctrl+";
    if (event.altKey) shortcut += "Alt+";
    if (event.shiftKey) shortcut += "Shift+";

    shortcut += event.key.toLowerCase();

    switch (shortcut) {
    case "Ctrl+e":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToEdit");
        break;

    case "Ctrl+n":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToCreate");
        break;

    case "Ctrl+Shift+d":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("DeleteObject");
        break;

    case "Ctrl+Shift+e":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToEducation");
        break;
    }
}

function handleGradeDetailPageKeys(event, dotNetHelper) {
    if (!event.ctrlKey && !event.altKey) return; // allow Ctrl or Alt combos

    let shortcut = "";

    if (event.ctrlKey) shortcut += "Ctrl+";
    if (event.altKey) shortcut += "Alt+";
    if (event.shiftKey) shortcut += "Shift+";

    shortcut += event.key.toLowerCase();

    switch (shortcut) {
    case "Ctrl+e":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToEdit");
        break;

    case "Ctrl+Shift+d":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("DeleteObject");
        break;

    case "Ctrl+Shift+q":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToSubject");
        break;

    case "Ctrl+Shift+e":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToEducation");
        break;
    }

    // maybe add keybind to add new grade with the same Subject
}
