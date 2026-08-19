import './styles/App.css'
import { Routes, Route } from "react-router-dom"

import SignUpPage from './pages/SignUpPage.jsx'
import Onboarding from "./pages/Onboarding.jsx"

function App() {
  return (
      <Routes>
        <Route path="/onboarding" element={<Onboarding />} />
        <Route path="/signup" element={<SignUpPage />} />
      </Routes>
  )
}

export default App
