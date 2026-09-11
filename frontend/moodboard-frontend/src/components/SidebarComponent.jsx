import React from "react";
import "./SidebarComponent.css";

const SidebarComponent = ({ activeItem }) => {
    const sidebarNavItems = [
        { key: "home", icon: "/assets/icons/home-03.png", label: "Home", path: "/home" },
        { key: "search", icon: "/assets/icons/search-01.png", label: "Search", path: "/home-search" },
        { key: "wand", icon: "/assets/icons/ai-beautify.png", label: "Ink", path: "/ai-welcome" },
        { key: "user", icon: "/assets/icons/user-03.png", label: "Profile", path: "/user" },
        { key: "settings", icon: "/assets/icons/settings-01.png", label: "Settings", path: "/notifications-email" },
    ];

    return (
        <aside className="sidebar">
            <div className="sidebar_logo">
                <img src="/src/assets/images/ink.png" alt="Logo"/>
            </div>

            <div className="sidebar_top">
                {sidebarNavItems.map((item) => (
                    <a
                    key={item.key}
                    className={`sidebar_item${item.key === activeItem ? " active" : ""}`}
                    aria-label={item.label}
                    href={item.path}
                    >
                    <img src={item.icon} alt="" />
                    </a>
                    ))}
            </div>

            <a
                className="sidebar_logout"
                aria-label="Logout"
                href="/login"
            >
                <img src="/assets/images/logout-02.png" alt=""/>
            </a>
        </aside>
    );
};

export default SidebarComponent;