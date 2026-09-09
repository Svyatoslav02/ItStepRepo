import { Link } from 'react-router-dom'

export default function NotFound() {
    return (
        <div style={{ textAlign: 'center', padding: '4rem 1rem', color: '#fff' }}>
            <h1>404</h1>
            <p>Сторінку не знайдено.</p>
            <Link to="/home" style={{ color: '#6366f1' }}>
                Повернутись на головну
            </Link>
        </div>
    )
}
