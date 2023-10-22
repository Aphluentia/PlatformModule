import React from 'react';
import './SearchBar.css'

const SearchBar = ({ placeholder, onChange }) => {
    return (
        <div className="search-container">
            <input 
                type="text" 
                className="search-input" 
                placeholder={placeholder} 
                onChange={onChange}
            />
        </div>
    );
}

export default SearchBar;
