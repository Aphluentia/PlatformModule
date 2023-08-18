import React from 'react';
import axios from 'axios';
import {useContext, useState, useEffect} from 'react';
import { useNavigate } from 'react-router-dom';
import Navbar from '../Base/Navbar.jsx'

const DashboardPage = () => {
  const navigate = useNavigate();
  var storedSessionData = {};
  useEffect(() => {
    storedSessionData = localStorage.getItem('SessionData');
   
  }, []);
  return (
    <div className="AppBody"> 
      <Navbar sessionData={storedSessionData}/>
    </div>
  );
};

export default DashboardPage;
