import { apiClient } from './apiClient';

// Wraps the /api/pins endpoints: create, like/unlike, save/unsave, comments.
export const pinsService = {
    getById: (id) => apiClient.get(`/api/pins/${id}`),

    create: ({ title, description, imageUrl, sourceUrl, categoryId, tags = [] }) =>
        apiClient.post('/api/pins', { title, description, imageUrl, sourceUrl, categoryId, tags }),

    like: (id) => apiClient.post(`/api/pins/${id}/like`),
    unlike: (id) => apiClient.delete(`/api/pins/${id}/like`),

    save: (id) => apiClient.post(`/api/pins/${id}/save`),
    unsave: (id) => apiClient.delete(`/api/pins/${id}/save`),

    getComments: (id) => apiClient.get(`/api/pins/${id}/comments`),
    addComment: (id, text) => apiClient.post(`/api/pins/${id}/comments`, { text }),
};
