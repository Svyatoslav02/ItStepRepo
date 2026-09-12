import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/HomePage.css";
import SidebarComponent from "../components/SidebarComponent";
import SearchComponent from "../components/SearchComponent.jsx";
import { feedService } from "../services/feedService";
import { searchService } from "../services/searchService";
import { pinsService } from "../services/pinsService";
import { clearSession } from "../utils/auth";

const fallbackCategories = ["All", "Nature", "Travel", "Wallpaper", "Art", "Design"];

// Array of image paths
const images = Array.from({ length: 10 }, (_, i) => `/assets/images/image${i + 1}.jpg`);

// Array of metadata
const fallbackGalleryItems = [
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
    const [categories, setCategories] = useState(fallbackCategories);
    const [categoryMap, setCategoryMap] = useState({}); // name -> id, for feed filtering
    const [galleryItems, setGalleryItems] = useState(fallbackGalleryItems);
    const [usingApiFeed, setUsingApiFeed] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();

    const handleLogout = () => {
        clearSession();
        navigate("/login");
    };

    // Load categories once.
    useEffect(() => {
        let cancelled = false;
        searchService
            .getCategories()
            .then((apiCategories) => {
                if (cancelled || !Array.isArray(apiCategories) || apiCategories.length === 0) return;
                const map = {};
                apiCategories.forEach((c) => { map[c.name] = c.id; });
                setCategoryMap(map);
                setCategories(["All", ...apiCategories.map((c) => c.name)]);
            })
            .catch(() => {
                // keep fallback categories
            });
        return () => { cancelled = true; };
    }, []);

    // Load the feed whenever the active category changes.
    useEffect(() => {
        let cancelled = false;
        setIsLoading(true);

        const categoryId = activeCategory !== "All" ? categoryMap[activeCategory] : undefined;

        feedService
            .getFeed({ page: 1, pageSize: 20, categoryId })
            .then((response) => {
                if (cancelled) return;
                const items = response?.items || response?.Items;
                if (!Array.isArray(items)) return;

                setGalleryItems(
                    items.map((item) => ({
                        id: item.id,
                        title: item.title,
                        category: item.category,
                        imageUrl: item.imageUrl,
                        isLiked: item.isLiked,
                        likeCount: item.likeCount,
                    }))
                );
                setUsingApiFeed(true);
                setFavorites(items.filter((i) => i.isLiked).map((i) => i.id));
            })
            .catch(() => {
                // keep whatever gallery items we already have (fallback or previous page)
            })
            .finally(() => {
                if (!cancelled) setIsLoading(false);
            });

        return () => { cancelled = true; };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [activeCategory, categoryMap]);

    const toggleFavorite = async (id) => {
        const isCurrentlyFavorite = favorites.includes(id);

        setFavorites((prev) =>
            isCurrentlyFavorite ? prev.filter((f) => f !== id) : [...prev, id]
        );

        if (!usingApiFeed) return; // fallback data has no real pin ids to call the API with

        try {
            if (isCurrentlyFavorite) {
                await pinsService.unlike(id);
            } else {
                await pinsService.like(id);
            }
        } catch (err) {
            // Roll back optimistic update on failure.
            setFavorites((prev) =>
                isCurrentlyFavorite ? [...prev, id] : prev.filter((f) => f !== id)
            );
        }
    };

    // Filtering by category and search (client-side search on top of
    // whatever the feed/category API returned).
    const filteredItems = galleryItems.filter(
        (item) =>
            (activeCategory === "All" || item.category === activeCategory) &&
            item.title.toLowerCase().includes(searchQuery.toLowerCase())
    );

    return (
        <div className="gallery-layout">
            <SidebarComponent activeItem="home" />

            {/* Main Content */}
            <main className="main-content">
                <header className="header-1">
                    <div className="header-content-1">
                        <SearchComponent onQueryChange={setSearchQuery} />
                        <div className="notification-btn">
                                <img src="/assets/icons/bell.png" alt="Favorite" />
                        </div>
                    </div>
                </header>
                
                <div className="with-scroll">
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
                            {/* Falls back to the local /assets/images pool when the item has no imageUrl */}
                            <div className="image-wrapper">
                                <img src={item.imageUrl || images[(item.id - 1 + images.length) % images.length]} alt={item.title} />
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
                    {!isLoading && filteredItems.length === 0 && (
                        <p style={{ color: "#9ca3af", padding: "1rem" }}>No pins found.</p>
                    )}
                </div>
                </div>
                
            </main>
        </div>
    );
};

export default HomePage;
