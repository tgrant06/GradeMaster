window.initializeGridAnimation = () => {
    const gridContainer = document.querySelector(".component-card-grid");

    if (gridContainer) {
        const gridAnimation = animateCSSGrid.wrapGrid(gridContainer, {
            duration: 300,
            easing: "circOut" // good options: easeOut, backOut, backInOut, circOut
        });

        let previousColumnCount = getComputedStyle(gridContainer).gridTemplateColumns.split(' ').length;

        const resizeListener = () => {
            const currentColumnCount = getComputedStyle(gridContainer).gridTemplateColumns.split(' ').length;
            if (currentColumnCount !== previousColumnCount) {
                previousColumnCount = currentColumnCount;
                gridAnimation.forceGridAnimation();
            }
        };

        window.addEventListener("resize", resizeListener);

        // Clean up the event listener when the component is disposed
        return () => {
            window.removeEventListener("resize", resizeListener);
        };
    } else {
        console.error("No grid found");
    }
};