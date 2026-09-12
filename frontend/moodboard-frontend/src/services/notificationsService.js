import { apiClient } from './apiClient';

function toQueryString(params = {}) {
    const usp = new URLSearchParams();
    Object.entries(params).forEach(([key, value]) => {
        if (value === undefined || value === null || value === '') return;
        usp.append(key, value);
    });
    const qs = usp.toString();
    return qs ? `?${qs}` : '';
}

// Wraps /api/notifications and /api/users/me/notification-preferences.
export const notificationsService = {
    getNotifications: ({ page = 1, pageSize = 10, type } = {}) =>
        apiClient.get(`/api/notifications${toQueryString({ page, pageSize, type })}`),

    markAsRead: (id) => apiClient.post(`/api/notifications/${id}/read`),
    markAllAsRead: () => apiClient.post('/api/notifications/read-all'),

    getPreferences: () => apiClient.get('/api/users/me/notification-preferences'),

    // partial: only the keys you want to change, e.g. { pushLikes: false }
    updatePreferences: (partial) =>
        apiClient.put('/api/users/me/notification-preferences', partial),
};
