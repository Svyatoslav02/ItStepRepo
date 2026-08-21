import React, { useState } from "react";
import "../styles/HomeSearchUI.css";

const recentSearches = [
    "Neon Tokyo street",
    "Minimalist interior",
    "Flash photography",
    "Drone beach shot",
];

const popularCategories = [
    { name: "Architecture", count: "12.4k", icon: "/src/assets/images/building-03.png" },
    { name: "Macro Shots", count: "6.3k", icon: "/src/assets/images/eco-power.png" },
    { name: "Studio Portrait", count: "15.8k", icon: "/src/assets/images/3-d-view.png" },
    { name: "Cinematic Landscape", count: "4.2k", icon: "/src/assets/images/clapperboard.png" },
    { name: "Street Style", count: "9.3k", icon: "/src/assets/images/footprints.png" },
];


const trendingItems = [
    { id: 1, title: "Fashion Editorial", searches: "15.7k", img: "/assets/images/image1.jpg" },
    { id: 2, title: "Seascapes", searches: "27.9k", img: "/assets/images/image2.jpg" },
    { id: 3, title: "Retro Film Aesthetic", searches: "1.8k", img: "/assets/images/image3.jpg" },
    { id: 4, title: "Light & Shadows", searches: "3.0k", img: "/assets/images/image4.jpg" },
    { id: 5, title: "Moody Wilderness", searches: "4.7k", img: "/assets/images/image5.jpg" },
    { id: 6, title: "Night Geometry", searches: "14.8k", img: "/assets/images/image6.jpg" },
    { id: 7, title: "Wild Nature", searches: "19.7k", img: "/assets/images/image7.jpg" },
    { id: 8, title: "Urban Exploration", searches: "4.7k", img: "/assets/images/image8.jpg" },
    { id: 9, title: "Futuristic Cities", searches: "9.3k", img: "/assets/images/image9.jpg" },
    { id: 10, title: "Neon Cyberpunk", searches: "12.4k", img: "/assets/images/image10.jpg" },
];

const exploreItems = [
    { id: 11, title: "Vibrant Gradients", searches: "14.0k", img: "/assets/images/image1.jpg" },
    { id: 12, title: "Minimalist Posters", searches: "10.6k", img: "/assets/images/image2.jpg" },
    { id: 13, title: "Brutalist Textures", searches: "11.3k", img: "/assets/images/image3.jpg" },
    { id: 14, title: "Cozy Workspace", searches: "12.9k", img: "/assets/images/image4.jpg" },
    { id: 15, title: "Summer Nostalgia", searches: "20.8k", img: "/assets/images/image5.jpg" },
    { id: 16, title: "Product Layouts", searches: "5.2k", img: "/assets/images/image6.jpg" },
];

const HomePage = () => {
    const [searchQuery, setSearchQuery] = useState("");
    const [favorites, setFavorites] = useState([]);
    const [recent, setRecent] = useState(recentSearches);

    const toggleFavorite = (id) => {
        setFavorites((prev) =>
            prev.includes(id) ? prev.filter((f) => f !== id) : [...prev, id]
        );
    };

    const clearRecent = () => {
        setRecent([]);
    };

    const removeRecent = (item) => {
        setRecent((prev) => prev.filter((r) => r !== item));
    };

    return (
        <div className="home-search-container">
            {/* ===== SIDEBAR ===== */}
            <aside className="sidebar">
                <div className="sidebar-icons">
                    <img src="/assets/icons/logo.png" alt="Logo" className="icon" />
                </div>
                <div className="sidebar-bottom">
                    <a href="/home" className="icon">
                        <img src="/assets/icons/home-03.png" alt="Go to Home" />
                    </a>
                    <a href="/homesearch" className="icon">
                        <img src="/assets/icons/search-01.png" alt="Search" />
                    </a>
                    <a href="/aiwelcome" className="icon">
                        <img src="/assets/icons/ai-beautify.png" alt="AI-Welcome" />
                    </a>
                    <a href="/user" className="icon">
                        <img src="/assets/icons/user-03.png" alt="User profile" />
                    </a>
                    <a href="/settings" className="icon">
                        <img src="/assets/icons/settings-01.png" alt="Settings" />
                    </a>
                </div>
                <div className="sidebar-bottom-ex">
                    <button className="icon"><img src="/assets/icons/logout-02.png" alt="Logout" /></button>
                </div>
            </aside>

            {/* ===== MAIN ===== */}
            <main className="main-content">
                {/* Шапка з пошуком */}
                <header className="header">
                    <div className="header-content">
                        <div className="search-wrapper">
                            <img
                                src="/assets/images/search-01.png"
                                alt="Search"
                                className="search-icon"
                            />
                            <input
                                type="text"
                                placeholder="Search for ideas"
                                value={searchQuery}
                                onChange={(e) => setSearchQuery(e.target.value)}
                            />
                        </div>
                        <button className="notification-btn">
                            <img src="/assets/icons/bell.png" alt="Notifications" />
                        </button>
                    </div>
                </header>

                {/* ===== КОНТЕНТ (дві колонки) ===== */}
                <div className="search-page-content">
                    {/* ----- ЛІВА КОЛОНКА ----- */}
                    <div className="left-column">
                        {/* Recent searches */}
                        <section className="section">
                            <div className="section-header">
                                <h2 className="section-title">Recent searches</h2>
                                {recent.length > 0 && (
                                    <button className="view-all-btn" onClick={clearRecent}>
                                        Clear all
                                    </button>
                                )}
                            </div>

                            <div className="recent-list">
                                {recent.map((item) => (
                                    <button key={item} className="recent-item">
                                        <img
                                            src="/assets/icons/search-01.png"
                                            alt=""
                                            className="recent-icon"
                                        />
                                        <span>{item}</span>
                                        <button
                                            className="remove-recent"
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                removeRecent(item);
                                            }}
                                        >
                                            <img src="/assets/images/link-square-02.png" alt="Remove" />
                                        </button>
                                    </button>
                                ))}
                            </div>
                        </section>

                        {/* Popular categories */}
                        <section className="section">
                            <div className="section-header">
                                <h2 className="section-title">Popular categories</h2>
                                <button className="view-all-btn">View all</button>
                            </div>

                            <div className="categories-list">
                                {popularCategories.map((cat) => (
                                    <button key={cat.name} className="category-item">
                                        <div className="category-icon">
                                            <img src={cat.icon} alt={cat.name} />
                                        </div>
                                        <span className="category-name">{cat.name}</span>
                                        <span className="category-count">{cat.count}</span>
                                        <img
                                            src="/assets/icons/chevron-right.png"
                                            alt=""
                                            className="chevron"
                                        />
                                    </button>
                                ))}
                            </div>
                        </section>
                    </div>

                    {/* ----- ПРАВА КОЛОНКА ----- */}
                    <div className="right-column">
                        {/* Trending */}
                        <section className="section">
                            <div className="section-header">
                                <h2 className="section-title">Trending searches on Ink</h2>
                                <button className="view-all-btn">View all</button>
                            </div>

                            <div className="masonry-grid">
                                {trendingItems.map((item) => (
                                    <div key={item.id} className="gallery-card search-card">
                                        <div className="image-wrapper">
                                            <img src={item.img} alt={item.title} />
                                            <button
                                                className={`fav-btn ${
                                                    favorites.includes(item.id) ? "active" : ""
                                                }`}
                                                onClick={() => toggleFavorite(item.id)}
                                            >
                                                <img
                                                    src={
                                                        favorites.includes(item.id)
                                                            ? "/assets/icons/heart-1.png"
                                                            : "/assets/icons/heart.png"
                                                    }
                                                    alt="Favorite"
                                                />
                                            </button>
                                        </div>
                                        <div className="card-info">
                                            <h4>{item.title}</h4>
                                            <span className="searches-count">
                      {item.searches} searches
                    </span>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </section>

                        {/* Explore */}
                        <section className="section">
                            <div className="section-header">
                                <h2 className="section-title">Explore collections</h2>
                                <button className="view-all-btn">View all</button>
                            </div>

                            <div className="masonry-grid">
                                {exploreItems.map((item) => (
                                    <div key={item.id} className="gallery-card search-card">
                                        <div className="image-wrapper">
                                            <img src={item.img} alt={item.title} />
                                            <button
                                                className={`fav-btn ${
                                                    favorites.includes(item.id) ? "active" : ""
                                                }`}
                                                onClick={() => toggleFavorite(item.id)}
                                            >
                                                <img
                                                    src={
                                                        favorites.includes(item.id)
                                                            ? "/assets/icons/heart-1.png"
                                                            : "/assets/icons/heart.png"
                                                    }
                                                    alt="Favorite"
                                                />
                                            </button>
                                        </div>
                                        <div className="card-info">
                                            <h4>{item.title}</h4>
                                            <span className="searches-count">
                      {item.searches} searches
                    </span>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </section>
                    </div>
                </div>
            </main>
        </div>
    );
};

export default HomePage;