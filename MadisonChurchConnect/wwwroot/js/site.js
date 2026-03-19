// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

(function () {
    const statusRegion = document.getElementById('app-status');
    const themeStorageKey = 'madison-theme';
    const darkThemeMediaQuery = window.matchMedia('(prefers-color-scheme: dark)');

    function getStoredTheme() {
        try {
            return window.localStorage.getItem(themeStorageKey);
        } catch {
            return null;
        }
    }

    function storeTheme(theme) {
        try {
            if (theme) {
                window.localStorage.setItem(themeStorageKey, theme);
            } else {
                window.localStorage.removeItem(themeStorageKey);
            }
        } catch {
            // Ignore storage failures and fall back to system preference.
        }
    }

    function getPreferredTheme() {
        const storedTheme = getStoredTheme();
        if (storedTheme === 'dark' || storedTheme === 'light') {
            return storedTheme;
        }

        return darkThemeMediaQuery.matches ? 'dark' : 'light';
    }

    function applyTheme(theme) {
        const normalizedTheme = theme === 'dark' ? 'dark' : 'light';
        const root = document.documentElement;
        const themeColorMeta = document.querySelector('meta[name="theme-color"]');
        const appleStatusBarMeta = document.querySelector('meta[name="apple-mobile-web-app-status-bar-style"]');
        const isDark = normalizedTheme === 'dark';

        root.dataset.theme = normalizedTheme;
        root.style.colorScheme = normalizedTheme;

        if (themeColorMeta) {
            themeColorMeta.setAttribute('content', isDark ? '#121212' : '#4267B2');
        }

        if (appleStatusBarMeta) {
            appleStatusBarMeta.setAttribute('content', isDark ? 'black-translucent' : 'default');
        }

        document.querySelectorAll('[data-theme-toggle]').forEach((button) => {
            const icon = button.querySelector('[data-theme-toggle-icon]');
            const label = button.querySelector('[data-theme-toggle-label]');
            const value = button.querySelector('[data-theme-toggle-value]');
            const nextTheme = isDark ? 'light' : 'dark';
            const nextLabel = isDark ? 'Light Mode' : 'Dark Mode';
            const currentLabel = isDark ? 'Dark' : 'Light';

            button.setAttribute('aria-pressed', String(isDark));
            button.setAttribute('aria-label', `Switch to ${nextLabel}`);
            button.dataset.nextTheme = nextTheme;

            if (icon) {
                icon.setAttribute('src', isDark ? '/images/sun-solid-full.svg' : '/images/moon-solid-full.svg');
            }

            if (label) {
                label.textContent = nextLabel;
            }

            if (value) {
                value.textContent = currentLabel;
            }
        });
    }

    function toggleTheme() {
        const nextTheme = getPreferredTheme() === 'dark' ? 'light' : 'dark';
        storeTheme(nextTheme);
        applyTheme(nextTheme);
        announceStatus(`Theme changed to ${nextTheme} mode.`);
    }

    function announceStatus(message) {
        if (!statusRegion || !message) {
            return;
        }

        statusRegion.textContent = '';
        window.setTimeout(() => {
            statusRegion.textContent = message;
        }, 50);
    }

    window.MadisonConnect = window.MadisonConnect || {};
    window.MadisonConnect.announceStatus = announceStatus;
    window.MadisonConnect.applyTheme = applyTheme;
    window.MadisonConnect.toggleTheme = toggleTheme;

    applyTheme(getPreferredTheme());

    if (typeof darkThemeMediaQuery.addEventListener === 'function') {
        darkThemeMediaQuery.addEventListener('change', (event) => {
            if (getStoredTheme()) {
                return;
            }

            applyTheme(event.matches ? 'dark' : 'light');
        });
    }

    document.querySelectorAll('[data-theme-toggle]').forEach((button) => {
        button.addEventListener('click', toggleTheme);
    });

    document.querySelectorAll('.more-icon-link').forEach((link) => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const href = this.getAttribute('href');
            const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

            if (!href || href === '#') {
                return;
            }

            if (prefersReducedMotion) {
                window.location.href = href;
                return;
            }

            const iframe = document.createElement('iframe');
            iframe.src = href;
            iframe.classList.add('page-transition-overlay');
            iframe.setAttribute('title', 'Loading menu');
            iframe.setAttribute('aria-hidden', 'true');
            document.body.appendChild(iframe);

            iframe.addEventListener('animationend', () => {
                window.location.href = href;
            });
        });
    });

    if ('serviceWorker' in navigator) {
        window.addEventListener('load', () => {
            navigator.serviceWorker.register('/service-worker.js')
                .then(() => announceStatus('Offline support is ready.'))
                .catch(() => announceStatus('Offline support could not be enabled.'));
        });
    }

    window.addEventListener('online', () => announceStatus('You are back online.'));
    window.addEventListener('offline', () => announceStatus('You are offline. Cached pages remain available.'));
})();
