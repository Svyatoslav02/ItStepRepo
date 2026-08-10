import React from "react";
import "../styles/AiWelcome.css";

const AiWelcome = () => {
    const actions = [
        { icon: "/assets/icons/action1.png", title: "Generate images", desc: "Moodboards, art and explore ideas" },
        { icon: "/assets/icons/action2.png", title: "Find inspiration", desc: "Search moodboards, trends" },
        { icon: "/assets/icons/action3.png", title: "Explore palettes", desc: "Colors, gradients and trendy styles" },
        { icon: "/assets/icons/action4.png", title: "Design UI / UX", desc: "Interfaces, wireframes and layouts" },
    ];

    return (
        <div className="ai-welcome">
            {/* Sidebar */}
            <aside className="sidebar">
                <img src="/assets/icons/star.png" alt="Logo" className="icon" />
                <button className="icon"><img src="/assets/icons/home-03.png" alt="Home" /></button>
                <button className="icon"><img src="/assets/icons/search-01.png" alt="Search" /></button>
                <button className="icon active"><img src="/assets/icons/ai-beautify.png" alt="AI" /></button>
                <button className="icon"><img src="/assets/icons/user-03.png" alt="User" /></button>
                <button className="icon"><img src="/assets/icons/settings-01.png" alt="Settings" /></button>
                <button className="icon bottom"><img src="/assets/icons/logout-02.png" alt="Back" /></button>
            </aside>

            {/* Main content */}
            <main className="main">
                <header className="topbar">
                    <button className="icon"><img src="/assets/icons/arrow-left-01.png" alt="Back" /></button>
                    <h2>Ink</h2>
                    <button className="icon"><img src="/assets/icons/Container.png" alt="History" /></button>
                </header>

                <section className="center">
                    <div className="welcome-content">
                        <div className="avatar-glow">
                            <img src="/assets/icons/bluesky.png" alt="Butterfly" className="butterfly" />
                        </div>
                        <h1>Hi, I'm Ink</h1>
                        <p>I'm your AI assistant here to sketch ideas, organize notes, and capture thoughts.</p>
                    </div>
                    <div className="actions">
                        {actions.map((a, i) => (
                            <div key={i} className="action-card">
                                <img src={a.icon} alt={a.title} className="action-icon" />
                                <h3>{a.title}</h3>
                                <p>{a.desc}</p>
                            </div>
                        ))}
                    </div>

                    <div className="chat-input">
                        <input type="text" placeholder="Ask me to find anything..." />
                        <div className="chat-icons">
                            <img src="/assets/icons/image-01.png" alt="Search" />
                            <img src="/assets/icons/link-04.png" alt="Link" />
                            <img src="/assets/icons/mic-02.png" alt="Mic" className="mic" />
                        </div>
                    </div>
                </section>
            </main>
        </div>
    );
};

export default AiWelcome;

