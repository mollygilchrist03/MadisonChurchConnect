const CACHE_NAME = 'madison-connect-v2';
const APP_SHELL = [
    '/',
    '/manifest.json',
    '/css/site.css',
    '/js/site.js',
    '/images/m-logo-new-white.png',
    '/images/apple-touch-icon.png'
];
const CACHE_EXCLUDED = [
    '/Menu',
    '/Login',
    '/Logout',
    '/Register',
    '/Notes',
    '/Sermons',
    '/Feedback'
];

self.addEventListener('install', (event) => {
    event.waitUntil(
        caches.open(CACHE_NAME).then((cache) => cache.addAll(APP_SHELL))
    );
    self.skipWaiting();
});

self.addEventListener('activate', (event) => {
    event.waitUntil(
        caches.keys().then((keys) => Promise.all(
            keys.filter((key) => key !== CACHE_NAME).map((key) => caches.delete(key))
        ))
    );
    self.clients.claim();
});

self.addEventListener('fetch', (event) => {
    if (event.request.method !== 'GET') {
        return;
    }
    const url = new URL(event.request.url);
    if (CACHE_EXCLUDED.some((path) => url.pathname.startsWith(path))) {
        event.respondWith(fetch(event.request));
        return;
    }
    event.respondWith(
        caches.match(event.request).then((cachedResponse) => {
            if (cachedResponse) {
                return cachedResponse;
            }
            return fetch(event.request)
                .then((networkResponse) => {
                    const responseClone = networkResponse.clone();
                    if (event.request.url.startsWith(self.location.origin)) {
                        caches.open(CACHE_NAME).then((cache) => cache.put(event.request, responseClone));
                    }
                    return networkResponse;
                })
                .catch(() => caches.match('/'));
        })
    );
});