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
import DiscoverPage from '../pages/DiscoverPage.jsx' 
import PushNotifications from '../pages/PushNotifications'
import AiWelcome from '../pages/AiWelcome.jsx'
import WelcomePage from '../pages/WelcomePage.jsx'
import NotFound from '../pages/NotFound.jsx'

export default function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<LoadingScreen />} />
            <Route path="/inspiration" element={<InspirationLoading />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/signup" element={<SignUpPage />} />
            <Route path="/home" element={<HomePage />} />
            <Route path="/interests" element={<InterestsPage />} />
            <Route path="/discover" element={<DiscoverPage />} />
            <Route path="/welcome" element={<WelcomePage />} />            
            <Route path="/ai-welcome" element={<AiWelcome />} />            
            <Route path="/welcome" element={<WelcomePage />} />
            <Route path="/notifications-email" element={<NotificationsEmail />} />
            <Route path="/welcome" element={<Home />} />
            <Route path="*" element={<Home />} />
            <Route path="/welcome" element={<WelcomePage />} />
            <Route path="/welcome" element={<Home />} />
            <Route path="/content-preferences" element={<ContentPreferences />} />
            <Route path="*" element={<Home />} />
            <Route path="/welcome" element={<WelcomePage />} />
            <Route path="/welcome" element={<Home />} />
            <Route path="/notifications-push" element={<PushNotifications />} />
            <Route path="*" element={<Home />} />
            <Route path="/ai-welcome" element={<AiWelcome />} />
            <Route path="/welcome" element={<WelcomePage />} />
            <Route path="*" element={<NotFound />} />
        </Routes>
    )
}
