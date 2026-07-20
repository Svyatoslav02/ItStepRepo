import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/DiscoverPage.css";

const images = Array.from({ length: 4 }, (_, i) => `/assets/images/im${i + 1}.jpg`);

const DiscoverPage = () => {
    const [active, setActive] = useState(false);
    const navigate = useNavigate();
    const handleNextClick = () => {
        setActive(!active);
        navigate("/interests");
    };
    return (
        <div className="container">
            <div className="header">
                <span className="step">Step 1 of 3</span>
                <button className="skip">Skip</button>
            </div>

            <div className="content">
                <div className="text-block">
                    <h1 className="title">Discover Ideas That Inspire You</h1>
                    <p className="subtitle">
                        Explore a world of creativity and find your next big project,
                        recipe, or style inspiration.
                    </p>
                    <div className="divider"></div>
                    <button className={`next-btn ${active ? "active" : ""}`} onClick={handleNextClick}>
                        Next
                    </button>
                </div>

                <div className="image-grid">
                    {images.map((src, i) => (
                        <img key={i} src={src} alt={`idea-${i}`} className="grid-img" />
                    ))}
                </div>
            </div>
        </div>
    );
}

export default DiscoverPage;
