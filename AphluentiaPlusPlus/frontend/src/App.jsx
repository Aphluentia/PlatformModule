
import './App.css'
import Navbar from './Base/Navbar.jsx'
import { BrowserRouter, Route, Routes} from "react-router-dom";
import Modules from './Modules/Modules.jsx';
import Dashboard from './Dashboard/Dashboard.jsx';
import Login from './Login/Login.jsx';
import {Context, SessionData} from './Context'
import {useContext, useState, useEffect} from 'react';

import Home from './Home/Home';
function App() {
  
  const [sessionData, setSessionData] = useState(null);
  const [isLoggedIn, setIsLoggedIn] = useState(null);

  useEffect(() => {
    const storedSessionData = localStorage.getItem('Authentication') || null;
    const userEmail = localStorage.getItem('UserEmail') || null;
    const WebPlatformId = localStorage.getItem('WebPlatformId') || null;
    console.log(storedSessionData!==null && userEmail!==null && WebPlatformId!==null)
    if (storedSessionData!==null && userEmail!==null && WebPlatformId!==null ) {
      setIsLoggedIn(true);
      setSessionData(storedSessionData);
    }
  }, []);
  

 
  return <BrowserRouter>
        {isLoggedIn == true ? 
        <>
          <Navbar sessionData={sessionData}/>
          <div className="AppBody"> 
            <Routes>
              <Route path="/" element={<Home/>} />
              <Route path="/login" element={<Login/>} />
              <Route path="/modules" element={<Modules sessionData={sessionData}/>} />
              <Route path="/dashboard" element={<Dashboard/>} />
            </Routes>
          </div>
          </>
        :
            <Login></Login>
          }
        </BrowserRouter>
      
}

export default App
