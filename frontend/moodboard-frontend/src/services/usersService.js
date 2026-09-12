import { apiClient } from './apiClient';

// Wraps GET/PUT /api/users/me.
export const usersService = {
    getMe: () => apiClient.get('/api/users/me'),

    updateMe: ({ fullName, email, avatarUrl } = {}) =>
        apiClient.put('/api/users/me', { fullName, email, avatarUrl }),
};
