import React from 'react';
import './Navbar.css';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';

const Navbar = (sessionData) => {
  const navigate = useNavigate();
  const Logout = ()=>{
    localStorage.clear();
    navigate("/Login");
  }
  console.log(sessionData)
  return (
    <>
      <header>  
        <nav >
          <ul className="nav-links">

            <li className="route"><Link to="/">Home</Link></li>
            <li className="route"><Link to="/login">Login</Link></li>
            <li><input type="button" className="logout-button" onClick={Logout} value="Logout"/></li>
          </ul>
        </nav>
      </header>
  
    </>
    
  );
}
export default Navbar;
