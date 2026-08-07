import React, { useState, useEffect } from "react";
import "../styles/WelcomePage.css";

const images = Array.from({ length: 6 }, (_, i) => `src/assets/images/welcome-page-${i + 1}.jpg`);

const WelcomePage = () => {
    const [currentStep, setCurrentStep] = useState(0);
    const [isMobile, setIsMobile] = useState(window.innerWidth <= 768);

    const handleNext = () => {
        console.log("Next step");
    };

    const handleSkip = () => {
        console.log("Skip onboarding");
    };

    const handlePrevious = () => {
        setCurrentStep(Math.max(0, currentStep - 1));
    };

    const handleNavNext = () => {
        setCurrentStep(Math.min(2, currentStep + 1));
    };

    useEffect(() => {
        const handler = () => setIsMobile(window.innerWidth <= 768);
        window.addEventListener("resize", handler);
        return () => window.removeEventListener("resize", handler);
    }, []);

    const buttons = (
        <div className="action-buttons">
            <button className="btn btn-primary" onClick={handleNext}>Continue</button>
            <button className="btn btn-secondary" onClick={handleSkip}>Skip for now</button>
        </div>
    );
    
    return (
        <div className="ink-onboarding">
            {/* Header Navigation */}
            <div className="onboarding-header">
                <button className="nav-chevron" onClick={handlePrevious}>
                    ‹
                </button>
                <h1 className="header-title">Ink</h1>
                <button className="nav-chevron" onClick={handleNavNext}>
                    ›
                </button>
            </div>

            {/* Main Content */}
            <div className="onboarding-container">
                {/* Left Column - Text Content (Mobile: Top, Desktop: Left) */}
                <div className="content-column">
                    {/* Step Indicator */}
                    <div className="step-indicator">
                        <span>Step 1 of 2</span>
                    </div>

                    {/* Text Content */}
                    <div className="text-content">
                        <h2 className="welcome-title">Welcome to Ink</h2>
                        <p className="welcome-description">
                            Discover millions of AI-curated ideas tailored to your style.
                        </p>
                        <p className="welcome-subtitle">
                            Next, you'll choose your core design interests.
                        </p>
                    </div>
                    {!isMobile && buttons}
                </div>

                <div className="image-column">
                    <div className="image_grid">
                        {images.map((src, i) => (
                            <div key={i} className={`image-cell image-${i + 1}`}>
                                <img src={src} alt={`Ink inspiration ${i + 1}`} />

                                {((isMobile && i === 1) || (!isMobile && i === 4)) && (
                                    <div className="search-layer">
                                        <div className="search-logo">
                                            <img src="src/assets/images/search-01.png" alt="search"/>
                                        </div>
                                    </div>
                                )}
                            </div>
                        ))}
                    </div>
                </div>
                {isMobile && buttons}
            </div>
        </div>
    );
};

export default WelcomePage;
