import React from 'react';
import './Navbar.css';
import { Link } from 'react-router-dom';

const Navbar = () => {
  return (
    <>
      <header>  
        <nav >
          <ul className="nav-links">
        
            <li className="route"><Link to="/">Home</Link></li>
            <li className="route"><Link to="/login">Login</Link></li>
          </ul>
        </nav>
      </header>
  
    </>
    
  );
}
export default Navbar;
