const pageKeybinds = {};

window.addPageKeybinds = (pageName, dotNetHelper) => {
    const handler = function (event) {
        if (event.ctrlKey) {
            switch (pageName) {
            case "EducationPage":
                handleEducationPageKeys(event, dotNetHelper);
                break;
            case "SubjectPage":
                handleSubjectPageKeys(event, dotNetHelper);
                break;
            case "GradePage":
                handleGradePageKeys(event, dotNetHelper);
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
    let searchElement = getElementById(`searchField${searchFieldName}`);
    if (searchElement) {
        searchElement.focus();
        searchElement.select(); // Selects the text inside
    }
}

function handleEducationPageKeys(event, dotNetHelper) {
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
    case "e":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToEdit");
        break;
    case "b":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateBack");
        break;
    }
}

function handleSubjectPageKeys(event, dotNetHelper) {
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
    case "e":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToEdit");
        break;
    case "b":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateBack");
        break;
    }
}

function handleGradePageKeys(event, dotNetHelper) {
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
    case "e":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateToEdit");
        break;
    case "b":
        event.preventDefault();
        dotNetHelper.invokeMethodAsync("NavigateBack");
        break;
    }
}