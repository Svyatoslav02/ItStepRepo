// Small helpers around the auth token / user object kept in localStorage.
// Centralizing this avoids every page reaching into localStorage directly
// with slightly different key names.

const TOKEN_KEY = "authToken";
const USER_KEY = "authUser";

export function getToken() {
    return localStorage.getItem(TOKEN_KEY);
}

export function setToken(token) {
    if (token) {
        localStorage.setItem(TOKEN_KEY, token);
    }
}

export function getUser() {
    const raw = localStorage.getItem(USER_KEY);
    if (!raw) return null;
    try {
        return JSON.parse(raw);
    } catch {
        return null;
    }
}

export function setUser(user) {
    if (user) {
        localStorage.setItem(USER_KEY, JSON.stringify(user));
    }
}

// Convenience helper used right after login/register, where the API
// returns { token, user }.
export function saveSession(authResult) {
    if (!authResult) return;
    if (authResult.token) setToken(authResult.token);
    if (authResult.user) setUser(authResult.user);
}

export function clearSession() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
}

export function isAuthenticated() {
    return Boolean(getToken());
}
