import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/InspirationLoading.css";

const images = Array.from({ length: 4 }, (_, i) => `/assets/images/inspire${i + 1}.jpg`);

const InspirationLoading = () => {
    const [active, setActive] = useState(false);
    const navigate = useNavigate();

    const handleNextClick = () => {
        setActive(true);
        navigate("/login");
    };

    const handleSkipClick = () => {
        navigate("/login");
    };

    return (
        <div className="inspiration-screen">
            <button className="skip-btn" onClick={handleSkipClick}>
                Skip
            </button>

            <div className="content">
                <div className="text-section">
                    <span className="step-badge">Step 1 of 3</span>
                    <h1 className="title">Find your inspiration</h1>
                    <p className="subtitle">
                        Explore millions of ideas tailored to your unique taste and interests.
                    </p>
                    <div className="divider"></div>
                    <button
                        className={`next-btn ${active ? "active" : ""}`}
                        onClick={handleNextClick}
                    >
                        Next
                    </button>
                </div>

                <div className="image-grid">
                    {images.map((src, i) => (
                        <div key={i} className="image-cell">
                            <img src={src} alt={`Artwork ${i + 1}`} />
                        </div>
                    ))}
                    <div className="search-overlay">
                        <div className="search-circle">
                            <img
                                src="/assets/images/search.png"
                                alt="Search icon"
                                className="search-icon"
                            />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default InspirationLoading;
