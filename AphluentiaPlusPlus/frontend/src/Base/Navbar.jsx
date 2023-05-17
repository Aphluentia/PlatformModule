import React from 'react';
import './Navbar.css';
import { Link } from 'react-router-dom';

export default function Navbar() {
  return (
    <>
      <header>  
        <nav >
          <ul className="nav-links">
        
            <li className="route"><Link to="/">Home</Link></li>
            <li className="route"><Link to="/login">Login</Link></li>
            <li className="route"><Link to="/dashboard">Dashboard</Link></li>
            <li className="route"><Link to="/modules">Modules</Link></li>
          </ul>
        </nav>
      </header>
  
    </>
    
  );
}
