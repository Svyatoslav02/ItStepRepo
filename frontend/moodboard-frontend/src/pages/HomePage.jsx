import React, { useState } from "react";
import "../styles/HomePage.css";

const categories = ["All", "Nature", "Travel", "Wallpaper", "Art", "Design"];

// Array of image paths
const images = Array.from({ length: 10 }, (_, i) => `/assets/images/image${i + 1}.jpg`);

// Array of metadata
const galleryItems = [
    { id: 1, title: "Summer Coast", category: "Nature" },
    { id: 2, title: "Palm Shadows", category: "Wallpaper" },
    { id: 3, title: "Deep Jungle", category: "Nature" },
    { id: 4, title: "City Lights", category: "Travel" },
    { id: 5, title: "Neon Waves", category: "Art" },
    { id: 6, title: "Acrylic Fluid", category: "Design" },
    { id: 7, title: "Autumn Lake", category: "Nature" },
    { id: 8, title: "Night Campfire", category: "Travel" },
    { id: 9, title: "Forest Path", category: "Nature" },
    { id: 10, title: "Golden Hour", category: "Wallpaper" },
];

const HomePage = () => {
    const [activeCategory, setActiveCategory] = useState("All");
    const [searchQuery, setSearchQuery] = useState("");
    const [favorites, setFavorites] = useState([]);

    const toggleFavorite = (id) => {
        setFavorites((prev) =>
            prev.includes(id) ? prev.filter((f) => f !== id) : [...prev, id]
        );
    };

    // Filtering by category and search
    const filteredItems = galleryItems.filter(
        (item) =>
            (activeCategory === "All" || item.category === activeCategory) &&
            item.title.toLowerCase().includes(searchQuery.toLowerCase())
    );

    return (
        <div className="gallery-layout">
            {/* Sidebar */}
            <aside className="sidebar">
                <div className="sidebar-icons">
                    <img src="/assets/icons/logo.png" alt="Logo" className="icon" />
                    <button className="icon"><img src="/assets/icons/home-03.png" alt="Home" /></button>
                    <button className="icon"><img src="/assets/icons/search-01.png" alt="Search" /></button>
                    <button className="icon"><img src="/assets/icons/user-03.png" alt="User" /></button>
                    <button className="icon"><img src="/assets/icons/settings-01.png" alt="Settings" /></button>
                </div>
                <div className="sidebar-bottom">
                    <button className="icon"><img src="/assets/icons/logout-02.png" alt="Logout" /></button>
                </div>
            </aside>

            {/* Main Content */}
            <main className="main-content">
                <header className="header">
                    <div className="header-content">
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


                {/* Category Tabs */}
                <div className="categories">
                    {categories.map((cat) => (
                        <button
                            key={cat}
                            className={`category-tab ${activeCategory === cat ? "active" : ""}`}
                            onClick={() => setActiveCategory(cat)}
                        >
                            {cat}
                        </button>
                    ))}
                </div>

                {/* Gallery */}
                <div className="gallery-grid">
                    {filteredItems.map((item) => (
                        <div key={item.id} className="gallery-card">
                            {/* Using images[item.id - 1] */}
                            <div className="image-wrapper">
                                <img src={images[item.id - 1]} alt={item.title} />
                                <button
                                    className={`fav-btn ${favorites.includes(item.id) ? "active" : ""}`}
                                    onClick={() => toggleFavorite(item.id)}
                                >
                                    <img src={favorites.includes(item.id) ? "/assets/icons/heart-1.png" : "/assets/icons/heart.png"} alt="Favorite" />
                                </button>
                            </div>
                            <div className="card-info">
                                <h4>{item.title}</h4>
                                <div className="card-actions">
                                    <button className="menu-btn">⋯</button>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </main>
        </div>
    );
};

export default HomePage;


