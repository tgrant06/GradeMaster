(function () {
    const savedTheme = localStorage.getItem("blazorbootstrap-theme");
    if (!savedTheme) {
        return;
    } 
    if (savedTheme === "system") {
        const prefersDark = window.matchMedia("(prefers-color-scheme: dark)").matches;
        const defaultTheme = prefersDark ? "dark" : "light";
        document.documentElement.setAttribute("data-bs-theme", defaultTheme);
    } else {
        document.documentElement.setAttribute("data-bs-theme", savedTheme);
    }
})();

// not used
/*
 else {
    // You could set a default or check user's system preference
    const prefersDark = window.matchMedia("(prefers-color-scheme: dark)").matches;
    const defaultTheme = prefersDark ? "dark" : "light";
    document.documentElement.setAttribute("data-bs-theme", defaultTheme);
    localStorage.setItem("theme", defaultTheme);
}
*/