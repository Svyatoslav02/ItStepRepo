/**
 * InterestsPage — Step 2 of the onboarding flow.
 *
 * Displays a masonry grid of interest topics (Abstract, Branding,
 * Photography, Illustration, UI/UX, Nature, Typography, Fashion).
 * The user must select at least 3 topics before proceeding.
 *
 * Layout:
 *  - Desktop: left panel (text + actions) + 4-column asymmetric grid
 *  - Tablet:  left panel + 3-column grid, actions stacked vertically
 *  - Mobile:  stacked column layout, 2-column grid
 *
 * @component
 */
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/InterestsPage.css";

const ICON_BASE = "/assets/icons";

const topics = [
  { id: 1, label: "Abstract",     image: "/assets/images/ins1.jpg", area: "abstract"     },
  { id: 2, label: "Branding",     image: "/assets/images/ins3.jpg", area: "branding"     },
  { id: 3, label: "Photography",  image: "/assets/images/ins5.jpg", area: "photography"  },
  { id: 4, label: "Illustration", image: "/assets/images/ins7.jpg", area: "illustration" },
  { id: 5, label: "UI / UX",      image: "/assets/images/ins4.jpg", area: "uiux"         },
  { id: 6, label: "Nature",       image: "/assets/images/ins6.jpg", area: "nature"       },
  { id: 7, label: "Typography",   image: "/assets/images/ins2.jpg", area: "typography"   },
  { id: 8, label: "Fashion",      image: "/assets/images/ins8.jpg", area: "fashion"      },
];

const columnLayout = [
  ["abstract", "typography"],
  ["branding", "uiux"],
  ["photography", "nature"],
  ["illustration", "fashion"],
];

const topicByArea = Object.fromEntries(topics.map((t) => [t.area, t]));

const TOTAL_STEPS = 2;
const CURRENT_STEP = 2;
const MIN_SELECTED = 3;

/**
 * InterestCard — a single selectable topic card.
 *
 * Shows a background image, an icon, a label, and a checkmark
 * indicator in the top-right corner. Toggled on click or Enter key.
 *
 * @param {object}   topic       - Topic data (id, label, image, area).
 * @param {boolean}  isSelected  - Whether the card is currently selected.
 * @param {Function} onToggle    - Callback invoked with topic.id on toggle.
 */
function InterestCard({ topic, isSelected, onToggle }) {
  return (
    <div
      className={`ip-card${isSelected ? " ip-card-selected" : ""}`}
      onClick={() => onToggle(topic.id)}
      role="checkbox"
      aria-checked={isSelected}
      tabIndex={0}
      onKeyDown={(e) => e.key === "Enter" && onToggle(topic.id)}
    >
      <img src={topic.image} alt="" className="ip-card-img" />
      <div className="ip-card-overlay" />
      <div className="ip-card-content">
        <img src={`${ICON_BASE}/${topic.area}.svg`} alt="" className="ip-card-icon" />
        <span className="ip-card-label">{topic.label}</span>
      </div>
      <div className={`ip-checkmark${isSelected ? " ip-checkmark-active" : ""}`}>
        {isSelected && <span className="ip-check-icon">✓</span>}
      </div>
    </div>
  );
}

/**
 * Root component for the Interests onboarding screen.
 * Manages the set of selected topic ids and renders the
 * panel + grid layout.
 */
export default function InterestsPage() {
  const [selected, setSelected] = useState(new Set());
  const navigate = useNavigate();

  const toggle = (id) => {
    setSelected((prev) => {
      const next = new Set(prev);
      next.has(id) ? next.delete(id) : next.add(id);
      return next;
    });
  };

  const canContinue = selected.size >= MIN_SELECTED;

  return (
    <div className="ip-root">
      <header className="ip-header">
        <button className="ip-nav-btn" onClick={() => navigate(-1)} aria-label="Back">‹</button>
        <span className="ip-logo">Ink</span>
        <button className="ip-nav-btn" onClick={() => navigate("/discover")} aria-label="Next">›</button>
      </header>

      <main className="ip-main">
        <div className="ip-panel">
          <span className="ip-step-badge">Step {CURRENT_STEP} of {TOTAL_STEPS}</span>
          <h1 className="ip-heading">What inspires you?</h1>
          <p className="ip-body">
            Choose the themes you love.<br />
            We'll personalize your experience with ideas that match your style.
          </p>
          <div className="ip-hint">
            <img src={`${ICON_BASE}/lightbulb.svg`} alt="" className="ip-hint-icon" />
            <div>
              <p className="ip-hint-title">Select at least {MIN_SELECTED} topics</p>
              <p className="ip-hint-sub">You can change this anytime.</p>
            </div>
          </div>
          <div className="ip-actions">
            <button
              className={`ip-btn-primary${canContinue ? "" : " ip-btn-disabled"}`}
              disabled={!canContinue}
              onClick={() => navigate("/discover")}
            >
              Continue
            </button>
            <button className="ip-btn-ghost" onClick={() => navigate("/discover")}>
              Skip for now
            </button>
          </div>
        </div>

        <div className="ip-grid">
          {columnLayout.map((areas, colIdx) => (
            <div key={areas.join("-")} className={`ip-col ip-col-${colIdx + 1}`}>
              {areas.map((area) => {
                const topic = topicByArea[area];
                return (
                  <InterestCard
                    key={topic.id}
                    topic={topic}
                    isSelected={selected.has(topic.id)}
                    onToggle={toggle}
                  />
                );
              })}
            </div>
          ))}
        </div>
      </main>
    </div>
  );
}
