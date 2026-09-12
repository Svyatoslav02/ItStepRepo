import { apiClient } from './apiClient';

function toQueryString(params = {}) {
    const usp = new URLSearchParams();
    Object.entries(params).forEach(([key, value]) => {
        if (value === undefined || value === null || value === '') return;
        if (Array.isArray(value)) {
            value.forEach((v) => usp.append(key, v));
        } else {
            usp.append(key, value);
        }
    });
    const qs = usp.toString();
    return qs ? `?${qs}` : '';
}

// Wraps GET /api/feed.
export const feedService = {
    getFeed: ({ page = 1, pageSize = 10, categoryId, tagIds, sort = 'newest' } = {}) =>
        apiClient.get(`/api/feed${toQueryString({ page, pageSize, categoryId, tagIds, sort })}`),
};
