import { Outlet } from 'react-router-dom';

export default function Layout() {
    return (
        <div className="app">
            <header>Moodboard AI</header>
            <main>
                <Outlet />
            </main>
            <footer>© 2026</footer>
        </div>
    );
}