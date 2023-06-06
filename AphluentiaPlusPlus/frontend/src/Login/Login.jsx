import React from 'react';
import axios from 'axios';
import {useContext, useState, useEffect} from 'react';
import { useNavigate } from 'react-router-dom';


const LoginPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

    
  const handleLogin = (e) => {
    var jsonData = {'Email': email, 'Password': password};
    e.preventDefault();
    axios
        .post("https://localhost:7176/api/Authentication/Login", jsonData)
        .then(data =>{
            if (data.data['token']!==undefined){
                localStorage.setItem("Authentication", data.data['token']);
                axios
                    .get(`https://localhost:7176/api/Authentication/User?Token=${data.data['token']}`)
                    .then(userData =>{
                        if (userData.data.user!==undefined){
                            localStorage.setItem("UserEmail", userData.data.user.email);
                            localStorage.setItem("WebPlatformId", userData.data.user['webPlatformId']);
                            
                            navigate("/Home");
                        }
                    } )
                    .catch(error => console.log(error));
    
            }

        } )
        .catch(error => console.log(error));
    
  };

  return (
    <div>
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
        <button type="submit">Login</button>
      </form>
    </div>
  );
};

export default LoginPage;
