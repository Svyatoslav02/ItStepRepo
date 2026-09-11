import React, { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import "../styles/ContentPreferences.css";
import SidebarComponent from "../components/SidebarComponent.jsx";
import AccountPrivacyComponent from "../components/AccountPrivacyComponent.jsx";
import SearchComponent from "../components/SearchComponent.jsx";

const ContentPreferences = () => {
    const [searchQuery, setSearchQuery] = useState("");
    const navigate = useNavigate();

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
            
            <SidebarComponent activeItem={"settings"} />
            
            <main className="content">
                <header className="header">
                    <div className="header-content">
                        <button onClick={() => navigate("/home")}>
                            <img src="/assets/icons/arrow-left-01.png" alt="Back" className="back-icon" />
                        </button>
                        <h3>Settings</h3>

                        <SearchComponent />

                        <div>
                            <button className="notification-btn">
                                <img src="/assets/icons/bell.png" alt="Favorite" />
                            </button>
                        </div>
                    </div>
                </header>

                <AccountPrivacyComponent activeItem={"privacy"}/>

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