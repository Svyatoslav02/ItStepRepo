import { apiClient } from './apiClient';

export const authService = {
    register: (FullName, Email, Password) =>
        apiClient.post('/api/auth/register', { FullName, Email, Password }),

    login: (Email, Password) =>
        apiClient.post('/api/auth/login', { Email, Password }),
};