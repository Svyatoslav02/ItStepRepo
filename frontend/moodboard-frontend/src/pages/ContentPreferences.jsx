import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/ContentPreferences.css";
import SidebarComponent from "../components/SidebarComponent.jsx";
import AccountPrivacyComponent from "../components/AccountPrivacyComponent.jsx";
import SearchComponent from "../components/SearchComponent.jsx";
import { privacyService } from "../services/privacyService";

const ContentPreferences = () => {
    const [searchQuery, setSearchQuery] = useState("");
    const navigate = useNavigate();

    // Backend-backed privacy toggles (falls back to the original static
    // defaults if the request fails).
    const [privateAccount, setPrivateAccount] = useState(true);
    const [searchVisibility, setSearchVisibility] = useState(false);
    const [contentVisibility, setContentVisibility] = useState(true);
    const [blockedCount, setBlockedCount] = useState(null);
    const [exportStatus, setExportStatus] = useState("");

    useEffect(() => {
        let cancelled = false;
        privacyService
            .getSettings()
            .then((settings) => {
                if (cancelled || !settings) return;
                setPrivateAccount(Boolean(settings.privateAccount));
                setSearchVisibility(Boolean(settings.searchVisibility));
                setContentVisibility(Boolean(settings.contentVisibility));
            })
            .catch(() => {
                // keep static defaults
            });

        privacyService
            .getBlockedUsers()
            .then((list) => {
                if (!cancelled && Array.isArray(list)) setBlockedCount(list.length);
            })
            .catch(() => {
                // keep the static "12 accounts blocked" placeholder
            });

        return () => { cancelled = true; };
    }, []);

    const persist = (next) => {
        privacyService
            .updateSettings({
                privateAccount: next.privateAccount ?? privateAccount,
                searchVisibility: next.searchVisibility ?? searchVisibility,
                contentVisibility: next.contentVisibility ?? contentVisibility,
            })
            .catch(() => {
                // best-effort; UI already reflects the optimistic change
            });
    };

    const handleTogglePrivateAccount = () => {
        setPrivateAccount((prev) => {
            const next = !prev;
            persist({ privateAccount: next });
            return next;
        });
    };

    const handleToggleSearchVisibility = () => {
        setSearchVisibility((prev) => {
            const next = !prev;
            persist({ searchVisibility: next });
            return next;
        });
    };

    const handleToggleContentVisibility = () => {
        setContentVisibility((prev) => {
            const next = !prev;
            persist({ contentVisibility: next });
            return next;
        });
    };

    const handleDataExport = async () => {
        setExportStatus("Preparing export...");
        try {
            const data = await privacyService.requestDataExport();
            const blob = new Blob([JSON.stringify(data, null, 2)], { type: "application/json" });
            const url = URL.createObjectURL(blob);
            const link = document.createElement("a");
            link.href = url;
            link.download = "moodboardai-data-export.json";
            link.click();
            URL.revokeObjectURL(url);
            setExportStatus("Export downloaded.");
        } catch (err) {
            setExportStatus(err.message || "Unable to export data.");
        }
    };

    const contentSettings = [
        {
            title: "Private Account",
            desc: "Only people you approve can see your content.",
            enabled: privateAccount,
            onToggle: handleTogglePrivateAccount,
        },
        {
            title: "Search Visibility",
            desc: "Allow your profile to appear in search engine results.",
            enabled: searchVisibility,
            onToggle: handleToggleSearchVisibility,
        },
    ];

    const interactions = [
        {
            icon: "/assets/icons/view.png",
            title: "Content Visibility",
            desc: "Manage who can see your posts",
            enabled: contentVisibility,
            onClick: handleToggleContentVisibility,
        },
        {
            icon: "/assets/icons/user-block-01.png",
            title: "Blocked Users",
            desc: blockedCount !== null ? `${blockedCount} accounts blocked` : "12 accounts blocked",
            enabled: false,
            onClick: () => navigate("/home"),
        },
        {
            icon: "/assets/icons/download-01.png",
            title: "Download My Data",
            desc: "Get a copy of your info",
            enabled: true,
            onClick: handleDataExport,
        },
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

                        <SearchComponent onQueryChange={setSearchQuery} />

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
                                <input type="checkbox" checked={item.enabled} onChange={item.onToggle} />
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
                                <button onClick={item.onClick}>
                                    <img src="/assets/icons/Vector-right.png" alt="Back" className="back-icon" />
                                </button>
                            </div>
                        </div>
                    ))}
                    {exportStatus && <p className="text-info">{exportStatus}</p>}
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
