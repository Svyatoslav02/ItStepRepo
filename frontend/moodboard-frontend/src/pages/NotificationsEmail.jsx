import React, { useState } from 'react';
import '../styles/NotificationsEmail.css';

const NotificationsEmail = () => {
    const [searchQuery, setSearchQuery] = useState("");
    const [active, setActive] = useState(false);
    const accountPrivacy = [
        { icon: "/assets/icons/security-lock.png", title: "Privacy" },
        { icon: "/assets/icons/bell.png", title: "Notifications Push" },
        { icon: "/assets/icons/mail-01.png", title: "Notifications Email", active: true },
        { icon: "/assets/icons/view.png", title: "Appearance" },
        { icon: "/assets/icons/languages.png", title: "Language" },
        { icon: "/assets/icons/user-block-01.png", title: "Blocked Users" },
        { icon: "/assets/icons/download-01.png", title: "Downloads" },
    ];

    const emailSettings = [
        { icon: "/assets/icons/heart.png", title: "Likes", desc: "Receive emails when someone likes my post.", enabled: true },
        { icon: "/assets/icons/message-01.png", title: "Comments", desc: "Receive emails about new comments.", enabled: false },
        { icon: "/assets/icons/tags.png", title: "Tags", desc: "Receive emails when someone tags me.", enabled: true },
        { icon: "/assets/icons/user-plus.png", title: "Friends requests", desc: "Receive emails for new friend requests.", enabled: false },
        { icon: "/assets/icons/telegram.png", title: "Updates", desc: "Receive emails about product updates and news.", enabled: true },
    ];

    return (
        <div className="settings-page">
            <aside className="sidebar">
                <img src="/assets/icons/bluesky.png" alt="Logo" className="icon" />
                <button className={`icon ${active === "home" ? "active" : ""}`} onClick={() => setActive("home")}>
                    <img src="/assets/icons/home-03.png" alt="Home" />
                </button>
                <button className={`icon ${active === "search" ? "active" : ""}`} onClick={() => setActive("search")}>
                    <img src="/assets/icons/search-01.png" alt="Search" />
                </button>
                <button className={`icon ${active === "ai" ? "active" : ""}`} onClick={() => setActive("ai")}>
                    <img src="/assets/icons/ai-beautify.png" alt="AI" />
                </button>
                <button className={`icon ${active === "user" ? "active" : ""}`} onClick={() => setActive("user")}>
                    <img src="/assets/icons/user-03.png" alt="User" />
                </button>
                <button className={`icon ${active === "settings" ? "active" : ""}`} onClick={() => setActive("settings")}>
                    <img src="/assets/icons/settings-01.png" alt="Settings" />
                </button>
                <button className="icon bottom"><img src="/assets/icons/logout-02.png" alt="Back" /></button>
            </aside>            

            <main className="content">                
                <header className="header">
                    <div className="header-content">
                        
                        <button><img src="/assets/icons/arrow-left-01.png" alt="Back" className="back-icon" /></button>
                            <h3>Settings</h3>
                        
                        
                        <div className="search-wrapper">
                            <img src="/assets/icons/search-01.png" alt="Search" className="search-icon" />
                            <input
                                type="text"
                                placeholder="Search for ideas"
                                value={searchQuery}
                                onChange={(e) => setSearchQuery(e.target.value)}
                            />

                        </div>
                        <div>
                            <button
                                className="notification-btn"
                            >
                                <img src="/assets/icons/bell.png" alt="Favorite" />
                            </button>
                        </div>
                        
                    </div>
                </header>

                {/* Account Privacy menu */}
                <section className="account-privacy">
                    <h3>Account Privacy</h3>
                    <ul>
                        {accountPrivacy.map((item) => (
                            <li key={item.title} className={item.active ? "active" : ""}>
                                <img src={item.icon} alt={item.title} className="icon" />
                                {item.title}
                            </li>
                        ))}
                    </ul>
                </section>

                <section className="email-settings">
                    <h2>Notifications</h2>
                    <p>Choose what email notifications you want to receive.</p>
                    {emailSettings.map((item) => (
                        <div key={item.title} className="setting-row">
                            <div className="info">
                                <img src={item.icon} alt={item.title} className="icon" />
                                <div className="text">
                                    <h4>{item.title}</h4>
                                    <p>{item.desc}</p>
                                </div>
                            </div>
                            <label className="switch">
                                <input type="checkbox" defaultChecked={item.enabled} />
                                <span className="slider"></span>
                            </label>
                        </div>
                    ))}
                </section>
            </main>            
        </div>
    );
};

export default NotificationsEmail;
