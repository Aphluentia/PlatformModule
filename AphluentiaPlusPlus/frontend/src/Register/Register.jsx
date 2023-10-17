import React from 'react';
import axios from 'axios';
import {useContext, useState, useEffect} from 'react';
import { Navigate} from "react-router-dom";
import { useNavigate } from 'react-router-dom';
import './Register.css';


const RegisterPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [countyCode, setCountryCode] = useState('+351');
  const [age, setAge] = useState(0);
  const [conditionName, setConditionName] = useState('');
  const [credentials, setCredentials] = useState('');
  const [description, setDescription] = useState('');
  const [userType, setUserType] = useState(0);
  const [resultMessage, setResultMessage] = useState('');
  const [resultValue, setResultValue] = useState('');
  const navigate = useNavigate();

  function validateForm() {
    return email.length > 0 && password.length > 0 && firstName.length > 0 && lastName.length > 0 && age>0;
  }
  const handleUserTypeChange = (e) => {
    setUserType(e.target.value);
  };
  const NavigateToLogin = () => {
    navigate("/Login");
  }

  const handleRegister = (e) => {
  
    e.preventDefault();
        console.log(userType)
        if (userType == 0){
            var jsonData = 
            {
                'Email': email, 
                'Password': password,
                'FirstName': firstName,
                'LastName': lastName,
                'Age':age,
                'PhoneNumber':phoneNumber,
                'CountryCode': countyCode,
                'ConditionName':conditionName
            };
            axios
            .post("https://localhost:7176/api/Authentication/Signup/Patient", jsonData)
            .then(data =>{
                console.log(data)
                setResultValue(true);
                setResultMessage(data.data.message);
                navigate("/login")
              
    
            } )
            .catch(error => {
              console.log(error)
              setResultValue(false);
              setResultMessage(error.response.data.message);
            });
        }else {
            var jsonData = 
            {
                'Email': email, 
                'Password': password,
                'FirstName': firstName,
                'LastName': lastName,
                'Age':age,
                'Credentials': credentials,
                'Description': description
            };
            axios
            .post("https://localhost:7176/api/Authentication/Signup/Therapist", jsonData)
            .then(data =>{
                console.log(data)
                setResultValue(true);
                setResultMessage(data.data.message);
                navigate("/login")
              
    
            } )
            .catch(error => {
              console.log(error)
              setResultValue(false);
              setResultMessage(error.response.data.message);
            });
        }
      
   
    
  };


  return (
    <div className="page">
      <div className="register-container">
      <h2>Login Page</h2>
      <form onSubmit={handleRegister}>
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
        <input
          type="text"
          placeholder="First Name"
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
        />
        <input
          type="text"
          placeholder="LastName"
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
        />
      
        {
          userType == 0 ?
              <> 
                <input
                      type="text"
                      placeholder="Country Code"
                      value={countyCode}
                      onChange={(e) => setCountryCode(e.target.value)}
                  />
                  <input
                      type="number"
                      placeholder="Phone Number"
                      value={phoneNumber}
                      onChange={(e) => setPhoneNumber(e.target.value)}
                  />
                  <input
                      type="text"
                      placeholder="Condition Name"
                      value={conditionName}
                      onChange={(e) => setConditionName(e.target.value)}
                  />
              </>
              :
              <>
                  <input
                      type="text"
                      placeholder="Description"
                      value={description}
                      onChange={(e) => setDescription(e.target.value)}
                  />
                  <input
                      type="text"
                      placeholder="Credentials"
                      value={credentials}
                      onChange={(e) => setCredentials(e.target.value)}
                  />
              </>
        }
        
          <input
          type="number"
          placeholder="Age"
          value={age}
          onChange={(e) => setAge(e.target.value)}
        />
        <label >
          Register As:
          <select value={userType} onChange={handleUserTypeChange}>
            <option value="0">Patient</option>
            <option value="1">Therapist</option>
          </select>
        </label>
        <input type="button" className="login-button" onClick={NavigateToLogin} value="Login"/>
        <button className="register-button"  type="submit">Register</button>
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

export default RegisterPage;
