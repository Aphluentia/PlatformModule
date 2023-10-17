
import './App.css'
import { Route, Routes} from "react-router-dom";
import Login from './Login/Login.jsx';
import Register from './Register/Register.jsx';
import Dashboard from './Dashboards/Dashboard.jsx';
import {useEffect} from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

function App() {
  const isLoggedIn = localStorage.getItem('isLoggedIn') === 'true';
  const navigate = useNavigate();
  useEffect(() => {
    const storedSessionData = localStorage.getItem('Authentication') || null;
    console.log(storedSessionData)
    if (storedSessionData!==null) {
      axios
        .get("https://localhost:7176/api/Authentication/Validate/"+storedSessionData)
        .then(data =>{
            if (data.data['isValidSession']){
              console.log(data.data)
                localStorage.setItem('Email', data.data.userDetails['email']);
                localStorage.setItem('fullName', data.data.userDetails['fullName']);
                localStorage.setItem('isLoggedIn','true');
                navigate("/Dashboard")
            }else{
              navigate("/Login")
            }

        } )
        .catch(error => navigate("/Login"));
    }
  }, []);
 

 
  return <Routes >
          <Route path="/login" element={<Login/>} />
          <Route path="/signup" element={<Register/>} />
          {isLoggedIn == true ? 
          <>
            <Route path="/dashboard" element={<Dashboard/>} />
            
          </>

          :
          <>
          </>
          }
        
        </Routes>
      
}

export default App
