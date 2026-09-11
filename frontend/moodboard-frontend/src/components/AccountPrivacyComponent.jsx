import React from "react";
import "./AccountPrivacyComponent.css";
import {useNavigate} from "react-router-dom";

const AccountPrivacyComponent = ({ activeItem }) => {
    const navigate = useNavigate();
    const accountPrivacy = [
        {
            key: "privacy",
            icon: "/assets/icons/security-lock.png",
            path: "/content-preferences",
            title: "Privacy"
        },
        {
            key: "notifications-push",
            icon: "/assets/icons/bell.png",
            path: "/notifications-push",
            title: "Notifications Push"
        },
        {
            key: "notifications-email",
            icon: "/assets/icons/mail-01.png",
            path: "/notifications-email",
            title: "Notifications Email"
        },
        {
            key: "appearance",
            icon: "/assets/icons/view.png",
            path: "/appearance",
            title: "Appearance"
        },
        {
            key: "language",
            icon: "/assets/icons/languages.png",
            path: "/language",
            title: "Language"
        },
        {
            key: "blocked-users",
            icon: "/assets/icons/user-block-01.png",
            path: "/blocked-users",
            title: "Blocked Users"
        },
        {
            key: "downloads",
            icon: "/assets/icons/download-01.png",
            path: "/downloads",
            title: "Downloads"
        },
    ];

    return (
        <section className="account-privacy">
            <h3>Account Privacy</h3>

            <ul>
                {accountPrivacy.map((item) => (
                    <li
                        key={item.key}
                        className={item.key === activeItem ? "active" : ""}
                        onClick={() => navigate(item.path)}
                    >
                        <img
                            src={item.icon}
                            alt={item.title}
                            className="icon"
                        />
                        {item.title}
                    </li>
                ))}
            </ul>
        </section>
    );
};

export default AccountPrivacyComponent;