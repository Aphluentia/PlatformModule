import React from 'react';
import axios from 'axios';
import {useContext, useState, useEffect} from 'react';
import { useNavigate } from 'react-router-dom';
import './Login.css';


const LoginPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [userType, setUserType] = useState(0);
  const [resultMessage, setResultMessage] = useState('');
  const [resultValue, setResultValue] = useState('');

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
    var jsonData = {'Email': email, 'Password': password, 'UserType':user};
    console.log(jsonData);
    e.preventDefault();
    axios
        .post("https://localhost:7176/api/Authentication/Login", jsonData)
        .then(data =>{
            console.log(JSON.stringify(data));
            if (data.data['token']!==undefined){
                localStorage.setItem("Authentication", data.data['token']);
                localStorage.setItem('Email', jsonData['email']);
                localStorage.setItem("userType", jsonData['userType']);
                localStorage.setItem("fullName", data.data['fullName']);
                localStorage.setItem('isLoggedIn','true');
            }
            setResultValue(true);
            setResultMessage(data.data.message);
            console.log(user)
            if (user == 0 )
              navigate("/Dashboard");
          

        } )
        .catch(error => {
          
          setResultValue(false);
          if (error.response == undefined) setResultMessage("Network Error");
          else setResultMessage(error.response.data.message);
        });
    
  };

  return (
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
          <option value="0">Patient</option>
          <option value="1">Therapist</option>
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
  );
};

export default LoginPage;
