import React, { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import "../styles/NotificationsEmail.css";
import SidebarComponent from "../components/SidebarComponent";
import AccountPrivacyComponent from "../components/AccountPrivacyComponent.jsx";
import SearchComponent from "../components/SearchComponent.jsx";

const NotificationsEmail = () => {
    const navigate = useNavigate();

    const emailSettings = [
        { icon: "/assets/icons/heart.png", title: "Likes", desc: "Receive emails when someone likes my post.", enabled: true },
        { icon: "/assets/icons/message-01.png", title: "Comments", desc: "Receive emails about new comments.", enabled: false },
        { icon: "/assets/icons/tags.png", title: "Tags", desc: "Receive emails when someone tags me.", enabled: true },
        { icon: "/assets/icons/user-plus.png", title: "Friends requests", desc: "Receive emails for new friend requests.", enabled: false },
        { icon: "/assets/icons/telegram.png", title: "Updates", desc: "Receive emails about product updates and news.", enabled: true },
    ];

    return (
        <div className="settings-page-2">
            
            <SidebarComponent activeItem={"settings"} />
            
            <main className="content">
                <header className="header-2">
                    <div className="header-content">
                        <button onClick={() => navigate("/home")}>
                            <img src="/assets/icons/arrow-left-01.png" alt="Back" className="back-icon" />
                        </button>
                        <h3>Settings</h3>

                        <SearchComponent/>

                        <div>
                            <button className="notification-btn">
                                <img src="/assets/icons/bell.png" alt="Favorite" />
                            </button>
                        </div>
                    </div>
                </header>

                <AccountPrivacyComponent activeItem={"notifications-email"}/>

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

