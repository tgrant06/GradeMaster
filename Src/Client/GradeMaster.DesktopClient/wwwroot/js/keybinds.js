const pageKeybinds = {};

window.addPageKeybinds = (pageName, dotNetHelper) => {
    const handler = function (event) {
        if (event.ctrlKey) {
            switch (pageName) {
                case "HomePage":
                    handleHomePageKeys(event, dotNetHelper);
                    break;
                case "EducationsPage":
                    handleEducationsPageKeys(event, dotNetHelper);
                    break;
                case "SubjectsPage":
                    handleSubjectsPageKeys(event, dotNetHelper);
                    break;
                case "GradesPage":
                    handleGradesPageKeys(event, dotNetHelper);
                    break;
                case "NotesPage":
                    handleNotesPageKeys(event, dotNetHelper);
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
                case "NoteDetailPage":
                    handleNoteDetailPage(event, dotNetHelper);
                    break;
                case "FormComponent":
                    handleFormComponentKeys(event, dotNetHelper);
                    break;
                case "NoteFormComponent":
                    handleNoteFormComponentKeys(event, dotNetHelper);
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
    const searchElement = document.getElementById(`searchField${searchFieldName}`);
    if (searchElement) {
        searchElement.focus();
        searchElement.select(); // Selects the text inside
    }
}

function focusSelectField(selectFieldName) {
    const selectElement = document.getElementById(`selectField${selectFieldName}`);
    if (selectElement) {
        selectElement.focus();
    }
}

function focusAndSelectFilterMenu() {
    const filterElement = document.getElementById("filterMenu");
    if (filterElement) {
        filterElement.focus();
        filterElement.click();
    }
}

// used for if still focused on a field the field data is not saved when pressing Ctrl+S (should solve this)
function focusOnSubmitButton() {
    const formSubmitBtn = document.getElementById("formSubmitBtn");
    if (formSubmitBtn) {
        formSubmitBtn.focus();
    }
}

function clickOnTab(tabName) {
    const tabBtn = document.getElementById(`${tabName}Tab`);
    if (tabBtn) {
        tabBtn.click();
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

// does not currently work
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

    if (event.altKey) return;

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

    if (event.altKey) return;

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

    if (event.altKey) return;

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

function handleNotesPageKeys(event, dotNetHelper) {
    if (!event.ctrlKey) return;

    if (event.altKey) return;

    switch (event.key.toLowerCase()) {
        case "f":
            event.preventDefault();
            focusSearchField("Note");
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

        case "Ctrl+Alt+n":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("GoToNotesPage");
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

        case "Ctrl+d":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("ToggleDescriptionHeight");
            break;

        case "Ctrl+Shift+d":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("DeleteObject");
            break;

        case "Ctrl+Shift+y":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("CopyPageUrl");
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

        case "Ctrl+d":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("ToggleDescriptionHeight");
            break;

        case "Ctrl+Shift+d":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("DeleteObject");
            break;

        case "Ctrl+Shift+a":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("NavigateToEducation");
            break;

        case "Ctrl+Shift+y":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("CopyPageUrl");
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

        case "Ctrl+d":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("ToggleDescriptionHeight");
            break;

        case "Ctrl+Shift+d":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("DeleteObject");
            break;

        case "Ctrl+Shift+q":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("NavigateToSubject");
            break;

        case "Ctrl+Shift+a":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("NavigateToEducation");
            break;

        case "Ctrl+Shift+y":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("CopyPageUrl");
            break;
    }

    // maybe add keybind to add new grade with the same Subject
}

function handleNoteDetailPage(event, dotNetHelper) {
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
        case "Ctrl+d":
            event.preventDefault();
            window.scrollToContentSection();
            break;
        case "Ctrl+Shift+d":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("DeleteObject");
            break;
        case "Ctrl+Shift+y":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("CopyPageUrl");
            break;
    }
}

function handleFormComponentKeys(event, dotNetHelper) {
    if (!event.ctrlKey) return; // allow Ctrl or Alt combos
    let shortcut = "";
    if (event.ctrlKey) shortcut += "Ctrl+";

    shortcut += event.key.toLowerCase();
    switch (shortcut) {
        case "Ctrl+s":
            event.preventDefault();
            focusOnSubmitButton();
            dotNetHelper.invokeMethodAsync("SubmitForm");
            break;
    }
}

function handleNoteFormComponentKeys(event, dotNetHelper) {
    if (!event.ctrlKey) return; // allow Ctrl or Alt combos

    let shortcut = "";

    if (event.ctrlKey) shortcut += "Ctrl+";
    if (event.shiftKey) shortcut += "Shift+";

    shortcut += event.key.toLowerCase();
    switch (shortcut) {
        case "Ctrl+s":
            event.preventDefault();
            focusOnSubmitButton();
            dotNetHelper.invokeMethodAsync("SubmitForm");
            break;
        case "Ctrl+Shift+w":
            event.preventDefault();
            clickOnTab("write");
            break;
        case "Ctrl+Shift+v":
            event.preventDefault();
            clickOnTab("preview");
            break;
    }
}

function handleHomePageKeys(event, dotNetHelper) {
    if (!event.ctrlKey || event.altKey) return;
    let shortcut = "";

    if (event.ctrlKey) shortcut += "Ctrl+";
    if (event.shiftKey) shortcut += "Shift+";

    shortcut += event.key.toLowerCase();
    switch (shortcut) {
        case "Ctrl+e":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("NavigateToEducationCreate");
            break;
        case "Ctrl+s":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("NavigateToSubjectCreate");
            break;
        case "Ctrl+g":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("NavigateToGradeCreate");
            break;
        case "Ctrl+f":
            event.preventDefault();
            focusSelectField("Education");
            break;
        case "Ctrl+q":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("ClearFilterState");
            break;
        case "Ctrl+Shift+f":
            event.preventDefault();
            focusAndSelectFilterMenu();
            break;
        case "Ctrl+Shift+y":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("CopyPageUrl");
            break;
        case "Ctrl+d":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("NavigateToEducationDetail");
            break;
        case "Ctrl+x":
            event.preventDefault();
            dotNetHelper.invokeMethodAsync("ReloadPageData");
            break;
    }
}
