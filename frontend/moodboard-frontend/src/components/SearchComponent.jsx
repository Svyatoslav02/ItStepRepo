import React, {useState} from "react";
import "./SearchComponent.css";

// onSearchSubmit: optional callback(query) fired on Enter.
// onQueryChange: optional callback(query) fired on every keystroke.
const SearchComponent = ({ onSearchSubmit, onQueryChange }) => {
    const [searchQuery, setSearchQuery] = useState("");

    const handleChange = (e) => {
        const value = e.target.value;
        setSearchQuery(value);
        if (onQueryChange) onQueryChange(value);
    };

    const handleKeyDown = (e) => {
        if (e.key === "Enter" && onSearchSubmit) {
            onSearchSubmit(searchQuery);
        }
    };

    const handleClear = () => {
        setSearchQuery("");
        if (onQueryChange) onQueryChange("");
    };

  return (
      <div className="search_wrapper">
          <img src="/assets/images/search-01.png" alt="Search" className="search-icon" />
          <input
              type="text"
              placeholder="Search for ideas"
              value={searchQuery}
              onChange={handleChange}
              onKeyDown={handleKeyDown}
          />
          <img src="\src\assets\images\cancel.png" alt="Clear" className="clear-icon" onClick={handleClear} />
      </div>
  );
};

export default SearchComponent;