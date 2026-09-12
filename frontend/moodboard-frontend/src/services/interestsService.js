import { apiClient } from './apiClient';

// Wraps GET /api/interests and POST /api/users/me/interests.
export const interestsService = {
    getAll: () => apiClient.get('/api/interests'),

    saveMine: (interestIds) =>
        apiClient.post('/api/users/me/interests', { interestIds }),
};
