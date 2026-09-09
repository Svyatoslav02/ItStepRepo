import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/LoadingScreen.css";

const images = Array.from({ length: 21 }, (_, i) => `/assets/images/img${i + 1}.jpg`);

const LoadingScreen = () => {
    const navigate = useNavigate();

    useEffect(() => {
        const timer = setTimeout(() => {
            navigate("/inspiration");
        }, 2000);

        return () => clearTimeout(timer); // cleanup the timer on component unmount
    }, [navigate]);

    return (
        <div className="loading-screen">
            <div className="image-grid">
                {images.map((src, i) => (
                    <div key={i} className="image-cell">
                        <img src={src} alt={`Artwork ${i + 1}`} />
                    </div>
                ))}
            </div>

            <div className="overlay">
                <h1 className="logo">Ink</h1>
                <div className="spinner"></div>
                <p className="loading-text">Loading</p>
            </div>
        </div>
    );
};

export default LoadingScreen;
