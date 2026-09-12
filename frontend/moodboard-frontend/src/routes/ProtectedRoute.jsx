import React from "react";
import { Navigate } from "react-router-dom";
import { isAuthenticated } from "../utils/auth";

/**
 * Wraps a route element and redirects to /login when there is no auth
 * token in storage. Usage:
 *
 *   <Route path="/home" element={<ProtectedRoute><HomePage /></ProtectedRoute>} />
 */
export default function ProtectedRoute({ children }) {
    if (!isAuthenticated()) {
        return <Navigate to="/login" replace />;
    }
    return children;
}
