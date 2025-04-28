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