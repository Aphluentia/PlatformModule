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
          "profilePicture": "data:image/png;base64,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"
        };
        if (userType == 0){
          jsonData['ConditionName'] = conditionName;
          jsonData['ConditionAcquisitionDate'] = conditionAcquisitionDate;
            axios
            .post("https://localhost:7176/api/Patient/Signup", jsonData)
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
            .post("https://localhost:7176/api/AuthentiTherapist/Register", jsonData)
            .then(data =>{
                setResultValue(true);
                setResultMessage(data.data.message);
                navigate("/login")
            } )
            .catch(error => {
              setResultValue(false);
              console.log(error.response);
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
            value={address}
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
                      type="date"
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
        <button className="register-button"  type="submit">Register</button>
        {resultValue ? (
          <p className="Success">{resultMessage}</p>
        ) : (
          <p  className="Error">{resultMessage}</p>
        )}
        
        <input type="button" className="login-button" onClick={NavigateToLogin} value="Go To Login"/>

      </form>
    </div>
  </div>
  );
};

export default RegisterPage;
