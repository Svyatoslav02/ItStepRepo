import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/Onboarding.css";

const features = [
    {
        icon: (
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.7">
                <path d="M12 2L2 7l10 5 10-5-10-5z" />
                <path d="M2 17l10 5 10-5" />
                <path d="M2 12l10 5 10-5" />
            </svg>
        ),
        title: "Generate images",
        description: "Moodboards, art and explore ideas",
    },
    {
        icon: (
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.7">
                <circle cx="11" cy="11" r="8" />
                <path d="M21 21l-4.35-4.35" />
            </svg>
        ),
        title: "Find inspiration",
        description: "Search moodboards, trends",
    },
    {
        icon: (
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.7">
                <circle cx="12" cy="12" r="3" />
                <path d="M12 2v2M12 20v2M4.93 4.93l1.41 1.41M17.66 17.66l1.41 1.41M2 12h2M20 12h2M4.93 19.07l1.41-1.41M17.66 6.34l1.41-1.41" />
            </svg>
        ),
        title: "Explore palettes",
        description: "Colors, gradients and trendy styles",
    },
    {
        icon: (
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.7">
                <rect x="3" y="3" width="18" height="18" rx="2" />
                <path d="M3 9h18M9 21V9" />
            </svg>
        ),
        title: "Design UI / UX",
        description: "Interfaces, wireframes and layouts",
    },
];

const Onboarding = () => {
    const navigate = useNavigate();
    const [active, setActive] = useState(false);

    const handleNext = () => {
        setActive(true);
        setTimeout(() => navigate("/interests"), 280);
    };

    const handleSkip = () => {
        navigate("/interests");
    };

    return (
        <div className="onboarding-screen">
            {/* ===== SIDEBAR ===== */}
            <aside className="sidebar">
                <div className="sidebar-icons">
                    <img src="/assets/icons/logo.png" alt="Logo" className="icon" />
                    <button className="icon"><img src="/assets/icons/home-03.png" alt="Home" /></button>
                    <button className="icon"><img src="/assets/icons/search-01.png" alt="Search" /></button>
                    <button className="icon"><img src="/assets/icons/user-03.png" alt="User" /></button>
                    <button className="icon"><img src="/assets/icons/settings-01.png" alt="Settings" /></button>
                </div>
                <div className="sidebar-bottom">
                    <button className="icon"><img src="/assets/icons/logout-02.png" alt="Logout" /></button>
                </div>
            </aside>

            {/* ===== MAIN ===== */}
            <main className="main">
                <div className="topbar">
                    <button className="back-btn" onClick={() => navigate(-1)}>
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                            <path d="M15 18l-6-6 6-6" />
                        </svg>
                    </button>
                    <span className="app-name">Ink</span>
                    <button className="history-btn" onClick={handleSkip} title="Skip">
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8">
                            <path d="M3 12a9 9 0 1 0 9-9 9.75 9.75 0 0 0-6.74 2.74L3 8" />
                            <path d="M3 3v5h5" />
                            <path d="M12 7v5l4 2" />
                        </svg>
                    </button>
                </div>
                

                <div className="hero">
                    <div className="logo-circle">
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5">
                            <path d="M12 2c-1.5 3-4 5-4 8a4 4 0 0 0 8 0c0-3-2.5-5-4-8z" />
                            <path d="M8 14c-1 2-1 4 0 6" />
                            <path d="M16 14c1 2 1 4 0 6" />
                            <path d="M9 18h6" />
                        </svg>
                    </div>

                    <h1 className="title">Hi, I'm Ink</h1>
                    <p className="subtitle">
                        I'm your AI assistant here to sketch ideas,<br />
                        organize notes, and capture thoughts.
                    </p>

                    <div className="divider"></div>

                    <div className="features-grid">
                        {features.map((feature, index) => (
                            <div key={index} className="feature-card">
                                <div className="feature-icon">{feature.icon}</div>
                                <div className="feature-text">
                                    <h3>{feature.title}</h3>
                                    <p>{feature.description}</p>
                                </div>
                            </div>
                        ))}
                    </div>

                    <div className="ask-bar">
                        <div className="ask-input">
                            <span className="placeholder">Ask me to find anything...</span>

                            <div className="ask-actions">
                                <button className="icon-btn" type="button" title="Chat">
                                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8">
                                        <path d="M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z" />
                                    </svg>
                                </button>

                                <button className="icon-btn" type="button" title="Attach">
                                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8">
                                        <path d="M21.44 11.05l-9.19 9.19a6 6 0 0 1-8.49-8.49l9.19-9.19a4 4 0 0 1 5.66 5.66l-9.2 9.19a2 2 0 0 1-2.83-2.83l8.49-8.48" />
                                    </svg>
                                </button>
                            </div>
                        </div>

                        <button
                            className={`next-btn ${active ? "active" : ""}`}
                            onClick={handleNext}
                        >
                            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                                <path d="M12 1a3 3 0 0 0-3 3v8a3 3 0 0 0 6 0V4a3 3 0 0 0-3-3z" />
                                <path d="M19 10v2a7 7 0 0 1-14 0v-2" />
                                <line x1="12" y1="19" x2="12" y2="23" />
                                <line x1="8" y1="23" x2="16" y2="23" />
                            </svg>
                        </button>
                    </div>
                </div>
            </main>
        </div>
    );
};

export default Onboarding;
