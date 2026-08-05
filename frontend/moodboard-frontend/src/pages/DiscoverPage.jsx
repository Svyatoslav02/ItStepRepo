import React from "react";
import { useNavigate } from "react-router-dom";
import "../styles/DiscoverPage.css";

const images = Array.from({ length: 4 }, (_, i) => `/assets/images/im${i + 1}.jpg`);

const DiscoverPage = () => {
    const navigate = useNavigate();

    return (
        <div className="container">
            <div className="header">
                <button className="back" onClick={() => navigate("/interests")}>←</button>
                <button className="skip" onClick={() => navigate("/signup")}>Skip</button>
            </div>

            <div className="content">
                <div className="text-block">
                    <span className="step-badge">Step 3 of 3</span>
                    <h1 className="title">Discover Ideas That Inspire You</h1>
                    <p className="subtitle">
                        Explore a world of creativity and find your next big project,
                        recipe, or style inspiration.
                    </p>
                    <div className="divider"></div>
                    <button className="nextBtn" onClick={() => navigate("/login")}>
                        Next
                    </button>
                </div>

                <div className="image-grid">
                    <div className="grid-item left-top">
                        <img src={images[0]} alt="left-top" />
                    </div>
                    <div className="grid-item right-top">
                        <img src={images[1]} alt="right-top" />
                    </div>
                    <div className="grid-item right-bottom">
                        <img src={images[2]} alt="right-bottom" />
                    </div>
                    <div className="grid-item bottom-full">
                        <img src={images[3]} alt="bottom-full" />
                    </div>
                </div>
            </div>
        </div>
    );
};

export default DiscoverPage;
