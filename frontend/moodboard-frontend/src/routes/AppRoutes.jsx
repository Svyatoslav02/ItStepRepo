import { Routes, Route } from 'react-router-dom'
import LoadingScreen from '../pages/LoadingScreen'
import InspirationLoading from '../pages/InspirationLoading'
import LoginPage from '../pages/LoginPage.jsx'
import SignUpPage from '../pages/SignUpPage'
import HomePage from '../pages/HomePage'
import InterestsPage from '../pages/InterestsPage.jsx'
import DiscoverPage from '../pages/DiscoverPage.jsx'
import AiWelcome from '../pages/AiWelcome.jsx'
import NotificationsEmail from '../pages/NotificationsEmail.jsx'
import ContentPreferences from '../pages/ContentPreferences.jsx'
import PushNotifications from '../pages/PushNotifications'
import WelcomePage from '../pages/WelcomePage.jsx'
import NotFound from '../pages/NotFound.jsx'
import CreatePostPage from '../pages/CreatePostPage.jsx'
import HomeSearchUI from "../pages/HomeSearchUI.jsx";
import ProtectedRoute from './ProtectedRoute.jsx'

export default function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<LoadingScreen />} />
            <Route path="/inspiration" element={<InspirationLoading />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/signup" element={<SignUpPage />} />
            <Route path="/home" element={<ProtectedRoute><HomePage /></ProtectedRoute>} />
            <Route path="/interests" element={<InterestsPage />} />
            <Route path="/discover" element={<DiscoverPage />} />
            <Route path="/ai-welcome" element={<AiWelcome />} />
            <Route path="/notifications-email" element={<ProtectedRoute><NotificationsEmail /></ProtectedRoute>} />
            <Route path="/content-preferences" element={<ProtectedRoute><ContentPreferences /></ProtectedRoute>} />
            <Route path="/notifications-push" element={<ProtectedRoute><PushNotifications /></ProtectedRoute>} />
            <Route path="/welcome" element={<WelcomePage />} /> 
            <Route path="*" element={<NotFound />} />
            <Route path="/home-search" element={<ProtectedRoute><HomeSearchUI /></ProtectedRoute>} />
            <Route path="/create-post-page" element={<ProtectedRoute><CreatePostPage /></ProtectedRoute>} />
        </Routes>
    )
}
