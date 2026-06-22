// MoodboardGrid.jsx
import React from 'react';
import './assets/styles/MoodboardGrid.css';

/**
 * MoodboardGrid component
 * Renders a grid of moodboard images with optional titles and source links. 
 * @param {Array} images - Array of image objects
 * @returns {JSX.Element} - Rendered moodboard grid
 */
const MoodboardGrid = ({ images }) => {
  if (!images || images.length === 0) {
    return <div className="empty-state">No moodboard images available.</div>;
  }

  return (
    <div className="moodboard-grid">
      {images.map((img, index) => (
        <div key={index} className="moodboard-item">
          <img src={img.url} alt={img.title || `Moodboard ${index + 1}`} />
          {img.title && <h4 className="image-title">{img.title}</h4>}
          {img.source && (
            <a href={img.source} target="_blank" rel="noopener noreferrer" className="source-link">
              Source
            </a>
          )}
        </div>
      ))}
    </div>
  );
};

export default MoodboardGrid;
