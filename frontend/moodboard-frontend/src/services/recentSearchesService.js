import { apiClient } from './apiClient';

// Wraps GET/POST/DELETE /api/users/me/recent-searches (auth required).
export const recentSearchesService = {
    getAll: () => apiClient.get('/api/users/me/recent-searches'),

    add: (query) => apiClient.post('/api/users/me/recent-searches', { query }),

    clearAll: () => apiClient.delete('/api/users/me/recent-searches'),
};
