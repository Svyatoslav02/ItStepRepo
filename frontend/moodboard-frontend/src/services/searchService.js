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

// Wraps GET /api/search, /api/search/trending, /api/search/categories.
export const searchService = {
    search: ({ q, categoryId, tagId, page = 1, pageSize = 10 } = {}) =>
        apiClient.get(`/api/search${toQueryString({ q, categoryId, tagId, page, pageSize })}`),

    getTrending: (count = 10) =>
        apiClient.get(`/api/search/trending${toQueryString({ count })}`),

    getCategories: () => apiClient.get('/api/search/categories'),
};
