import { Routes, Route } from 'react-router-dom'
import Home from '../pages/Home'
import SignUpPage from '../pages/SignUpPage'
import LoadingScreen from '../pages/LoadingScreen'

export default function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/signup" element={<SignUpPage />} />
            <Route path="/loading" element={<LoadingScreen />} />
        </Routes>
    )
}