import { apiClient } from './apiClient';

// Wraps /api/users/me/privacy, /blocked-users and /data-export.
export const privacyService = {
    getSettings: () => apiClient.get('/api/users/me/privacy'),

    updateSettings: ({ privateAccount, searchVisibility, contentVisibility }) =>
        apiClient.put('/api/users/me/privacy', { privateAccount, searchVisibility, contentVisibility }),

    getBlockedUsers: () => apiClient.get('/api/users/me/blocked-users'),
    blockUser: (userId) => apiClient.post('/api/users/me/blocked-users', { userId }),
    unblockUser: (blockedUserId) => apiClient.delete(`/api/users/me/blocked-users/${blockedUserId}`),

    requestDataExport: () => apiClient.post('/api/users/me/data-export'),
};
