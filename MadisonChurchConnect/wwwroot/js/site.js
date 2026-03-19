// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

(function () {
    const statusRegion = document.getElementById('app-status');
    const themeToggle = document.querySelector('[data-theme-toggle]');
    const themeStorageKey = 'madison-connect-theme';
    const darkThemeName = 'dark';
    const lightThemeName = 'light';

    function announceStatus(message) {
        if (!statusRegion || !message) {
            return;
        }

        statusRegion.textContent = '';
        window.setTimeout(() => {
            statusRegion.textContent = message;
        }, 50);
    }

    function getPreferredTheme() {
        const storedTheme = window.localStorage.getItem(themeStorageKey);

        if (storedTheme === darkThemeName || storedTheme === lightThemeName) {
            return storedTheme;
        }

        return window.matchMedia('(prefers-color-scheme: dark)').matches ? darkThemeName : lightThemeName;
    }

    function updateThemeToggle(theme) {
        if (!themeToggle) {
            return;
        }

        const isDarkTheme = theme === darkThemeName;
        const icon = themeToggle.querySelector('.theme-toggle-icon');
        const label = themeToggle.querySelector('.theme-toggle-text');

        themeToggle.setAttribute('aria-pressed', String(isDarkTheme));
        themeToggle.setAttribute('aria-label', isDarkTheme ? 'Switch to light mode' : 'Switch to dark mode');

        if (icon) {
            icon.textContent = isDarkTheme ? '🌙' : '☀️';
        }

        if (label) {
            label.textContent = isDarkTheme ? 'Dark' : 'Light';
        }
    }

    function applyTheme(theme, shouldPersist) {
        document.documentElement.setAttribute('data-theme', theme);
        updateThemeToggle(theme);

        if (shouldPersist) {
            window.localStorage.setItem(themeStorageKey, theme);
        }
    }

    const activeTheme = getPreferredTheme();
    applyTheme(activeTheme, false);

    if (themeToggle) {
        themeToggle.addEventListener('click', () => {
            const currentTheme = document.documentElement.getAttribute('data-theme') === darkThemeName ? darkThemeName : lightThemeName;
            const nextTheme = currentTheme === darkThemeName ? lightThemeName : darkThemeName;
            applyTheme(nextTheme, true);
            announceStatus(nextTheme === darkThemeName ? 'Dark mode enabled.' : 'Light mode enabled.');
        });
    }

    const colorSchemeQuery = window.matchMedia('(prefers-color-scheme: dark)');
    colorSchemeQuery.addEventListener('change', (event) => {
        if (window.localStorage.getItem(themeStorageKey)) {
            return;
        }

        applyTheme(event.matches ? darkThemeName : lightThemeName, false);
    });

    window.MadisonConnect = window.MadisonConnect || {};
    window.MadisonConnect.announceStatus = announceStatus;

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
