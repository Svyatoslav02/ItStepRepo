import { Routes, Route } from 'react-router-dom'
import Home from '../pages/Home'
import SignUpPage from '../pages/SignUpPage'
import LoginPage from '../pages/LoginPage'
import LoadingScreen from '../pages/LoadingScreen'

export default function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/signup" element={<SignUpPage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/loading" element={<LoadingScreen />} />
        </Routes>
    )
}