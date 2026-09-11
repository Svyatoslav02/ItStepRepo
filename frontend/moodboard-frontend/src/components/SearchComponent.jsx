import React, {useState} from "react";
import "./SearchComponent.css";

const SearchComponent = () => {
    const [searchQuery, setSearchQuery] = useState("");
  return (
      <div className="search_wrapper">
          <img src="/assets/images/search-01.png" alt="Search" className="search-icon" />
          <input
              type="text"
              placeholder="Search for ideas"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
          />
          <img src="\src\assets\images\cancel.png" alt="Clear" className="clear-icon" onClick={() => setSearchQuery("")} />
      </div>
  );
};

export default SearchComponent;