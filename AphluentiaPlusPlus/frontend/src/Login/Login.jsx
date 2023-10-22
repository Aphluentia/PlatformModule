import React from 'react';
import axios from 'axios';
import {useContext, useState, useEffect} from 'react';
import { useNavigate } from 'react-router-dom';
import './Login.css';


const LoginPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [userType, setUserType] = useState('0');
  const [resultMessage, setResultMessage] = useState('');
  const [resultValue, setResultValue] = useState('');
  if (location.pathname === "/login" || location.pathname === "/signup") {
    return null;
  }
  const navigate = useNavigate();
  const NavigateToSignup = () => {
    navigate("/Signup");
  }
  function validateForm() {
    return email.length > 0 && password.length > 0;
  }
  const handleUserTypeChange = (e) => {
    setUserType(e.target.value);
  };

  const handleLogin = (e) => {
    var user = parseInt(userType);
    var jsonData = {'email': email, 'password': password, 'userType': userType === '0' ? 0 : 1};
    e.preventDefault();
    axios
        .post("https://localhost:7176/api/Authentication/Login", jsonData)
        .then(data =>{
            if (data.data.data!==undefined){
                console.log(data.data.data);
                localStorage.setItem("Token", data.data.data);
                localStorage.setItem("UserType", userType === '0' ? 0 : 1);
                localStorage.setItem('isLoggedIn','true');
                setResultValue(true);
                setResultMessage(data.data.message);
                navigate("/home");
            }
            
          

        } )
        .catch(error => {
          console.log(error);
          setResultValue(false);
          if (error.response == undefined) setResultMessage("Network Error");
          else setResultMessage(error.response.data.message);
        });
    
  };

  return (
    <div className="page">
       <div className="login-container">
    <h2>Login Page</h2>
    <form onSubmit={handleLogin}>
      <input
        type="email"
        placeholder="Email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
      />
      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <label >
        Login As:
        <select value={userType} onChange={handleUserTypeChange}>
          <option value='0'>Patient</option>
          <option value='1'>Therapist</option>
        </select>
      </label>
      <button className="login-button" type="submit">Login</button>
      <input type="button" className="register-button" onClick={NavigateToSignup} value="Register"/>
      {resultValue ? (
        <p className="Success">{resultMessage}</p>
      ) : (
        <p  className="Error">{resultMessage}</p>
      )}

    </form>
  </div>
    </div>
   
  );
};

export default LoginPage;
