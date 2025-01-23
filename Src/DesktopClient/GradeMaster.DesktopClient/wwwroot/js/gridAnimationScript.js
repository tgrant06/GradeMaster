document.addEventListener('DOMContentLoaded', (event) => {
    const grid = document.querySelector(".component-card-grid");

    if (grid) {
        const observer = new MutationObserver(() => {
            grid.style.transition = 'none';
            grid.offsetHeight; // Trigger reflow
            grid.style.transition = 'all 0.3s ease';
        });

        observer.observe(grid, { childList: true, subtree: true });

        // Initial animation
        const items = grid.children;
        for (let i = 0; i < items.length; i++) {
            items[i].classList.add('animated-card');
        }
    }
});