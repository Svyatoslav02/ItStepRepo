import React, { useEffect, useState } from "react";
import "../styles/HomeSearchUI.css";
import SidebarComponent from "../components/SidebarComponent";
import SearchComponent from "../components/SearchComponent.jsx";
import { recentSearchesService } from "../services/recentSearchesService";
import { searchService } from "../services/searchService";
import { isAuthenticated } from "../utils/auth";

const recentSearches1 = [
    "Neon Tokyo street",
    "Minimalist interior",
    "Flash photography",
    "Drone beach shot",
];

const popularCategories1 = [
    { name: "Architecture", count: "12.4k", icon: "/src/assets/images/building-03.png" },
    { name: "Macro Shots", count: "6.3k", icon: "/src/assets/images/eco-power.png" },
    { name: "Studio Portrait", count: "15.8k", icon: "/src/assets/images/3-d-view.png" },
    { name: "Cinematic Landscape", count: "4.2k", icon: "/src/assets/images/clapperboard.png" },
    { name: "Street Style", count: "9.3k", icon: "/src/assets/images/footprints.png" },
];

const trendingItems1 = [
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

const exploreItems1 = [
    { id: 11, title: "Vibrant Gradients", searches: "14.0k", img: "/assets/images/image1.jpg" },
    { id: 12, title: "Minimalist Posters", searches: "10.6k", img: "/assets/images/image2.jpg" },
    { id: 13, title: "Brutalist Textures", searches: "11.3k", img: "/assets/images/image3.jpg" },
    { id: 14, title: "Cozy Workspace", searches: "12.9k", img: "/assets/images/image4.jpg" },
    { id: 15, title: "Summer Nostalgia", searches: "20.8k", img: "/assets/images/image5.jpg" },
    { id: 16, title: "Product Layouts", searches: "5.2k", img: "/assets/images/image6.jpg" },
];

// Formats a raw like-count number the way the original mock data did (e.g. "15.7k").
function formatCount(n) {
    if (typeof n !== "number") return n;
    if (n >= 1000) return `${(n / 1000).toFixed(1)}k`;
    return String(n);
}

const HomePage1 = () => {
    const [searchQuery1, setSearchQuery1] = useState("");
    const [favorites1, setFavorites1] = useState([]);
    const [recent1, setRecent1] = useState(recentSearches1);
    const [popularCategories, setPopularCategories] = useState(popularCategories1);
    const [trendingItems, setTrendingItems] = useState(trendingItems1);
    const [exploreItems] = useState(exploreItems1);

    // Recent searches — only available for authenticated users.
    useEffect(() => {
        if (!isAuthenticated()) return;
        let cancelled = false;
        recentSearchesService
            .getAll()
            .then((apiRecent) => {
                if (cancelled || !Array.isArray(apiRecent)) return;
                if (apiRecent.length > 0) {
                    setRecent1(apiRecent.map((r) => r.query));
                }
            })
            .catch(() => {
                // keep static fallback
            });
        return () => { cancelled = true; };
    }, []);

    // Categories.
    useEffect(() => {
        let cancelled = false;
        searchService
            .getCategories()
            .then((apiCategories) => {
                if (cancelled || !Array.isArray(apiCategories) || apiCategories.length === 0) return;
                setPopularCategories(
                    apiCategories.map((c) => ({
                        name: c.name,
                        count: "",
                        icon: `/assets/icons/${c.icon}.png`,
                    }))
                );
            })
            .catch(() => {
                // keep static fallback
            });
        return () => { cancelled = true; };
    }, []);

    // Trending pins.
    useEffect(() => {
        let cancelled = false;
        searchService
            .getTrending(10)
            .then((apiTrending) => {
                if (cancelled || !Array.isArray(apiTrending) || apiTrending.length === 0) return;
                setTrendingItems(
                    apiTrending.map((p) => ({
                        id: p.id,
                        title: p.title,
                        searches: formatCount(p.likeCount),
                        img: p.imageUrl,
                    }))
                );
            })
            .catch(() => {
                // keep static fallback
            });
        return () => { cancelled = true; };
    }, []);

    const toggleFavorite1 = (id) => {
        setFavorites1((prev) =>
            prev.includes(id) ? prev.filter((f) => f !== id) : [...prev, id]
        );
    };

    const clearRecent1 = () => {
        setRecent1([]);
        if (isAuthenticated()) {
            recentSearchesService.clearAll().catch(() => {});
        }
    };

    const removeRecent1 = (item) => {
        setRecent1((prev) => prev.filter((r) => r !== item));
    };

    const handleSearchSubmit = (query) => {
        setSearchQuery1(query);
        if (!query) return;
        setRecent1((prev) => [query, ...prev.filter((r) => r !== query)]);
        if (isAuthenticated()) {
            recentSearchesService.add(query).catch(() => {});
        }
        searchService.search({ q: query }).catch(() => {});
    };

    return (
        <div className="home-search-container1">
            {/* ===== SIDEBAR ===== */}
            <SidebarComponent activeItem="search" />

            {/* ===== MAIN ===== */}
            <main className="main-content1">
                {/* Шапка з пошуком */}
                <header className="header1">
                    <div className="header-content-1">
                        <SearchComponent onSearchSubmit={handleSearchSubmit} onQueryChange={setSearchQuery1} />
                        <div className="notification-btn">
                            <img src="/assets/icons/bell.png" alt="Favorite" />
                        </div>
                    </div>
                </header>

                {/* ===== КОНТЕНТ (дві колонки) ===== */}
                <div className="search-page-content1">
                    {/* ----- ЛІВА КОЛОНКА ----- */}
                    <div className="left-column1">
                        {/* Recent searches */}
                        <section className="section1">
                            <div className="section-header1">
                                <h2 className="section-title1">Recent searches</h2>
                                {recent1.length > 0 && (
                                    <button className="view-all-btn1" onClick={clearRecent1}>
                                        Clear all
                                    </button>
                                )}
                            </div>

                            <div className="recent-list1">
                                {recent1.map((item) => (
                                    <button key={item} className="recent-item1">
                                        <img
                                            src="/assets/icons/search-01.png"
                                            alt=""
                                            className="recent-icon1"
                                        />
                                        <span>{item}</span>
                                        <button
                                            className="remove-recent1"
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                removeRecent1(item);
                                            }}
                                        >
                                            <img src="/src/assets/images/Vector.png" alt="Remove" />
                                        </button>
                                    </button>
                                ))}
                            </div>
                        </section>

                        {/* Popular categories */}
                        <section className="section1">
                            <div className="section-header1">
                                <h2 className="section-title1">Popular categories</h2>
                                <button className="view-all-btn1">View all</button>
                            </div>

                            <div className="categories-list1">
                                {popularCategories.map((cat) => (
                                    <button key={cat.name} className="category-item1">
                                        <div className="category-icon1">
                                            <img src={cat.icon} alt={cat.name} />
                                        </div>
                                        <span className="category-name1">{cat.name}</span>
                                        <span className="category-count1">{cat.count}</span>
                                        <img
                                            src="/assets/icons/chevron-right.png"
                                            alt=""
                                            className="chevron1"
                                        />
                                    </button>
                                ))}
                            </div>
                        </section>
                    </div>

                    {/* ----- ПРАВА КОЛОНКА ----- */}
                    <div className="right-column1">
                        {/* Trending */}
                        <section className="section1">
                            <div className="section-header1">
                                <h2 className="section-title1">Trending searches on Ink</h2>
                                <button className="view-all-btn1">View all</button>
                            </div>

                            <div className="masonry-grid1">
                                {trendingItems.map((item) => (
                                    <div key={item.id} className="gallery-card1 search-card1">
                                        <div className="image-wrapper1">
                                            <img src={item.img} alt={item.title} />
                                            <button
                                                className={`fav-btn1 ${
                                                    favorites1.includes(item.id) ? "active" : ""
                                                }`}
                                                onClick={() => toggleFavorite1(item.id)}
                                            >
                                                <img
                                                    src={
                                                        favorites1.includes(item.id)
                                                            ? "/assets/icons/heart-1.png"
                                                            : "/assets/icons/heart.png"
                                                    }
                                                    alt="Favorite"
                                                />
                                            </button>
                                        </div>
                                        <div className="card-info1">
                                            <h4>{item.title}</h4>
                                            <span className="searches-count1">
                                              {item.searches} searches
                                            </span>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </section>

                        {/* Explore */}
                        <section className="section1">
                            <div className="section-header1">
                                <h2 className="section-title1">Explore collections</h2>
                                <button className="view-all-btn1">View all</button>
                            </div>

                            <div className="masonry-grid1">
                                {exploreItems.map((item) => (
                                    <div key={item.id} className="gallery-card1 search-card1">
                                        <div className="image-wrapper1">
                                            <img src={item.img} alt={item.title} />
                                            <button
                                                className={`fav-btn1 ${
                                                    favorites1.includes(item.id) ? "active" : ""
                                                }`}
                                                onClick={() => toggleFavorite1(item.id)}
                                            >
                                                <img
                                                    src={
                                                        favorites1.includes(item.id)
                                                            ? "/assets/icons/heart-1.png"
                                                            : "/assets/icons/heart.png"
                                                    }
                                                    alt="Favorite"
                                                />
                                            </button>
                                        </div>
                                        <div className="card-info1">
                                            <h4>{item.title}</h4>
                                            <span className="searches-count1">
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

export default HomePage1;
