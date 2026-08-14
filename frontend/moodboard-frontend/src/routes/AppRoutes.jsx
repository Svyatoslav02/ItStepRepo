import { Routes, Route } from 'react-router-dom'
import Home from '../pages/Home'
import SignUpPage from '../pages/SignUpPage'
import LoginPage from '../pages/LoginPage'
import LoadingScreen from '../pages/LoadingScreen'
import InspirationLoading from '../pages/InspirationLoading'
import HomePage from '../pages/HomePage'
import InterestsPage from '../pages/InterestsPage.jsx'
import DiscoverPage from '../pages/DiscoverPage.jsx'
import ContentPreferences from '../pages/ContentPreferences.jsx'

export default function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<LoadingScreen />} />
            <Route path="/inspiration" element={<InspirationLoading />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/signup" element={<SignUpPage />} />
            <Route path="/loading" element={<LoadingScreen />} />
            <Route path="/home" element={<HomePage />} />
            <Route path="/interests" element={<InterestsPage />} />
            <Route path="/discover" element={<DiscoverPage />} />
            <Route path="/welcome" element={<Home />} />
            <Route path="/content-preferences" element={<ContentPreferences />} />
            <Route path="*" element={<Home />} />
        </Routes>
    )
}