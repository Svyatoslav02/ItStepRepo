import React, { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import "../styles/ContentPreferences.css";

const ContentPreferences = () => {
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

    const contentSettings = [
        { title: "Private Account", desc: "Only people you approve can see your content.", enabled: true },
        { title: "Search Visibility", desc: "Allow your profile to appear in search engine results.", enabled: false },
    ];

    const interactions = [        
        { icon: "/assets/icons/view.png", title: "Content Visibility", desc: "Manage who can see your posts", enabled: true },
        { icon: "/assets/icons/user-block-01.png", title: "Blocked Users", desc: "12 accounts blocked", enabled: false },
		{ icon: "/assets/icons/download-01.png", title: "Download My Data", desc: "Get a copy of your infor", enabled: true },
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
                    <h2>Content Preferences</h2>
                    <p>Manage your privacy and data settings.</p>
                    <p>Your email notifications</p>
                    {contentSettings.map((item) => (
                        <div key={item.title} className="setting-row">
                            <div className="info">                                
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
                    <p>Interactions & Data</p>
                    {interactions.map((item) => (
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
                    <img src="/assets/icons/info.png" alt="info" className="info-icon" />
                        <p className="text-info">Privacy settings apply everywhere. Some changes may take up to 24 hours to update.</p>
					</div>
                </section>                
            </main>
        </div>
    );
};

export default ContentPreferences;