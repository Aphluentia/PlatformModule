import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './Navbar.css';
import { useNavigate, Link, useLocation } from 'react-router-dom';


const Navbar = () => {
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    getSessionData();
  }, []);
 const getSessionData = () => {
  const token = localStorage.getItem("Token");
  axios
      .get("https://localhost:7176/api/Authentication/"+token)
      .then(data =>{
        if (!data.data.data['isExpired']){
          localStorage.setItem("fullName", data.data.data['fullName']);
          localStorage.setItem('Email', data.data.data['Email']);
          localStorage.setItem("UserType", data.data.data.userType);
          localStorage.setItem("Expires", data.data.data['expires']);
        }else{
          Logout();
        }
      } )
      .catch(error => {
        
          navigate("/Register");
      });
  
};
  const Logout = ()=>{
    localStorage.clear();
    navigate("/Login");
  }
  if (location.pathname.toLocaleLowerCase() === '/login' || location.pathname.toLocaleLowerCase() === '/register') {
    return null;
  }
  return (
    <>
      <header>  
        <nav >
          <ul className="nav-links">

            <li className="route"><Link to="/home">Home</Link></li>
            <li className="route"><Link to="/profile">Profile</Link></li>
            <li className="route"><Link to="/associations">Associations</Link></li>
            
          </ul>
          <ul className="options">
            <li  className="route" onClick={Logout}><a>Logout</a></li>
          </ul>
        </nav>
      </header>
  
    </>
    
  );
}
export default Navbar;
