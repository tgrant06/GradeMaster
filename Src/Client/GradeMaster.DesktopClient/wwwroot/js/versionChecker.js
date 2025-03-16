// Use on Pages where version is displayed

async function checkForUpdates() {
    const installedVersion = document.getElementById("installedVersion").innerText.trim();
    const latestVersionElement = document.getElementById("latestAvailableVersion");
    const updateStatusElement = document.getElementById("currentInstalledVersion");

    try {
        const response = await fetch("https://img.shields.io/github/v/release/devt06/GradeMaster");
        const svgText = await response.text();

        const parser = new DOMParser();
        const svgDoc = parser.parseFromString(svgText, "image/svg+xml");
        const titleElement = svgDoc.querySelector("title");

        if (titleElement) {
            const latestVersion = titleElement.textContent.replace("release: v", "").trim();

            // Update latest version UI
            latestVersionElement.innerText = latestVersion;

            latestVersionElement.innerHTML = `<span style="text-decoration: underline; padding-bottom: 1px;">${latestVersion}</span> <i class="bi bi-box-arrow-up-right" style="text-decoration: none;"></i>`;

            // Compare versions
            if (installedVersion === latestVersion) {
                updateStatusElement.innerHTML = "<i class=\"bi bi-check-circle\"></i> Up to Date";
                // updateStatusElement.innerText = " Up to Date";
                updateStatusElement.classList.remove("alert-warning");
                updateStatusElement.classList.add("alert-success");
            } else {
                updateStatusElement.innerHTML = "<i class=\"bi bi-info-circle\"></i> Update Available";
                // updateStatusElement.innerText = "<i class=\"bi bi-exclamation-triangle-fill\"></i> Update Available";
                updateStatusElement.classList.remove("alert-success");
                updateStatusElement.classList.add("alert-warning");
            }
        } else {
            latestVersionElement.innerText = "Error fetching version";
        }
    } catch (error) {
        latestVersionElement.innerText = "Error fetching version";
        console.error("Error fetching latest version:", error);
    }
}

window.checkForUpdates = checkForUpdates;