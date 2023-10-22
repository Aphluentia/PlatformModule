import React from 'react';
import axios from 'axios';
import {useContext, useState, useEffect} from 'react';
import { useNavigate } from 'react-router-dom';
import './Register.css';


const RegisterPage = () => {
  
  const [userType, setUserType] = useState(0);
  const [resultMessage, setResultMessage] = useState('');
  const [resultValue, setResultValue] = useState('');

  // General User Information
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [address, setAddress] = useState('');
  const [age, setAge] = useState(0);
  // Patient information
  const [conditionName, setConditionName] = useState('');
  const [conditionAcquisitionDate, setConditionAcquisitionDate] = useState('');
  // Therapist Information
  const [credentials, setCredentials] = useState('');
  const [description, setDescription] = useState('');
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
        var jsonData = 
        {
          'Email': email, 
          'Password': password,
          'FirstName': firstName,
          'LastName': lastName,
          'Age':age,
          'PhoneNumber':phoneNumber,
          'Address': address,
        };
        if (userType == 0){
          jsonData['ConditionName'] = conditionName;
          jsonData['ConditionAcquisitionDate'] = conditionAcquisitionDate;
            axios
            .post("https://localhost:9050/api/Patient/Register", jsonData)
            .then(data =>{
                setResultValue(true);
                setResultMessage(data.data.message);
                navigate("/login")
            } )
            .catch(error => {
              setResultValue(false);
              setResultMessage(error.response.data.message);
            });
        }else {
            jsonData['Credentials'] = credentials;
            jsonData['Description'] = description;
            axios
            .post("https://localhost:9050/api/AuthentiTherapist/Register", jsonData)
            .then(data =>{
                setResultValue(true);
                setResultMessage(data.data.message);
                navigate("/login")
            } )
            .catch(error => {
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
        <input
            type="number"
            placeholder="Phone Number"
            value={phoneNumber}
            onChange={(e) => setPhoneNumber(e.target.value)}
        />
        <input
            type="address"
            placeholder="Address"
            value={countyCode}
            onChange={(e) => setAddress(e.target.value)}
        />  
        <input
          type="number"
          placeholder="Age"
          value={age}
          onChange={(e) => setAge(e.target.value)}
        />
                 
        {
          userType == 0 ?
              <> 
              
                  <input
                      type="text"
                      placeholder="Condition Name"
                      value={conditionName}
                      onChange={(e) => setConditionName(e.target.value)}
                  />
                  <input
                      type="datetime"
                      placeholder="Condition Acquisition Date"
                      value={conditionAcquisitionDate}
                      onChange={(e) => setConditionAcquisitionDate(e.target.value)}
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
