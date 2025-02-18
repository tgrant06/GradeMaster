window.scrollToTop = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
};

window.scrollToTopInstant = () => {
    window.scrollTo({ top: 0, behavior: 'auto' }); // Instant scroll
};