import React, { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import "../styles/PushNotifications.css";

const PushNotifications = () => {
    const [searchQuery, setSearchQuery] = useState("");
    const navigate = useNavigate();
    const location = useLocation();

    const accountPrivacy = [
        { icon: "/assets/icons/security-lock.png", path: "/content-preferences", title: "Privacy" },
        { icon: "/assets/icons/bell.png", path: "/notifications-push", title: "Notifications Push" },
        { icon: "/assets/icons/mail-01.png", path: "/notifications-email", title: "Notifications Email" },
        { icon: "/assets/icons/view.png", path: "/appearance", title: "Appearance" },
        { icon: "/assets/icons/languages.png", path: "/language", title: "Language" },
        { icon: "/assets/icons/user-block-01.png", path: "/blocked-users", title: "Blocked Users" },
        { icon: "/assets/icons/download-01.png", path: "/downloads", title: "Downloads" },
    ];

    const pushNotificationsSettings = [
        { icon: "/assets/icons/heart.png", title: "Likes", desc: "Notify me when someone likes my post.", enabled: true },
        { icon: "/assets/icons/message-01.png", title: "Comments", desc: "Notify me about new comments on my post.", enabled: false },
        { icon: "/assets/icons/user-plus.png", title: "New Followers", desc: "Alert me when i get a new follower.", enabled: true },
        { icon: "/assets/icons/at.png", title: "Mentions", desc: "Get notified when someone tags you.", enabled: false },
    ];

    const discovery = [
        { icon: "/assets/icons/star.png", title: "Recommendations", desc: "Suggestions based on your interests." },
        
    ];

    const advanced = [
        { icon: "/assets/icons/clock-01.png", title: "Quiet Mode", desc: "Pause notifications during set times" },
        { icon: "/assets/icons/bell.png", title: "Keep Mentions", desc: "Changes may take a moment to sync. Keep Mentions on for better communication." },       
    ];

    return (
        <div className="settings-page">
            <aside className="sidebar">
                <img src="/assets/icons/bluesky.png" alt="Logo" className="icon" />
                <button className={`icon ${location.pathname === "/home" ? "active" : ""}`} onClick={() => navigate("/home")}>
                    <img src="/assets/icons/home-03.png" alt="Home" />
                </button>
                <button className={`icon ${location.pathname === "/search" ? "active" : ""}`} onClick={() => navigate("/search")}>
                    <img src="/assets/icons/search-01.png" alt="Search" />
                </button>
                <button className={`icon ${location.pathname === "/ai" ? "active" : ""}`} onClick={() => navigate("/ai")}>
                    <img src="/assets/icons/ai-beautify.png" alt="AI" />
                </button>
                <button className={`icon ${location.pathname === "/user" ? "active" : ""}`} onClick={() => navigate("/user")}>
                    <img src="/assets/icons/user-03.png" alt="User" />
                </button>
                <button className={`icon ${location.pathname === "/settings" ? "active" : ""}`} onClick={() => navigate("/settings")}>
                    <img src="/assets/icons/settings-01.png" alt="Settings" />
                </button>
                <button className="icon bottom" onClick={() => navigate("/login")}>
                    <img src="/assets/icons/logout-02.png" alt="Back" />
                </button>
            </aside>

            <main className="content">
                <header className="header">
                    <div className="header-content">
                        <button onClick={() => navigate("/home")}>
                            <img src="/assets/icons/arrow-left-01.png" alt="Back" className="back-icon" />
                        </button>
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
                            <button className="notification-btn">
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
                            <li
                                key={item.title}
                                className={location.pathname === item.path ? "active" : ""}
                                onClick={() => navigate(item.path)}
                            >
                                <img src={item.icon} alt={item.title} className="icon" />
                                {item.title}
                            </li>
                        ))}
                    </ul>
                </section>

                <section className="email-settings">
                    <h2>Notifications</h2>
                    <p>Choose how you want to receive push notifications.</p>
                    <p>Activiti notifications</p>
                    {pushNotificationsSettings.map((item) => (
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
                    <p>Discovery</p>
                    {discovery.map((item) => (
                        <div key={item.title} className="setting-row">
                            <div className="info">
                                <img src={item.icon} alt={item.title} className="icon" />
                                <div className="text">
                                    <h4>{item.title}</h4>
                                    <p>{item.desc}</p>
                                </div>
                            </div>
                            <div>
                                <button onClick={() => navigate("/home")}>
                                    <img src="/assets/icons/Vector-right.png" alt="Back" className="back-icon" />
                                </button>
                            </div>
                        </div>
                    ))}
                    <p>Advanced</p>
                    {advanced.map((item) => (
                        <div key={item.title} className="setting-row">
                            <div className="info">
                                <img src={item.icon} alt={item.title} className="icon" />
                                <div className="text">
                                    <h4>{item.title}</h4>
                                    <p>{item.desc}</p>
                                </div>
                            </div>
                            <div>
                                <button onClick={() => navigate("/home")}>
                                    <img src="/assets/icons/Vector-right.png" alt="Back" className="back-icon" />
                                </button>
                            </div>
                        </div>
                    ))}
                    <div className="text-div-info">
                        <img src="/assets/icons/bell.png" alt="info" className="info-icon" />
                        <p className="text-info">Changes may take a moment to sync. Keep "Mentions" on for better communication.</p>
                    </div>
                    </section>                
            </main>
        </div>
    );
};

export default PushNotifications;