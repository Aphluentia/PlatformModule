import './PatientProfile.css';
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import SaveOutlinedIcon from '@mui/icons-material/SaveOutlined';
import Button from '@mui/material/Button';
import isBase64 from 'is-base64';



const PatientProfile = () => {
  const [patient, setPatient] = useState({});
  const [initialPatient, setInitialPatient] = useState({});
  const [editableField, setEditableField] = useState(null);
  const [changed, setChanged] = useState(false);
  const token = localStorage.getItem('Authentication');
  const email = localStorage.getItem('Email');

  useEffect(() => {
     getPatientData(token, email);
  }, []);

  const getPatientData= ()=>{
    axios
      .get("https://localhost:7176/api/Patient/" + token)
      .then(data => {
        console.log(data.data.data)
        setPatient(data.data.data);
        setInitialPatient(data.data.data)
      })
      .catch(error => {
        // Handle error
      });
  }
  const saveChanges=()=>{
      axios
        .put("https://localhost:7176/api/Patient/" + token, patient)
        .then(() => {
          getPatientData();
        })
        .catch(error => {
          // Handle error
        });
      setChanged(false);
  }
  const handleFieldClick = (fieldName) => {
    setEditableField(fieldName);
    setChanged(true);
    if (fieldName === null)
      saveChanges();
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setPatient({
      ...patient,
      [name]: value,
    });
  };
  function handleFileChange(event) {
      let reader = new FileReader();
      reader.readAsDataURL( event.target.files[0]);
        reader.onload = function () {
          console.log(reader.result);
          setPatient({
            ...patient,
            ['profilePicture']:reader.result,
          });
          setChanged(true);
      };
      reader.onerror = function (error) {
          console.log('Error: ', error);
      };
  }

return (
  
    <div className="patient-profile">
     
      {patient !== initialPatient && changed === false ? <p>Loading...</p> : (
          <>
            <div className="patient-picture">
              <span onClick={() => handleFieldClick('profilePicture')}>
                {isBase64(patient.profilePicture.split(",")[1]) ? (
                  <img
                    src={patient.profilePicture}
                    alt="Profile"
                    style={{ width: '100%', height: 'auto', maxWidth: '100px' }}
                    onClick={(e) => e.preventDefault()}
                  />
                  ):(
                    <p>Image Invalid</p>
                  )
                }
              </span>
                  
          </div>

          <div className="profile-details">
            <h2>
              {editableField === 'firstName' ? (
                <input
                  type="text"
                  name="firstName"
                  value={patient.firstName}
                  onChange={handleInputChange}
                  onBlur={() => setEditableField(null)}
                />
              ) : (
                <span onClick={() => handleFieldClick('firstName')}>
                  {patient.firstName}
                </span>
              )}{' '}
              {editableField === 'lastName' ? (
                <input
                  type="text"
                  name="lastName"
                  value={patient.lastName}
                  onChange={handleInputChange}
                  onBlur={() => setEditableField(null)}
                />
              ) : (
                <span onClick={() => handleFieldClick('lastName')}>
                  {patient.lastName}
                </span>
              )}
            </h2>
            <p>
              <b>Email:</b> {' '}
                <span onClick={() => handleFieldClick('email')}>{patient.email}</span>
            </p>
            <p>
              <b>Age:</b>{' '}
                {editableField === 'age' ? (
                  <input
                    type="number"
                    name="Age"
                    value={patient.age}
                    onChange={handleInputChange}
                    onBlur={() => setEditableField(null)}
                  />
                ) : (
                  <span onClick={() => handleFieldClick('age')}>{patient.age}</span>
                )}
            </p>
            <p>
            <b>Condition Type:</b>{' '}
              {editableField === 'conditionName' ? (
                <textarea 
                  type="text"
                  name="conditionName"
                  value={patient.conditionName}
                  onChange={handleInputChange}
                  onBlur={() => setEditableField(null)}
                />
              ) : (
                <span onClick={() => handleFieldClick('conditionName')}>{patient.conditionName}</span>
              )}
            </p>
            <p>
            <b>Country:</b>{' '}
              {editableField === 'countryCode' ? (
                <textarea 
                  type="text"
                  name="countryCode"
                  value={patient.countryCode}
                  onChange={handleInputChange}
                  onBlur={() => setEditableField(null)}
                />
              ) : (
                <span onClick={() => handleFieldClick('countryCode')}>{patient.countryCode}</span>
              )}
            </p>
            <p>
            <b>Phone Number:</b>{' '}
              {editableField === 'phoneNumber' ? (
                <textarea 
                  type="text"
                  name="phoneNumber"
                  value={patient.phoneNumber}
                  onChange={handleInputChange}
                  onBlur={() => setEditableField(null)}
                />
              ) : (
                <span onClick={() => handleFieldClick('phoneNumber')}>{patient.phoneNumber}</span>
              )}
            </p>
            {changed == true ? ( <Button  variant="contained"
              startIcon={<SaveOutlinedIcon />} 
              onClick={() => handleFieldClick(null)}>Save</Button>):
              <p></p>
            }
          <input type="file" onChange={handleFileChange} />
          </div>
          </>

      )}
    </div>
  );
};

export default PatientProfile;
