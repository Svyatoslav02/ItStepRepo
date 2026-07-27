import { Routes, Route } from 'react-router-dom'
import LoadingScreen from '../pages/LoadingScreen'
import InspirationLoading from '../pages/InspirationLoading'
import SignUpPage from '../pages/SignUpPage'
import LoginPage from '../pages/LoginPage.jsx'
import HomePage from '../pages/HomePage'
import Home from '../pages/Home'
import InterestsPage from '../pages/InterestsPage.jsx'
import DiscoverPage from '../pages/DiscoverPage.jsx'

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
            <Route path="/welcome" element={<Home />} />
            <Route path="*" element={<Home />} />
        </Routes>
    )
}
