import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/InterestsPage.css";

const interestsList = [
    "Minimal", "3D Art", "App Mobile", "Retro", "Photography",
    "Architecture", "Modern", "Art", "Eco", "Prints"
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
                <button className="back" onClick={() => navigate("/inspiration")}>←</button>
                <button className="skip" onClick={() => navigate("/signup")}>Skip</button>
            </div>

            <div className="content">
                <div className="text-block">
                    <span className="step-badge">Step 2 of 3</span>
                    <h1 className="title">What are you into?</h1>
                    <p className="subtitle">
                        Select at least 3 interests to personalize your feed
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
                        <div
                            key={interest}
                            className={`card ${selected.includes(interest) ? "selected" : ""}`}
                            onClick={() => toggleInterest(interest)}
                        >
                            <input
                                type="checkbox"
                                checked={selected.includes(interest)}
                                readOnly
                                className="card-checkbox"
                            />
                            <span className="card-text">{interest}</span>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default InterestsPage;