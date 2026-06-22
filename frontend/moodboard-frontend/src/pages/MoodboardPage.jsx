// src/pages/MoodboardPage.jsx
import React, { useEffect, useState } from 'react';
import MoodboardGrid from '../components/MoodboardGrid';

/**
 * MoodboardPage is a React page component that fetches moodboard 
 * images from the backend API and displays them 
 * using the MoodboardGrid component.
 * @returns {JSX.Element} - Rendered moodboard page
 * @exeption {Error} - Throws an error if the API request fails
 */
const MoodboardPage = () => {
  const [images, setImages] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchMoodboard = async () => {
      try {
        const response = await fetch('/api/moodboard'); 
        if (!response.ok) {
          throw new Error('Failed to fetch moodboard');
        }
        const data = await response.json();
        setImages(data.images || []);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchMoodboard();
  }, []);

  if (loading) {
    return <div className="loading">Loading moodboard...</div>;
  }

  if (error) {
    return <div className="error">Error: {error}</div>;
  }

  return (
    <div className="moodboard-page">
      <h2>Moodboard</h2>
      <MoodboardGrid images={images} />
    </div>
  );
};

export default MoodboardPage;
