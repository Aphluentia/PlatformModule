
import './App.css'
import { Route, Routes} from "react-router-dom";
import Login from './Login/Login.jsx';
import Register from './Register/Register.jsx';
import Navbar from './Base/Navbar.jsx';
import Dashboard from './Dashboard/Dashboard.jsx';
import Profile from './Profile/ProfilePage.jsx';
import {useEffect} from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import AssociationsPage from './Associations/AssociationsPage';
import ModuleDashboardPage from './Modules/ModuleDashboard';

function App() {
  const isLoggedIn = localStorage.getItem('isLoggedIn') === 'true';
  const navigate = useNavigate();
  let userType = 0;
  useEffect(() => {
    const token = localStorage.getItem('Authentication') || null;
    if (token!==null) {
      axios
        .get("https://localhost:7176/api/Authentication/"+token)
        .then(data =>{
            if (data.data['isExpired']){
                localStorage.setItem('Email', data.data.userDetails['email']);
                localStorage.setItem('fullName', data.data.userDetails['fullName']);
                localStorage.setItem('UserType', data.data.userDetails['userType']);
                userType = data.data.userDetails['userType'];
                localStorage.setItem('Expires', data.data.userDetails['expires']);
                localStorage.setItem('isLoggedIn','true');
                navigate("/home")
            }else{
              navigate("/Login")
            }

        } )
        .catch(error => navigate("/Login"));
    }
  }, []);
 
  //  <Route path="/modules/:moduleId" element={<Module/>} />
  
  return <>
      { (isLoggedIn == true) ? 
        ( <>
        <Navbar/>
          <Routes >
            <Route path="/login" element={<Login/>} />
            <Route path="/signup" element={<Register/>} />
            <Route path="/home" element={<Dashboard/>} />
            <Route path="/associations" element={<AssociationsPage/>} />
            <Route path="/profile" element={<Profile/>} />
            <Route path="/modules/:moduleId/:patient" element={<ModuleDashboardPage/>} />

          </Routes>   
          </>   
        ) : (     
            <Routes >
              <Route path="/login" element={<Login/>} />
              <Route path="/signup" element={<Register/>} />
            </Routes>    
        )};
    </>
};


export default App;
