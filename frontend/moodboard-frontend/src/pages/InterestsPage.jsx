import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/InterestsPage.css";

const interestsList = [
    "Eco", "Asymmetry", "Collage", "Minimal", "Vector",
    "3D Art", "Zen", "Retro", "Art", "Texture"
];

const InterestsPage = () => {
    const [selected, setSelected] = useState([]);
    const navigate = useNavigate();

    const toggleInterest = (interest) => {
        setSelected((prev) =>
            prev.includes(interest)
                ? prev.filter((i) => i !== interest)
                : [...prev, interest]
        );
    };

    const canProceed = selected.length >= 3;

    return (
        <div className="container">
            
            <div className="header">
                <span className="step">Step 1 of 3</span>
                <button className="skip">Skip</button>
            </div>

            <div className="content">
                <div className="text-block">
                    <h1 className="title">What are you into?</h1>
                    <p className="subtitle">
                        Select at least 3 interest to personalize your feed
                    </p>
                    <div className="divider"></div>
                    <button
                        className={`nextBtn ${!canProceed ? "disabled" : ""}`}
                        disabled={!canProceed}
                        onClick={() => navigate("/discover")}
                    >
                        Next
                    </button>
                </div>

                <div className="grid">
                    {interestsList.map((interest) => (
                        <button
                            key={interest}
                            className={`card ${selected.includes(interest) ? "selected" : ""}`}
                            onClick={() => toggleInterest(interest)}
                        >
                            {interest}
                            {selected.includes(interest) && <span className="check">✓</span>}
                        </button>
                    ))}
                </div>
            </div>
        </div>
    );
}

export default InterestsPage;