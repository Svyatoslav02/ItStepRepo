import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/PushNotifications.css";
import SidebarComponent from "../components/SidebarComponent";
import AccountPrivacyComponent from "../components/AccountPrivacyComponent.jsx";
import SearchComponent from "../components/SearchComponent.jsx";
import { notificationsService } from "../services/notificationsService";

// Maps each row's title to the backend NotificationPreferenceDto field name.
const FIELD_BY_TITLE = {
    Likes: "pushLikes",
    Comments: "pushComments",
    "New Followers": "pushFriendRequests",
    Mentions: "pushMentions",
    Recommendations: "pushRecommendations",
};

const defaultPushSettings = [
    { icon: "/assets/icons/heart.png", title: "Likes", desc: "Notify me when someone likes my post.", enabled: true },
    { icon: "/assets/icons/message-01.png", title: "Comments", desc: "Notify me about new comments on my post.", enabled: false },
    { icon: "/assets/icons/user-plus.png", title: "New Followers", desc: "Alert me when i get a new follower.", enabled: true },
    { icon: "/assets/icons/at.png", title: "Mentions", desc: "Get notified when someone tags you.", enabled: false },
];

const defaultDiscovery = [
    { icon: "/assets/icons/star.png", title: "Recommendations", desc: "Suggestions based on your interests.", enabled: true },
];

const PushNotifications = () => {
    const [searchQuery, setSearchQuery] = useState("");
    const navigate = useNavigate();

    const [pushNotificationsSettings, setPushNotificationsSettings] = useState(defaultPushSettings);
    const [discovery, setDiscovery] = useState(defaultDiscovery);
    const [quietMode, setQuietMode] = useState(false);

    const advanced = [
        { icon: "/assets/icons/clock-01.png", title: "Quiet Mode", desc: "Pause notifications during set times", enabled: quietMode },
        { icon: "/assets/icons/bell.png", title: "Keep Mentions", desc: "Changes may take a moment to sync. Keep Mentions on for better communication." },
    ];

    useEffect(() => {
        let cancelled = false;
        notificationsService
            .getPreferences()
            .then((prefs) => {
                if (cancelled || !prefs) return;

                setPushNotificationsSettings((prev) =>
                    prev.map((item) => {
                        const field = FIELD_BY_TITLE[item.title];
                        return field && prefs[field] !== undefined && prefs[field] !== null
                            ? { ...item, enabled: prefs[field] }
                            : item;
                    })
                );

                setDiscovery((prev) =>
                    prev.map((item) => {
                        const field = FIELD_BY_TITLE[item.title];
                        return field && prefs[field] !== undefined && prefs[field] !== null
                            ? { ...item, enabled: prefs[field] }
                            : item;
                    })
                );

                if (prefs.quietMode !== undefined && prefs.quietMode !== null) {
                    setQuietMode(prefs.quietMode);
                }
            })
            .catch(() => {
                // keep static defaults
            });
        return () => { cancelled = true; };
    }, []);

    const handleTogglePush = (title) => {
        setPushNotificationsSettings((prev) =>
            prev.map((item) => {
                if (item.title !== title) return item;
                const nextEnabled = !item.enabled;
                const field = FIELD_BY_TITLE[title];
                if (field) {
                    notificationsService.updatePreferences({ [field]: nextEnabled }).catch(() => {});
                }
                return { ...item, enabled: nextEnabled };
            })
        );
    };

    const handleToggleDiscovery = (title) => {
        setDiscovery((prev) =>
            prev.map((item) => {
                if (item.title !== title) return item;
                const nextEnabled = !item.enabled;
                const field = FIELD_BY_TITLE[title];
                if (field) {
                    notificationsService.updatePreferences({ [field]: nextEnabled }).catch(() => {});
                }
                return { ...item, enabled: nextEnabled };
            })
        );
    };

    const handleToggleQuietMode = () => {
        setQuietMode((prev) => {
            const next = !prev;
            notificationsService.updatePreferences({ quietMode: next }).catch(() => {});
            return next;
        });
    };

    return (
        <div className="settings-page-1">

            <SidebarComponent activeItem={"settings"} />

            <main className="content">
                <header className="header">
                    <div className="header-content">
                        <button onClick={() => navigate("/home")}>
                            <img src="/assets/icons/arrow-left-01.png" alt="Back" className="back-icon" />
                        </button>
                        <h3>Settings</h3>

                        <SearchComponent onQueryChange={setSearchQuery}/>

                        <div>
                            <button className="notification-btn">
                                <img src="/assets/icons/bell.png" alt="Favorite" />
                            </button>
                        </div>
                    </div>
                </header>

                <AccountPrivacyComponent activeItem={"notifications-push"}/>

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
                                <input type="checkbox" checked={item.enabled} onChange={() => handleTogglePush(item.title)} />
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
                            <label className="switch">
                                <input type="checkbox" checked={item.enabled} onChange={() => handleToggleDiscovery(item.title)} />
                                <span className="slider"></span>
                            </label>
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
                                {item.title === "Quiet Mode" ? (
                                    <label className="switch">
                                        <input type="checkbox" checked={item.enabled} onChange={handleToggleQuietMode} />
                                        <span className="slider"></span>
                                    </label>
                                ) : (
                                    <button onClick={() => navigate("/home")}>
                                        <img src="/assets/icons/Vector-right.png" alt="Back" className="back-icon" />
                                    </button>
                                )}
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
