import React from 'react';
import {useContext, useState, useEffect} from 'react';
import Navbar from '../Base/Navbar.jsx'
import TherapistPage from './TherapistPage/TherapistPage.jsx';
import PatientPage from './PatientPage/PatientPage.jsx';

const DashboardPage = () => {
  var storedSessionData = {};
  const userType = localStorage.getItem("userType");
  useEffect(() => {
    storedSessionData = localStorage.getItem('SessionData');
   
  }, []);
  return (<>
    <Navbar sessionData={storedSessionData}/>
    <div className="AppBody"> 
      {userType == 0 ? (
        <PatientPage/>
      ) : (
        <TherapistPage/>
      )}

    </div></>
    
  );
};

export default DashboardPage;
